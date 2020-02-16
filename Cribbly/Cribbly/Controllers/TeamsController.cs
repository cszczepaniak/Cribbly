using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cribbly.Data;
using Cribbly.Models;
using Cribbly.Models.Gameplay;
using Microsoft.AspNetCore.Authorization;

namespace Cribbly.Controllers
{
    //Require user to be logged in to access any endpoint below
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _user;

        public TeamsController(ApplicationDbContext context, UserManager<IdentityUser> user)
        {
            _context = context;
            _user = user;
        }

        /*
         * ==============================
         * VIEW ALL TEAMS (Admin)
         * ==============================
         */

        // GET: Teams
        public async Task<IActionResult> AllTeams()
        {
            return View(await _context.Teams.ToListAsync());
        }

        /*
         * ==============================
         * VIEW YOUR TEAM (User)
         * ==============================
         */

        // GET: Teams/MyTeam
        public async Task<IActionResult> MyTeam(int id)
        {
            var loggedInUser = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _user.GetUserName(User));

            //User does not have a TeamID yet
            if (id == 0)
            {
                return RedirectToAction(nameof(TeamNotFound));
            }

            //Get user's team object
            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);

            //Something went wrong fetching the data, return 404
            if (team == null)
            {
                return RedirectToAction(nameof(TeamNotFound));
            }
            //Return 401 if the id in URL does not match the team ID of the logged in user
            if (team.Id != loggedInUser.TeamId)
            {
                return StatusCode(401);
            }
            //Get the user's team's standing
            Standing userStanding = _context.Standings.FirstOrDefault(m => m.id == team.Id);
            List<PlayInGame> userGames = _context.PlayInGames.Where(m => m.Team1Id == id || m.Team2Id == id).ToList();
            List<_3WayGame> user3wayGames = _context.PlayInGames.OfType<_3WayGame>().Where(m => m.Team1Id == id || m.Team2Id == id).ToList();
            //Instantiaste UserDataView object to pass to the view
            UserDataView data = new UserDataView(_context, team, userStanding, userGames, user3wayGames);
            //No errors, return View with team obj
            return View(data);
        }

        public IActionResult TeamNotFound()
        {
            return View();
        }
        /*
         * ==============================
         * REGISTER TEAM
         * ==============================
         */

        // GET: Teams/Register
        public IActionResult Register()
        {
            //Instantiate model class
            Team team = new Team();
            TeamRegView model = new TeamRegView(_context, team);

            return View(model);
        }

        // POST: Teams/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(TeamRegView team)
        {
            //Get the ApplicationUser class for the players on the newly registered team
            var newPlayers = await _context.ApplicationUsers
                .Select(n => n)
                .Where(n => (n.FirstName + " " + n.LastName) == team._team.PlayerOne || 
                            (n.FirstName + " " + n.LastName) == team._team.PlayerTwo)
                .ToListAsync();

            //Seed random 6 digit numbers for Team Id
            Random rnd = new Random();
            int id = rnd.Next(100000, 999999);

            //Data validated, add to DB
            if (ModelState.IsValid)
            {
                //Set Team Id
                team._team.Id = id;
                //Add team to DB
                _context.Add(team._team);

  
                //For each new player, change HasTeam property to true and add TeamID
                foreach (var player in newPlayers)
                {
                    player.HasTeam = true;
                    player.TeamId = id;
                }
                //Save DB changes
                await _context.SaveChangesAsync();
                //Return confirmation page
                return RedirectToAction(nameof(RegisterConfirm));
            }
            //Data is not valid, return to previous page
            TeamRegView model = new TeamRegView(_context, team._team);
            return View(model);
        }

        // GET: Teams/RegisterConfirm
        public IActionResult RegisterConfirm()
        {
            return View();
        }

        /*
        * ==============================
        * EDIT YOUR TEAM
        * ==============================
        */

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Invalid route. Team ID must be specified
            if (id == null)
            {
                return NotFound();
            }

            var loggedInUser = _context.ApplicationUsers.FirstOrDefault(m => m.Email == _user.GetUserName(User));
            //Return 401 if the id in URL does not match the team ID of the logged in user
            if (id != loggedInUser.TeamId)
            {
                return StatusCode(401);
            }

            //Find team
            var team = await _context.Teams.FindAsync(id);

            //Something went wrong fetching the data, return 404
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PlayerOne,PlayerTwo")] Team team)
        {
            //The wrong team is attempting to be edited, return error
            if (id != team.Id)
            {
                return NotFound();
            }

            //Data validated, update DB with changes
            if (ModelState.IsValid)
            {
                _context.Update(team);

                try
                {
                    //Get the team's Standing 
                    Standing standingName = _context.Standings.Find(team.Id);
                    //Update DB
                    standingName.TeamName = team.Name;
                }
                catch (NullReferenceException)
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyTeam), new { id = team.Id });
                }
                catch (DbUpdateConcurrencyException)
                {
                    //There was a problem posting the data, return error
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                await _context.SaveChangesAsync();
                //If user is an admin, go back to the AdminView route
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(AllTeams));
                }
                //If they are a regular user, redirect to the MyTeam page
                else
                {
                    return RedirectToAction(nameof(MyTeam), new { id = team.Id });
                }
                
            }
            //Data edited, return View
            return View(team);
        }

        /*
        * ==============================
        * DELETE YOUR TEAM
        * ==============================
        */
        [Authorize(Roles = "Admin")]
        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);

            //Get the ApplicationUser class for the players on the deleted team
            var newPlayers = await _context.ApplicationUsers
                .Select(n => n)
                .Where(n => (n.FirstName + " " + n.LastName) == team.PlayerOne ||
                            (n.FirstName + " " + n.LastName) == team.PlayerTwo)
                .ToListAsync();

            _context.Teams.Remove(team);

            //Reset user team attributes to not being on a team
            foreach (var player in newPlayers)
            {
                player.HasTeam = true;
                player.TeamId = 0;
            }

            await _context.SaveChangesAsync();
            return RedirectToRoute("Home/Index");
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}

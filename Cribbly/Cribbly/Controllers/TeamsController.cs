using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cribbly.Data;
using Cribbly.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cribbly.Controllers
{
    //Require user to be logged in to access any endpoint below
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*
         * ==============================
         * VIEW ALL TEAMS (Admin)
         * ==============================
         */

        // GET: Teams
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teams.ToListAsync());
        }

        /*
         * ==============================
         * VIEW YOUR TEAM (User)
         * ==============================
         */

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
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

        /*
         * ==============================
         * REGISTER TEAM
         * ==============================
         */

        // GET: Teams/Register
        public IActionResult Register()
        {
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

            //Data validated, add to DB
            if (ModelState.IsValid)
            {
                _context.Add(team._team);

                //For each new player, change HasTeam property to true
                foreach (var player in newPlayers)
                {
                    player.HasTeam = true;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RegisterConfirm));
            }
            //Data is not valid, return to previous page
            return View(team);
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
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PlayerOne,PlayerTwo,Division")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            //Data validated, update DB with changes
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        /*
        * ==============================
        * DELETE YOUR TEAM
        * ==============================
        */

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
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }
    }
}

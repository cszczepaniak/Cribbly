﻿using System;
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
        public async Task<IActionResult> AdminView()
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
            if (id == 0)
            {
                return RedirectToAction(nameof(TeamNotFound));
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);

            if (team == null)
            {
                return RedirectToAction(nameof(TeamNotFound));
            }

            return View(team);
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

  
                //For each new player, change HasTeam property to true
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

        // POST: Teams/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,PlayerOne,PlayerTwo")] Team team)
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
                return RedirectToAction(nameof(AdminView));
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

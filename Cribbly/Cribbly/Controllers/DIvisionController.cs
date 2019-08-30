using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cribbly.Data;
using Microsoft.AspNetCore.Authorization;

namespace Cribbly.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DivisionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DivisionController(ApplicationDbContext context)
        {
            _context = context;
        }

        //View all divisions
        public IActionResult Index()
        {
            var divisions = _context.Divisions.ToList();

            //Divisions are not yet set up. Redirect to the Setup route
            if (divisions == null)
            {
                return RedirectToAction(nameof(Setup));
            }

            return View(divisions);
        }

        //Takes user to confirmation page listing the amount of players and divisions
        public IActionResult Setup()
        {
            var standings = _context.Standings.ToList();
            return View(standings);
        }

        //Creates the standings and commits them to the DB
        public IActionResult Create()
        {
            return RedirectToAction(nameof(Index));
        }

        //Edit the name or members of a Division
        public IActionResult Edit()
        {
            return View();
        }

        //Deletes a division
        public IActionResult Delete()
        {
            return View();
        }

        /*If a team gets deleted, and the 'redistribute' box is checked,
        this method reorganizes teams to fit the amount of divisions*/
        public IActionResult TeamRedistribution()
        {
            return View();
        }
    }
}
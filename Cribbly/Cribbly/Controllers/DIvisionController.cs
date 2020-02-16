using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cribbly.Data;
using Cribbly.Models;
using Microsoft.AspNetCore.Authorization;
using Cribbly.Models.ViewModels;

namespace Cribbly.Controllers
{
    //Require user to be logged in to access any endpoint below
    [Authorize]
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
            DivisionView model = new DivisionView(_context);

            //Divisions are not yet set up. Redirect to the Setup route
            if (model._divisions.Count == 0)
            {
                return RedirectToAction(nameof(Setup));
            }

            return View(model);
        }

        //Takes user to confirmation page listing the amount of players and divisions
        [Authorize(Roles = "Admin")]
        public IActionResult Setup()
        {
            var standings = _context.Standings.ToList();
            return View(standings);
        }

        //Creates the standings and commits them to the DB
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //Get number of divisions required for each one to have 4 teams
            int numDivs = _context.Standings.Count() / 4;

            //Create divisions with temporary numbered names
            for (int i = 1; i <= numDivs; i++)
            {
                Division div = new Division()
                {
                    DivName = i.ToString()
                };
                //Add records to DB
                _context.Divisions.Add(div);
            }
            //Update DB
            _context.SaveChanges();

            List<Standing> allTeams = _context.Standings.ToList();
            List<Division> allDivs = _context.Divisions.ToList();
            int tracker = 1;

            for (int i = 0; i < 2; i++)
            {
                try
                {
                    allTeams[0].Division = tracker.ToString();
                    allTeams[1].Division = tracker.ToString();
                    allTeams[2].Division = tracker.ToString();
                    allTeams[3].Division = tracker.ToString();
                    //4 players have been assigned a division. Remove them from the List
                    allTeams.RemoveRange(0, 4);
                    tracker++;
                }
                catch
                {
                    //Assign an extra team to division 1 and/or 2 if one or two teams are left
                    if (allTeams.Count == 2)
                    {
                        allTeams[0].Division = 1.ToString();
                        allTeams[1].Division = 2.ToString();
                    }
                    else if (allTeams.Count == 1)
                    {
                        allTeams[0].Division = 1.ToString();
                    }
                    else
                    {
                        //It is not coming out evenly, enter loop to make odd numbered division
                        foreach (var team in allTeams)
                        {
                            team.Division = tracker.ToString();
                        }

                        Division div = new Division()
                        {
                            DivName = tracker.ToString()
                        };
                        _context.Divisions.Add(div);
                    }
                    break;
                }
                //Reset enumerator to 0. Next 4 teams will then be assigned 
                i = 0;
            }

            _context.SaveChanges();

            //Redirect to list of Divisions
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        //Edit the name or members of a Division
        [Authorize(Roles = "Admin")]
        public IActionResult EditDivision(int id)
        {
            var model = _context.Divisions.FirstOrDefault(m => m.Id == id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult EditDivision(Division model)
        {
            var divName = _context.Divisions.FirstOrDefault(m => m.Id == model.Id).DivName;
            var divGames = _context.PlayInGames.Where(m => m.Division == divName);
            var members = _context.Standings.Where(m => m.Division == divName);

            foreach (var member in members)
            {
                member.Division = model.DivName;
            }

            foreach (var divGame in divGames)
            {
                divGame.Division = model.DivName;
            }

            _context.Divisions.FirstOrDefault(m => m.Id == model.Id).DivName = model.DivName;
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
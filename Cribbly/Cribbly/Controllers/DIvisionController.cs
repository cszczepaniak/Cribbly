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
            DivisionView model = new DivisionView(_context);

            //Divisions are not yet set up. Redirect to the Setup route
            if (model._divisions.Count == 0)
            {
                return RedirectToAction(nameof(Setup));
            }

            return View(model);
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
                    /*
                    if (tracker > numDivs)
                    {
                        i = 2;
                        break;
                    };*/

                    allTeams[0].Division = tracker.ToString();
                    allTeams[1].Division = tracker.ToString();
                    allTeams[2].Division = tracker.ToString();
                    allTeams[3].Division = tracker.ToString();

                    allTeams.RemoveRange(0, 4);
                    tracker++;
                }
                catch
                {
                    if (allTeams.Count == 2)
                    {
                        allTeams[0].Division = 1.ToString();
                        allTeams[1].Division = 2.ToString();
                    }
                    else if (allTeams.Count == 2)
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

                i = 0;
            }

            _context.SaveChanges();

            //Redirect to list of Divisions
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Cribbly.Controllers
{
    public class TournamentSetupController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cribbly.Data;


namespace Cribbly.Models
{
    public class TeamRegView
    {
        ApplicationDbContext _context { get; set; }
        public Team _team { get; set; }
        public List<string> _players { get; set; }
        public List<string> _emails { get; set; }

        public TeamRegView(ApplicationDbContext context, Team team)
        {
            _context = context;
            _team = team;
            _players = new List<string>();
            _emails = new List<string>();

            var players =  _context.ApplicationUsers
                .Select(n => new { n.FirstName, n.LastName, n.HasTeam, n.Email })
                .Where(n => n.HasTeam == false && n.LastName != "_admin");

            foreach (var player in players)
            {
                _players.Add(player.FirstName + " " + player.LastName);
                _emails.Add(player.Email);
            }
        }

        public TeamRegView()
        {

        }
    }
}
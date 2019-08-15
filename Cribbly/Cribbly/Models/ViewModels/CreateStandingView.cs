using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class CreateStandingView
    {
        public List<Team> _newTeams;
        public List<ApplicationUser> _teamlessUsers;
        public bool _playerLeftOver;

        public CreateStandingView(List<Team> newTeams, List<ApplicationUser> teamlessUsers, bool playerLeftOver)
        {
            _newTeams = newTeams;
            _playerLeftOver = playerLeftOver;
            _teamlessUsers = teamlessUsers;
        }
        public CreateStandingView()
        {

        }
    }
}

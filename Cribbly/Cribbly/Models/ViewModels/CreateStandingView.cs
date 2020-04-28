using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cribbly.Models
{
    public class CreateStandingView
    {
        public List<Team> _newTeams;
        public bool _playerLeftOver;

        public CreateStandingView(List<Team> newTeams, bool playerLeftOver)
        {
            _newTeams = newTeams;
            _playerLeftOver = playerLeftOver;
        }
        public CreateStandingView()
        {

        }
    }
}

class Dashboard extends React.Component {
    state = {  }
    render() { 
        return ( 
            <div>
                <p>Dashboard</p>
                <div class="row">
                    <TeamModule/>
                </div>
                <div class="row">
                    <StandingModule/>
                </div>
                <div class="row">
                    <GamesModule/>
                </div>

            </div>
        );
    }
}
 
class TeamModule extends React.Component {
    state = {  }
    render() { 
        return ( 
            <div class="col-md-12">
                <h3 id="teamName">Team Name</h3>
                <ul class="list-group-item">
                    <li>Team member 1</li>
                    <li>Team member 2</li>
                </ul>
            </div>
        );
    }
}

class StandingModule extends React.Component {
    state = {  }
    render() { 
        return ( 
            <div class="col-md-12">
                <h3>In tournament/Not in tournament</h3>
                <dl class="row">
                    <dt>Division</dt>
                    <dd>1</dd>
                    <dt>Standing in Division</dt>
                    <dd>2</dd>
                    <dt>Standing Overall</dt>
                    <dd>10</dd>
                </dl>
            </div>
        );
    }
}

class GamesModule extends React.Component {
    state = {  }
    render() { 
        return ( 
            <div class="col-md-12">
                <div class="row">
                    <dl class="col-md-6">
                        <dt>Game 1</dt>
                        <dd>vs. Team 65</dd>
                        <dt>Game 2</dt>
                        <dd>vs. good guys</dd>
                        <dt>Game 3</dt>
                        <dd>vs. another team</dd>
                    </dl>
                    <GameResults/>
                </div>

            </div>
        );
    }
}
class GameResults extends React.Component {
    state = {  }
    render() { 
        return ( 
            <div class="col-md-6">
                <p>Results of highlighted game</p>
                <table class="table">
                    <thead>
                        <th>Win/Loss</th>
                        <th>Your Score</th>
                        <th>Opp Score</th>
                        <th>Score Difference</th>
                    </thead>
                    <tbody>
                        <tr>
                            <td>W</td>
                            <td>121</td>
                            <td>55</td>
                            <td>66</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        );
    }
}
 
 
 
 

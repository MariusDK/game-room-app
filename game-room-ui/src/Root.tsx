import { Switch, Route, BrowserRouter } from 'react-router-dom';
import * as React from 'react';
import Home from './components/Home/Home';
import SoloGamePage from './components/SoloGamePage/SoloGamePage';
import CreateTeam from './components/CreateTeam/CreateTeam';
import TeamGamePage from './components/TeamGamePage/TeamGamePage';
import RegisterPlayer from './components/PlayerComponents/RegisterPlayer/RegisterPlayer';
import LoginPlayer from './components/PlayerComponents/LoginPlayer/LoginPlayer';
import CreateGameSolo from './components/GameComponents/CreateGameSolo/CreateGameSolo';
import CreateTeamGame from './components/GameComponents/CreateTeamGame/CreateTeamGame';
import ImageComponent from './components/ImageAIComponent/ImageComponent';
import FinishGames from './components/GameComponents/GameList/FinishGames/FinishGames';
import UnfinishGames from './components/GameComponents/GameList/UnfinishGames/UnfinishGames';
import About from './components/AboutComponent/About';
import Terms from './components/TermsComponent/Terms';
import TeamList from './components/TeamComponents/TeamList';

const Root = () => (
    <BrowserRouter>
        <Switch>
          <Route exact={true} path="/" component={Home} />        
          <Route path="/register" component={RegisterPlayer}/>
          <Route path="/login" component={LoginPlayer}/>
          <Route path="/createGameSolo" component={CreateGameSolo}/>
          <Route path="/gameSoloPage/:gameName" component={SoloGamePage}/>
          <Route path="/createTeam" component={CreateTeam}/>
          <Route path="/createTeamGame" component={CreateTeamGame}/>
          <Route path="/gameTeamPage/:gameName" component={TeamGamePage}/>
          <Route path="/imageAnalyzer" component={ImageComponent}/>
          <Route path="/finishGames" component={FinishGames}/>
          <Route path="/unfinishGames" component={UnfinishGames}/>
          <Route path="/about" component={About}/>
          <Route path="/terms" component={Terms}/>
          <Route path="/teams" component={TeamList}/>
        </Switch>
    </BrowserRouter>

)
export default Root;
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
import GameList from './components/GameComponents/GameList/GameList';
import ImageComponent from './components/ImageAIComponent/ImageComponent';


const Root = () => (
    <BrowserRouter>
        <Switch>
          <Route exact={true} path="/" component={Home} />        
          <Route path="/register" component={RegisterPlayer}/>
          <Route path="/login" component={LoginPlayer}/>
          <Route path="/createGameSolo" component={CreateGameSolo}/>
          <Route path="/gameSoloPage" component={SoloGamePage}/>
          <Route path="/createTeam" component={CreateTeam}/>
          <Route path="/createTeamGame" component={CreateTeamGame}/>
          <Route path="/gameTeamPage" component={TeamGamePage}/>
          <Route path="/games" component={GameList}/>
          <Route path="/imageAnalyzer" component={ImageComponent}/>
        </Switch>
    </BrowserRouter>
)
export default Root;
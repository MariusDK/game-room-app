import { Switch, Route, BrowserRouter } from 'react-router-dom';
import * as React from 'react';
import RegisterPlayer from './components/RegisterPlayer/RegisterPlayer';
import LoginPlayer from './components/LoginPlayer/LoginPlayer';
import Home from './components/Home/Home';


const Root = () => (
    <BrowserRouter>
        <Switch>
          <Route exact={true} path="/" component={Home} />        
          <Route path="/register" component={RegisterPlayer}/>
          <Route path="/login" component={LoginPlayer}/>
        </Switch>
    </BrowserRouter>
)
export default Root;
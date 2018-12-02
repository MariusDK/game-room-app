import * as React from 'react';
import { Link }  from "react-router-dom";
import  './StartPage.css'

export default function StartPage(){
    return (
      <div className="App">
        <h1>Wellcome!</h1>
        <Link to="/login" className="Links">Login</Link>
        <Link to="/register" className="Links">Register</Link>
      </div>
    );
  }
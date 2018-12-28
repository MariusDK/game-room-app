import * as React from 'react';
import { Link }  from "react-router-dom";
import  './StartPage.css'
import Header from './Header/Header';
import Footer from '../Footer/Footer';


export default function StartPage(){
    return (
      
      <div>
        <Header/>
      <div className="SPage">
      <div>
        <h2><p>"Computer programming is an art, because it applies accumulated
          knowledge to the world, because it requires skill and ingenuity, and
          especially because it produces objects of beauty.</p><p>
            A programmer who subconsciously views himself as an artist will enjoy
          what he does and will do it better." - Donald Knuth</p>
        </h2>
      </div>
      <div className="registerButton">
        <Link to="/register" className="Links">Register</Link>
      </div>
      </div>
        <Footer/>
      </div>
    );
  }
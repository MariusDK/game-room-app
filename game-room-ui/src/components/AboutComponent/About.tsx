import * as React from 'react';
import "./About.css";
import Header from '../StartPage/Header/Header';
import Footer from '../Footer/Footer';


export default function About() {
    return (
  
      <div>
        <Header />
        <div className="About">
          <div>
            <h2><p>“Teamwork is the ability to work together toward a common vision. 
                The ability to direct individual accomplishments toward organizational objectives.
                It is the fuel that allows common people to attain uncommon results.” – Andrew Carnegie</p>
                <p></p>
            </h2>
          </div>
        </div>
        <Footer />
      </div>
    );
  }
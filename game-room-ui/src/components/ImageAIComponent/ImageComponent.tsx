import * as React from 'react';
import Footer from '../Footer/Footer';
import Header from '../StartPage/Header/Header';
import 'src/components/ImageAIComponent/ImageComponent.css';


export default class AnalyzerImage extends React.Component<any,any>{
    constructor(props:any)
    {
      super(props);
      this.state = {
      }
    }
    render()
    {
      {var imgAddress = localStorage.getItem('clickedImg')}
    return (
      <div>
        <Header/>
        <div className="imageAnalyzer">
          <div>
            
            {imgAddress!=null}{
            <img src={imgAddress} />
            }
          </div>
          <div>
          <button >Click to find what game you are playing!</button>
          </div>
        </div>
        <Footer/>
      </div>
    );
          }
  }
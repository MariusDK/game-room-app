import * as React from 'react';
import Footer from '../Footer/Footer';
import Header from '../StartPage/Header/Header';
import 'src/components/ImageAIComponent/ImageComponent.css';
import GameService from 'src/services/GameService';
import { ClipLoader } from 'react-spinners';

export interface IAnalyzerImageState {
  loading: boolean;
  response: string;
}

export default class AnalyzerImage extends React.Component<any,IAnalyzerImageState>{
    constructor(props:any)
    {
      super(props);
      this.state = {
         loading:false,
         response:''
      }
    }
    analyzeImg=(imgAddress:string)=>
    {
      this.setState({loading:true});
      var infoAboutImg = localStorage.getItem('ImgPositionAndListType');
      GameService.getPredictionGame(infoAboutImg).then((response:string)=>{
        var completeResponse = "You are playing "+response;
        this.setState({loading:false,response:completeResponse});
        console.log(response);
      });
    }
    render()
    {
      {var imgAddress = localStorage.getItem('clickedImg');
    console.log(imgAddress);}
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
          <button onClick={()=>this.analyzeImg(imgAddress)} className="probabiltyBtn">Click to find what game you are playing!</button>
          </div>
            <div className="spinner">
            <ClipLoader
                color={'#B4498D'}
                loading={this.state.loading}
                />
            </div>
            <span className="resultIA">{this.state.response}</span><br/>
        </div>
        <Footer/>
      </div>
    );
    }
  }
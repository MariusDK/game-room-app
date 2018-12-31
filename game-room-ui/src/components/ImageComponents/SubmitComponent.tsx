import * as React from 'react';
import GameService from 'src/services/GameService';
import { IGame } from 'src/models/IGame';
export interface ISubmitComponentProps {
    gameName: string;
    onChange(gameName:string):void;
}
export default class SubmitComponent extends React.Component<ISubmitComponentProps,any>
{
    constructor(props:any)
    {
        super(props);
        this.state = {
            image: ''
        }
    }
    fileSelectedHandler=(e:any)=>
    {
        let files = e.target.files;
        console.log(files[0]);
        this.setState({image:files[0]});
        
    }
    fileUploadVictoryHandler=(e:any)=>
    {
        let reader = new FileReader();
        reader.readAsDataURL(this.state.image);
        
        reader.onload=(e:any)=>
        {
            GameService.getGameByName(this.props.gameName).then((result: IGame) => 
            {
            if (e.target!=null)
            {
                const formData = {file:e.target.result}
                console.log(result.victoryMoments);
                var file = formData.file;
                var fileList = file.split(",");
                if (result.victoryMoments!=undefined){
                    console.log(result.victoryMoments);
                    const victoryMomentsList:string[]=result.victoryMoments;
                    victoryMomentsList.push(fileList[1]);
                    result.victoryMoments = victoryMomentsList;
                    console.log(result.victoryMoments);

                }
                else{
                    console.log(result.victoryMoments);
                    const victoryMomentsList:string[]=[];
                    victoryMomentsList.push(fileList[1]);
                    result.victoryMoments = victoryMomentsList;
                    console.log(result.victoryMoments);
                    
                }
                GameService.updateGame(this.props.gameName,result);
            }
        })
        this.onChange();
    }
    }
    fileUploadEmbarrassingHandler=(e:any)=>
    {
        let reader = new FileReader();
        reader.readAsDataURL(this.state.image);
        
        reader.onload=(e:any)=>
        {
            GameService.getGameByName(this.props.gameName).then((result: IGame) => 
            {
            if (e.target!=null)
            {
                const formData = {file:e.target.result}
                var file = formData.file;
                var fileList = file.split(",");
                if (result.embarrassingMoments!=undefined){

                    const embarrassingMomentsList:string[]=result.embarrassingMoments;
                    embarrassingMomentsList.push(fileList[1]);
                    result.embarrassingMoments = embarrassingMomentsList;
                    console.log(result.victoryMoments);
                }
                else{
                    const embarrassingMomentsList:string[]=[];
                    embarrassingMomentsList.push(fileList[1]);
                    result.embarrassingMoments = embarrassingMomentsList;
                    console.log(result.victoryMoments);
                }
                GameService.updateGame(this.props.gameName,result);
            }
        })
        this.onChange();
     }}
    onChange=()=>
    {
        let gameName = this.props.gameName;
        this.props.onChange(gameName);
        console.log("aici");
    }
    render()
    {
        return(
            <div>
                <h1>Image Upload</h1>
                <input type="file" name="file" onChange={this.fileSelectedHandler}/>
                <button onClick={this.fileUploadVictoryHandler}>Add Victory Moments</button>
                <button onClick={this.fileUploadEmbarrassingHandler}>Add Embarrassing Moments</button>
            </div>
        )
    }
}
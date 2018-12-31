import * as React from 'react';
import "./GameForm.css";

export interface IGameFormProps{
    handleChange(e: any): void;
    name: string;
    type:string;
    nameError:string;
}
const GameForm = (props: IGameFormProps) => {
    return (
        <div className="gameForm">
            <h1>Create Game</h1>
            <input type="text" name="name" onChange={props.handleChange} placeholder="Name" value={props.name}/><br/>
            <span style={{color: "red"}}>{props.nameError}</span><br/>

            <input type="text" name="type" onChange={props.handleChange} placeholder="Type (Ex:Darts/X01,Darts/Cricket)" value={props.type}/><br/>

        </div>
    );
}
export default GameForm;
  
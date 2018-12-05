import * as React from 'react';

export interface IGameFormProps{
    handleChange(e: any): void;
    name: string;
    type:string;
    nameError:string;
}
const GameForm = (props: IGameFormProps) => {
    return (
        <div>
            <h1>Create Game</h1>
            <label htmlFor="name">Name</label>
            <input type="text" name="name" onChange={props.handleChange} value={props.name}/><br/>
            <span style={{color: "red"}}>{props.nameError}</span><br/>

            <label htmlFor="type">Type</label>
            <input type="text" name="type" onChange={props.handleChange} value={props.type}/><br/>

        </div>
    );
}
export default GameForm;
  
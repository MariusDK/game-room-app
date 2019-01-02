import * as React from 'react';
import './TeamForm.css';

export interface ITeamFormProps {
    handleChange(e:any):void;
    name:string;
    nameError:string;
}

const TeamForm = (props: ITeamFormProps) =>{
    return (
        <div className="teamForm">
            <h1>Create Team</h1>
            <input className="teamNameInput" type="text" name="name" placeholder ="Team Name" onChange={props.handleChange} value={props.name}/><br/>
            <span style={{color: "red"}}>{props.nameError}</span><br/>
        </div>
    );
}
export default TeamForm;
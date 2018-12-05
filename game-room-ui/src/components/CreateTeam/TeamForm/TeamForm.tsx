import * as React from 'react';

export interface ITeamFormProps {
    handleChange(e:any):void;
    name:string;
    nameError:string;
}

const TeamForm = (props: ITeamFormProps) =>{
    return (
        <div>
            <h1>Create Team</h1>
            <input type="text" name="name" onChange={props.handleChange} value={props.name}/><br/>
            <span style={{color: "red"}}>{props.nameError}</span><br/>
        </div>
    );
}
export default TeamForm;
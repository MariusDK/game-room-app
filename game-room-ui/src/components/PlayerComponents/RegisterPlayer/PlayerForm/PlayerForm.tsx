import * as React from 'react';
import '../RegisterPlayer.css';

export interface IPlayerFormProps {
    handleChange(e: any): void;
    name: string;
    username: string;
    password: string;
    age: number;
    nameError: string;
    usernameError: string;
    passwordError: string;
    ageError: string;
    title: string;
}

const PlayerForm = (props: IPlayerFormProps) => {
    return (
        <div>
            <h1>{props.title}</h1>
            <label htmlFor="name">Name</label>
            <input type="text" name="name" onChange={props.handleChange} value={props.name}/><br/>
            <span style={{color: "red"}}>{props.nameError}</span><br/>

            <label htmlFor="username">Username</label>
            <input type="text" name="username" onChange={props.handleChange} value={props.username}/><br/>
            <span style={{color: "red"}}>{props.usernameError}</span><br/>

            <label htmlFor="password">Password</label>
            <input type="password" name="password" onChange={props.handleChange} value={props.password}/><br/>
            <span style={{color: "red"}}>{props.passwordError}</span><br/>

            <label htmlFor="age">Age</label>
            <input type="number" name="age" onChange={props.handleChange} value={props.age}/><br/>
            <span style={{color: "red"}}>{props.ageError}</span><br/>
        </div>
    );
}
export default PlayerForm;

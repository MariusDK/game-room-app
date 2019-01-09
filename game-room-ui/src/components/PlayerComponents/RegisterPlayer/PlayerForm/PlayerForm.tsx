import * as React from 'react';
import './PlayerForm.css';

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
        <div className="playerForm">
            <h1>{props.title}</h1>
            <input type="text" name="name" placeholder="Name" required pattern="^[A-Za-zÀ-ÿ ,.'-]+$" onChange={props.handleChange} value={props.name} /><br />
            <span style={{ color: "red" }}>{props.nameError}</span><br />

            <input type="text" name="username" placeholder="Username" onChange={props.handleChange} value={props.username} /><br />
            <span style={{ color: "red" }}>{props.usernameError}</span><br />

            <input type="password" name="password" placeholder="Password" onChange={props.handleChange} value={props.password} /><br />
            <span style={{ color: "red" }}>{props.passwordError}</span><br />

            <input type="number" name="age" placeholder="Age" onChange={props.handleChange} value={props.age} /><br />
            <span style={{ color: "red" }}>{props.ageError}</span><br />
        </div>
    );
}
export default PlayerForm;

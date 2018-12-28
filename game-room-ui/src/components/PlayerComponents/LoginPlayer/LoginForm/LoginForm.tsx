import * as React from 'react';
import "./LoginForm.css";

export interface ILoginFormProps {
    handleChange(e: any): void;
    username: string;
    password: string;
    usernameError: string;
    passwordError: string;
}

const LoginForm = (props: ILoginFormProps) => {
    return (
        <div className="loginForm">
            <h1> Login </h1>
            
            <input type="text" name="username" placeholder="Username" onChange={props.handleChange} value={props.username}/><br/>
            <span style={{color: "red"}}>{props.usernameError}</span><br/>

            <input type="password" name="password" placeholder="Password" onChange={props.handleChange} value={props.password}/><br/>
            <span style={{color: "red"}}>{props.passwordError}</span><br/>
        </div>
    );
}
export default LoginForm;
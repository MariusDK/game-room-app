import * as React from 'react';

export interface ILoginFormProps {
    handleChange(e: any): void;
    username: string;
    password: string;
    usernameError: string;
    passwordError: string;
}

const LoginForm = (props: ILoginFormProps) => {
    return (
        <div>
            <h1> Login </h1>
            <label htmlFor="username">Username</label>
            <input type="text" name="username" onChange={props.handleChange} value={props.username}/><br/>
            <span style={{color: "red"}}>{props.usernameError}</span><br/>

            <label htmlFor="password">Password</label>
            <input type="text" name="password" onChange={props.handleChange} value={props.password}/><br/>
            <span style={{color: "red"}}>{props.passwordError}</span><br/>
        </div>
    );
}
export default LoginForm;
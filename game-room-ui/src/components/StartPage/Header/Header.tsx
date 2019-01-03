import * as React from 'react';
import { Link, Redirect } from 'react-router-dom';
import "./Header.css";
export default class Header extends React.Component<any, any>
{
    constructor(props: any) {
        super(props);
        this.state = {
        }
    }

    public render() {

        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to='/' />
        }
        return (
            <header>
                <div className="homeButton">
                    <Link to="/">Home</Link>
                </div>
                <div className="m-logo">
                    <img src={require('src/Resurces/logo.png')} />
                </div>
                <div className="logInButton">
                    <Link to="/login">Log in</Link>
                </div>
            </header>
        )
    }
}
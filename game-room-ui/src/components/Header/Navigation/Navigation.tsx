import * as React from 'react';
import { Link, Redirect } from 'react-router-dom';
import "./Navigation.css";

export default class Navigation extends React.Component<any, any>
{
    constructor(props: any)
    {
        super(props);
        this.state = {
            redirect: false
        }
    }
    logout = () =>
    {
        localStorage.removeItem("currentUser");
        this.setState({redirect:true});
    }
    public render()
    {
        const {redirect} = this.state;
        if (redirect){
            return <Redirect to='/'/>
        }
        return (
            <div>
                <nav>
                    <ul>
                        <li>
                            <Link to=''>Home</Link><br />
                        </li>
                        <li>
                            <Link to=''>Games</Link><br />
                        </li>
                        <li className="logoutButton">
                            <button onClick={this.logout}>Logout</button>
                        </li>                        
                    </ul>
                </nav>
            </div>
        )
    }
}
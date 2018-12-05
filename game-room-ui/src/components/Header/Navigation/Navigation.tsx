import * as React from 'react';
import { Link, Redirect } from 'react-router-dom';
import "./Navigation.css";
import DropdownMenu from '../DropdownMenu/DropdownMenu';


export default class Navigation extends React.Component<any, any>
{
    constructor(props: any)
    {
        super(props);
        this.state = {
            redirect: false,
            displayMenu: false
        }
    }
    showDropdown=(e:any)=> {
        e.preventDefault();
        this.setState({displayMenu:true});
      }
    cancelDropdown=(e:any)=> {
        this.setState({displayMenu:false});
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
                        <li onFocus={this.showDropdown} onBlur={this.cancelDropdown} >
                            <DropdownMenu
                                
                                />
                        </li>
                        <li>
                            <Link to='/createTeam'>Create Team</Link><br />
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
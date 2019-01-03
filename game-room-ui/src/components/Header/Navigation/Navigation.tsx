import * as React from 'react';
import { Link, Redirect } from 'react-router-dom';
import "./Navigation.css";
import DropdownMenu from '../DropdownMenu/DropdownMenu';



export default class Navigation extends React.Component<any, any>
{
    constructor(props: any) {
        super(props);
        this.state = {
            redirect: false,
            displayMenu: false
        }
    }
    showDropdown = (e: any) => {
        e.preventDefault();
        if (this.state.displayMenu) {
            this.setState({ displayMenu: false });
        }
        else {
            this.setState({ displayMenu: true }, () => {
                document.addEventListener('click', this.cancelDropdown);
            });
        }
    }

    cancelDropdown = (e: any) => {
        this.setState({ displayMenu: false }, () => {
            document.removeEventListener('click', this.cancelDropdown);
        });
    }
    logout = () => {
        localStorage.removeItem("currentUser");
        this.setState({ redirect: true });
    }

    public render() {

        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to='/' />
        }

        return (
            <div className="menuNav">
                <nav>
                    <ul>
                        <li>
                            <Link to=''>Home</Link><br />
                        </li>
                        <li>
                            <Link to='/createTeam'>Create Team</Link><br />
                        </li>
                    </ul>
                </nav>
                <div className="dropdown">
                    <button onClick={this.showDropdown} className="dropdownBtn">Games</button>
                    <div className="myDropdown" id="idDropdown">
                        <DropdownMenu
                            displayMenu={this.state.displayMenu}
                        />
                    </div>
                </div>
                <div className="m-logo">
                    <img src={require('src/Resurces/logo.png')} />
                </div>
                <div className="logoutButton">
                    <button onClick={this.logout}>Logout</button>
                </div>
            </div>
        )
    }
}
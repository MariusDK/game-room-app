import * as React from 'react';
import { Link, Redirect } from 'react-router-dom';
import "./Navigation.css";
import DropdownMenu from '../DropdownMenu/DropdownMenu';

export interface INavigationProps 
{ 
    onAddBlur():any;
    onRemoveBlur():any;
}

export default class Navigation extends React.Component<INavigationProps, any>
{
    constructor(props: any) {
        super(props);
        this.state = {
            redirect: false,
            displayMenu: false,
        }
    }
    showDropdown = (e: any) => {
        e.preventDefault();
        if (this.state.displayMenu) {
            this.props.onRemoveBlur();
            this.setState({ displayMenu: false });
        }
        else {
            this.props.onAddBlur();
            this.setState({ displayMenu: true }, () => {
                document.addEventListener('click', this.cancelDropdown);
            });
        }
        
    }

    cancelDropdown = (e: any) => {
        this.props.onRemoveBlur();
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
                    <button onClick={this.logout}>Log out</button>
                </div>
            </div>
        )
    }
}
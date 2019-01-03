import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import LoginForm from './LoginForm/LoginForm'
import PlayerService from 'src/services/PlayerService';
import { IPlayer } from 'src/models/IPlayer';
import { SpinnerComponent } from 'src/components/Spinner/Spinner';
import "../LoginPlayer/LoginPlayer.css";
import Header from 'src/components/StartPage/Header/Header';
import Footer from 'src/components/Footer/Footer';



export interface ILoginPlayerState {
    username: string;
    password: string;
    usernameError: string;
    passwordError: string;
    loading: boolean;
    redirect: boolean;
    errorMessage: string;
}

export interface ILoginPlayerProps extends RouteComponentProps<any> { }

export default class LoginPlayer extends React.Component<ILoginPlayerProps, ILoginPlayerState>
{
    constructor(props: ILoginPlayerProps) {
        super(props);
        this.state = {
            username: '',
            password: '',
            usernameError: '',
            passwordError: '',
            loading: false,
            redirect: false,
            errorMessage: ''
        }
    }
    handleValidation() {
        let username = this.state.username;
        let password = this.state.password;
        let usernameError = '';
        let passwordError = '';
        let formIsValid = true;
        if (!username) {
            formIsValid = false;
            usernameError = "Username cannot be empty!";
        }
        else if (username.length < 6) {
            formIsValid = false;
            usernameError = "Username must hava more than 5 characters!";
        }
        else if (!password) {
            formIsValid = false;
            usernameError = "Password cannot be empty";
        }
        else if (password.length < 6) {
            formIsValid = false;
            usernameError = "Password must hava more than 5 characters!";
        }
        this.setState({ usernameError: usernameError });
        this.setState({ passwordError: passwordError });
        return formIsValid;

    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: ILoginPlayerState) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }

    login = () => {
        this.setState({ loading: true });
        if (this.handleValidation()) {
            var username = this.state.username;
            var password = this.state.password;
            var response = PlayerService.login(username, password);
            response.then((player: IPlayer) => {

                if (!player) {
                    this.setState({ errorMessage: "Player Not Found!" });
                }
                else {
                    player.password = password;
                    localStorage.setItem('currentUser', JSON.stringify(player));
                    this.setState({ redirect: true });
                }

            });

            //this.setState({redirect:true});
        } else {
            console.log("Error");
            this.setState({ errorMessage: "Error" });
        }
        this.setState({ loading: false });

    }
    logout = () => {
        localStorage.removeItem('currentUser')
    }

    public render() {
        const { redirect } = this.state;
        if (redirect) {
            return <Redirect to='/' />
        }
        return (
            <div>
                <Header />
                <div className="Login">
                    <LoginForm
                        username={this.state.username}
                        password={this.state.password}
                        handleChange={this.handleChange}
                        usernameError={this.state.usernameError}
                        passwordError={this.state.passwordError}
                    />
                    <SpinnerComponent
                        loading={this.state.loading}
                    />
                    <button onClick={this.login}>Sing in</button><br />
                    <span style={{ color: "red" }}>{this.state.errorMessage}</span><br />
                </div>
                <Footer />
            </div>
        )
    }

}

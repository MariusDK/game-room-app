import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import LoginForm from './LoginForm/LoginForm'
import PlayerService from 'src/services/PlayerService';
import { IPlayer } from 'src/models/IPlayer';
import { SpinnerComponent } from 'src/components/Spinner/Spinner';
import "../LoginPlayer/LoginPlayer.css";
import Header from 'src/components/StartPage/Header/Header';
import Footer from 'src/components/Footer/Footer';
import FacebookLogin from 'react-facebook-login';
import GoogleLogin from 'react-google-login';



export interface ILoginPlayerState {
    username: string;
    password: string;
    usernameError: string;
    passwordError: string;
    loading: boolean;
    redirect: boolean;
    errorMessage: string;
    appId:string;
    clientId:string;
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
            errorMessage: '',
            appId: "1810690805706216",
            clientId:"793667409742-i7s4vr8kmea5gsbkomon4ustrn683em1.apps.googleusercontent.com"
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
        this.setState({ errorMessage: "" });
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
    responseFacebook = (response:any) => {
        this.setState({ errorMessage: "" });
        if (response!=undefined){
        console.log(response.email);
        this.setState({username:response.email});
        PlayerService.getPlayerByUsername(this.state.username).then((player: IPlayer) => {
            if (!player) {
                this.setState({ errorMessage: "Player Not Found!" });
            }
            else {
                localStorage.setItem('currentUser', JSON.stringify(player));
                this.setState({ redirect: true });
            }
        });
    }
    }
    responseGoogle = (response:any) => {
        this.setState({ errorMessage: "" });
        if (response!=undefined){
            this.setState({username:response.w3.U3});
            PlayerService.getPlayerByUsername(this.state.username).then((player: IPlayer) => {
                if (!player) {
                    this.setState({ errorMessage: "Player Not Found!" });
                }
                else {
                    localStorage.setItem('currentUser', JSON.stringify(player));
                    this.setState({ redirect: true });
                }
            });
    }
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
                <div className="LoginForm">
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
                    <button className="loginBtn" onClick={this.login}>Sign in</button><br />
                    </div>
                    <div className="socialExperience">
                    <FacebookLogin
                    textButton=""
                    size="small"
                    cssClass="facebookLogin"
                    icon={require("src/Resurces/facebook_icon.png")}
                    appId={this.state.appId}
                    autoLoad={false}
                    fields="name,email"
                    callback={this.responseFacebook}
                    />
                    <GoogleLogin
                        className="googleLogin"
                        clientId={this.state.clientId}
                        buttonText=""
                        onSuccess={this.responseGoogle}
                        onFailure={this.responseGoogle}
                        autoLoad={false}
                        />
                    </div>
                    <span className="errorMessage" style={{ color: "red" }}>{this.state.errorMessage}</span><br />
                </div>
                <Footer />
            </div>
        )
    }

}

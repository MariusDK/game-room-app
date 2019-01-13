import * as React from 'react';
import { RouteComponentProps, Redirect } from 'react-router';
import PlayerService from 'src/services/PlayerService';
import PlayerForm from './PlayerForm/PlayerForm';
import { IPlayer } from 'src/models/IPlayer';
import { SpinnerComponent } from 'src/components/Spinner/Spinner';
import './RegisterPlayer.css';
import Header from 'src/components/StartPage/Header/Header';
import Footer from 'src/components/Footer/Footer';
import FacebookLogin from 'react-facebook-login';
import GoogleLogin from 'react-google-login';

export interface IRegisterPlayerState {
    infoMessage: string;
    name: string;
    username: string;
    password: string;
    age: number;
    nameError: string;
    usernameError: string;
    passwordError: string;
    ageError: string;
    loading: boolean;
    redirect: boolean;
    blur:boolean;
    appId:string;
    clientId:string;
    serverError:string;
}

export interface IRegisterPlayerProps extends RouteComponentProps<any> { }

export default class RegisterPlayer extends React.Component<IRegisterPlayerProps, IRegisterPlayerState>
{
    constructor(props: IRegisterPlayerProps) {
        super(props);
        this.state = {
            name: '',
            username: '',
            password: '123456',
            age: 0,
            nameError: '',
            usernameError: '',
            passwordError: '',
            infoMessage: '',
            ageError: '',
            loading: false,
            redirect: false,
            blur:false,
            appId: '1810690805706216',
            clientId:'793667409742-i7s4vr8kmea5gsbkomon4ustrn683em1.apps.googleusercontent.com',
            serverError: ''
        }
    }

    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IRegisterPlayerState) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }

    handleValidation() {
        let name = this.state.name;
        let username = this.state.username;
        let password = this.state.password;
        let age = this.state.age;
        let nameError = '';
        let usernameError = '';
        let passwordError = '';
        let ageError = '';
        let formIsValid = true;
        if (!name) {
            formIsValid = false;
            nameError = "Name field cannot be empty!";
        }
        if (!username) {
            formIsValid = false;
            usernameError = "Username field cannot be empty!";
        }
        else if (username.length < 6) {
            formIsValid = false;
            usernameError = "Username must hava more than 5 characters!";
        }
        if (!password) {
            formIsValid = false;
            passwordError = "Password must have more than 5 characters!";
        }
        if (age == 0) {
            formIsValid = false;
            ageError = "Age field cannot be empty!";
        }
        this.setState({ nameError: nameError });
        this.setState({ usernameError: usernameError });
        this.setState({ passwordError: passwordError });
        this.setState({ ageError: ageError });
        return formIsValid;
    };

    addPlayer = () => {
        this.setState({serverError: ""});
        if (this.handleValidation()) {
            const newPlayer: IPlayer = {
                name: this.state.name,
                username: this.state.username,
                password: this.state.password,
                age: this.state.age
            }
            PlayerService.insertPlayer(newPlayer)
                .then((response: string) => {
                    this.setState({ infoMessage: response });
                    if (response == "Insert Working!") {
                        console.log(this.state.redirect);
                        this.setState({ redirect: true });
                        console.log(this.state.redirect);
                    }
                    else{
                        this.setState({serverError: response});
                    }
                });
        }
        else {
            console.log("Error");

        }
        this.setState({ loading: false });
    }
    responseFacebook = (response:any) => {
        this.setState({serverError: ""});
        if (response!=undefined){
            
            this.setState({name:response.name, username:response.email});
            const newPlayer: IPlayer = {
                name: this.state.name,
                username: this.state.username,
                password: '123456',
                age: this.state.age
            }
            console.log(newPlayer);
        PlayerService.insertPlayer(newPlayer)
        .then((response: string) => {            
            this.setState({ infoMessage: response });
            if (response == "Insert Working!") {
                console.log(response);
                this.setState({ redirect: true });
            }
            else{
                this.setState({serverError: response});
            }
        });
    }
    }  
    responseGoogle = (response:any) => {
        this.setState({serverError: ""});
        if (response!=undefined){
            this.setState({name:response.w3.ig, username:response.w3.U3});
            const newPlayer: IPlayer = {
                name: this.state.name,
                username: this.state.username,
                password: '123456',
                age: this.state.age
            }
            console.log(newPlayer);
        PlayerService.insertPlayer(newPlayer)
        .then((response: string) => {
            this.setState({ infoMessage: response });
            if (response == "Insert Working!") {
                this.setState({ redirect: true });
            }
            else{
                this.setState({serverError: response});
            }
        });
    }
    }

    public render() {
        const redirect = this.state.redirect;
        if (redirect) {
            return <Redirect to='/' />
        }
        return (
            <div>
                <Header />
                <div className="registerPanel">
                <div className="registerPlayerForm">
                    <PlayerForm
                        name={this.state.name}
                        username={this.state.username}
                        password={this.state.password}
                        age={this.state.age}
                        handleChange={this.handleChange}
                        nameError={this.state.nameError}
                        usernameError={this.state.usernameError}
                        passwordError={this.state.passwordError}
                        ageError={this.state.ageError}
                        title="Create your account"
                    />
                </div>
                    <SpinnerComponent
                        loading={this.state.loading}
                    />
                    <button className="registerBtn" onClick={this.addPlayer}>Register</button><br />
                    <div className="socialExperienceRegister">
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
                    <span className="errorMessageRegister" style={{ color: "red" }}>{this.state.serverError}</span><br />
                </div>
                <Footer />
            </div>
        )
    }

}
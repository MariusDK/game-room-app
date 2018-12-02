import * as React from 'react';
import {RouteComponentProps, Redirect} from 'react-router';
import PlayerService from 'src/services/PlayerServices';
import PlayerForm from './PlayerForm/PlayerForm';
import { SpinnerComponent } from '../Spinner/Spinner';

export interface IRegisterPlayerState {
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
}

export interface IRegisterPlayerProps extends RouteComponentProps<any>{}

export default class RegisterPlayer extends React.Component<IRegisterPlayerProps,IRegisterPlayerState>
{
    constructor(props: IRegisterPlayerProps)
    {
        super(props);
        this.state = {
            name: '',
            username: '',
            password: '',
            age: 0,
            nameError: '',
            usernameError: '',
            passwordError: '',
            ageError: '',
            loading: false,
            redirect: false
        }
    }

    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IRegisterPlayerState)=>(
            {
                ...prevState,
                [name]:value
            }
        ));
    }
    handleValidation (){
        let name = this.state.name;
        let username = this.state.username;
        let password = this.state.password;
        let age = this.state.age;
        let nameError = '';
        let usernameError = '';
        let passwordError = '';
        let ageError = '';
        let formIsValid = true;
        if (!name)
        {
            formIsValid = false;
            nameError = "Name field cannot be empty";
        }
        else if (typeof name !== "undefined")
        {
            if (!name.match(/^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$/))
            {
                formIsValid = false;
                nameError = "Only letters";
            }
        }
        if (!username)
        {
            formIsValid = false;
            nameError = "Username field cannot be empty!";
        }
        else if (username.length < 6)
        {
            formIsValid = false;
            usernameError = "Username must hava more than 5 characters!";
        }
        if (!password)
        {
            formIsValid = false;
            passwordError = "Password must have more than 5 characters!";
        }
        if (age==0)
        {
            formIsValid = false;
            ageError = "Age field cannot be empty!";
        }
        this.setState({nameError:nameError});
        this.setState({usernameError:usernameError});
        this.setState({passwordError:passwordError});
        this.setState({ageError:ageError});
        return formIsValid;
    };

    addPlayer = () => {
        
        this.setState({loading:true});
        if (this.handleValidation())
        {
            console.log("Aici");
                const newPlayer: IPlayer = {
                    name: this.state.name,
                    username: this.state.username,
                    password: this.state.password,
                    age: this.state.age
                }
            PlayerService.insertPlayer(newPlayer)
                        .then(() => {
                            this.props.history.push('/')
                });
            this.setState({redirect:true})
        }
        else{
            console.log("Error");
        }
        this.setState({loading:false});
    }
    

    public render() {
        const {redirect} = this.state;
        if (redirect)
        {
            <Redirect to='/'/>
        }
        return (
            <div className="App">
                <PlayerForm
                    name={this.state.name}
                    username={this.state.username}
                    password={this.state.password}
                    age={this.state.age}
                    handleChange = {this.handleChange}
                    nameError={this.state.nameError}
                    usernameError={this.state.usernameError}
                    passwordError={this.state.passwordError}
                    ageError={this.state.ageError}
                    title = "Register"
                />
                <SpinnerComponent 
                    loading = {this.state.loading}
                />
                <button onClick={this.addPlayer}>Register</button>
            </div>
        )
    }

}
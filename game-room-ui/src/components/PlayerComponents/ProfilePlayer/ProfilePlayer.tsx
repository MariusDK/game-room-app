import * as React from 'react';
import PlayerService from 'src/services/PlayerService';
import PlayerForm from '../RegisterPlayer/PlayerForm/PlayerForm';
import { IRegisterPlayerState } from '../RegisterPlayer/RegisterPlayer';
import { RouteComponentProps } from 'react-router-dom';
import { IPlayer } from 'src/models/IPlayer';
import Navigation from 'src/components/Header/Navigation/Navigation';
import { SpinnerComponent } from 'src/components/Spinner/Spinner';
import "./ProfilePlayer.css"
import Footer from 'src/components/Footer/Footer';

export interface IProfilePlayerProps extends RouteComponentProps<any> { }
export default class ProfilePlayer extends React.Component<any, IRegisterPlayerState>
{
    constructor(props: IProfilePlayerProps) {
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
            redirect: false,
            infoMessage: '',
            blur:false,
            appId:'',
            clientId:'',
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
            nameError = "Name field cannot be empty";
        }
        else if (typeof name !== "undefined") {
            if (!name.match(/^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$/)) {
                formIsValid = false;
                nameError = "Only letters";
            }
        }
        if (!username) {
            formIsValid = false;
            nameError = "Username field cannot be empty!";
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

    componentDidMount() {
        let json = localStorage.getItem("currentUser");
        var currentUser = null;
        if (json != null) {
            currentUser = JSON.parse(json)
        }
        let id = currentUser.id;
        let password = currentUser.password;
        PlayerService.getPlayerById(id).then((user: IPlayer) => {
            this.setState({
                name: user.name,
                username: user.username,
                password: password,
                age: user.age
            });
        });

    }
    updatePlayer = () => {
        if (this.handleValidation()) {
            let json = localStorage.getItem("currentUser");
            var currentUser = null;
            if (json != null) {
                currentUser = JSON.parse(json)
            }
            let id = currentUser.id;
            currentUser.name = this.state.name;
            currentUser.username = this.state.username;
            currentUser.password = this.state.password;
            currentUser.age = this.state.age;
            PlayerService.updatePlayer(id, currentUser);
        }
        else {
            console.log("Error");
        }
    }
    onAddBlur=()=>
    {
        this.setState({blur:true});
    }
    onRemoveBlur=()=>{
        this.setState({blur:false});
    }
    public render() {
        return (
            <div>
                <div>
                    <Navigation
                        onAddBlur={this.onAddBlur}
                        onRemoveBlur={this.onRemoveBlur}
                     />
                </div>
                <div className="profile">
                    <div className={this.state.blur?"hideProfilePanel":"profilePanel"}>
                    <div className="playerForm">
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
                            title="Profile"
                        />
                    </div>
                    <div>
                        <SpinnerComponent
                            loading={this.state.loading}
                        />
                    </div>

                    <button onClick={this.updatePlayer}>Update</button>
                </div>
                </div>
                <div>
                    <Footer />
                </div>
            </div>
        )
    }
}
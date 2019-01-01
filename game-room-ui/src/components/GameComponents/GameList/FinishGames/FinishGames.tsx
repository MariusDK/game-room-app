import * as React from 'react';
import { IGame } from 'src/models/IGame';
import GameService from 'src/services/GameService';
import { ITeam } from 'src/models/ITeam';
import { Redirect } from 'react-router';
import Navigation from 'src/components/Header/Navigation/Navigation';
import Game from '../GameListForm/Game';
import Footer from 'src/components/Footer/Footer';
import "./FinishGames.css";
import { ClipLoader } from 'react-spinners';

export interface IFinishGamesState {
    fgames: IGame[];
    selectedGames: IGame[];
    loading: boolean;
    redirect: boolean;
    gameType: string;
    pageNumber: number;
}
export default class GameList extends React.Component<any, IFinishGamesState>
{
    constructor(props: any) {
        super(props);
        this.state = {
            fgames: [],
            selectedGames: [],
            loading: true,
            redirect: false,
            gameType: "solo",
            pageNumber:0
        }
    }
    componentDidMount() {
        this.setState({selectedGames:[]});
        console.log(this.state.selectedGames);
        this.getGamesOfUser(0);
    }
    getGamesOfUser(pageNumber:number)
    {
        let currentUser = localStorage.getItem("currentUser");
        if (currentUser != null) {
            var obj = JSON.parse(currentUser);
            GameService.getGamesFinishOfPlayer(pageNumber,obj.id).then((result: IGame[]) => {
                this.setState({ fgames: result, loading: false, pageNumber: pageNumber });
                console.log(this.state.pageNumber);
            });
        }
    }
    public selectGame = (game: IGame) => {
        if (!game.id) return;
        var name = game.name;
        localStorage.setItem('currentGame', name);
        var teams: ITeam[] = game.teams;
        teams.forEach(element => {
            if (element.players.length > 1) {
                var gameType = "multi";
                this.setState({ gameType: gameType })
            }
        });
        this.setState({ redirect: true });
        if (game.endOn != null) {
            localStorage.setItem("finishGame", "true");
        }
        else {
            localStorage.setItem("finishGame", "false");
        }
    }
    nextPage=()=>
    {
        let pageNumber = this.state.pageNumber;
        pageNumber++;
        this.setState({pageNumber:pageNumber});
        this.getGamesOfUser(pageNumber);
    }
    backPage=()=>
    {
        let pageNumber = this.state.pageNumber;
        pageNumber--;
        this.setState({pageNumber:pageNumber});
        this.getGamesOfUser(pageNumber);
    }
    deleteGames=()=>
    {
        this.setState({loading:true});
        this.state.selectedGames.forEach(element => {
            GameService.deleteGame(element.id).then(() => {
            });
        });
        this.getGamesOfUser(this.state.pageNumber);
        
    }
    render() {
        console.log(this.state.pageNumber);
        if (this.state.redirect) {
            console.log(this.state.gameType);
            if (this.state.gameType == "solo") {
                return <Redirect to='/gameSoloPage' />
            }
            else {
                return <Redirect to='/gameTeamPage' />
            }
        }
        return (
            <div>
                <div><Navigation /></div>
                <div className="finishGameList">
                <div className="title">
                <h1>Finish Game List</h1>
                </div>
                {this.state.loading && <h1>Loading</h1>}
                <div className="finishGameL">
                {!this.state.loading &&
                    this.state.fgames.map((item, index) => (
                        <Game
                            key={index}
                            game={item}
                            selectGame={this.selectGame}
                            listSelectedGames = {this.state.selectedGames}
                        >
                        </Game>       
                    )
                )}
                </div>
                <div>
                    <ClipLoader
                    color={'#123abc'}
                    loading={this.state.loading}
                     />
                </div>
                {!this.state.loading &&  
                    this.state.fgames.length==2 && this.state.pageNumber >= 0 && (
                    <button className="nextAndBackBtn" onClick={this.nextPage} >Next</button>
                )}
                {(!this.state.loading && 
                    this.state.fgames.length<=2 && this.state.pageNumber >= 1 &&
                        <button className="nextAndBackBtn" onClick={this.backPage}>Back</button>
                )}
                {(!this.state.loading && 
                    this.state.fgames.length>0 &&
                    <button className="nextAndBackBtn" onClick={this.deleteGames}>Delete Games</button>)}
                </div>
                <div><Footer/></div>
            </div>
        )
    }


}
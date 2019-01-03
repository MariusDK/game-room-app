import * as React from 'react';
import { IGame } from 'src/models/IGame';
import GameService from 'src/services/GameService';
import { ITeam } from 'src/models/ITeam';
import { Redirect } from 'react-router';
import Navigation from 'src/components/Header/Navigation/Navigation';
import Game from '../GameListForm/Game';
import Footer from 'src/components/Footer/Footer';
import { ClipLoader } from 'react-spinners';
import DropdownFilter from '../DropdownOptions/DropdownFilter';

export interface IUnfinishGamesState {
    fgames: IGame[];
    selectedGames: IGame[];
    loading: boolean;
    redirect: boolean;
    gameType: string;
    pageNumber: number;
    gameName: string;
    errorMessage: string;
    filter: boolean;
    filterType: string;
    displayMenu: boolean;
    ordered: boolean;
}
export default class UnfinishGames extends React.Component<any, IUnfinishGamesState>
{
    constructor(props: any) {
        super(props);
        this.state = {
            fgames: [],
            selectedGames: [],
            loading: true,
            redirect: false,
            gameType: "solo",
            pageNumber: 0,
            gameName: "",
            errorMessage: "",
            filter: false,
            filterType: "",
            displayMenu: false,
            ordered: false
        }
    }
    componentDidMount() {
        this.setState({ selectedGames: [] });
        this.getGamesOfUser(0);
    }
    getGamesOfUser(pageNumber: number) {
        this.setState({ fgames: [] });
        let currentUser = localStorage.getItem("currentUser");
        if (currentUser != null) {
            var obj = JSON.parse(currentUser);
            GameService.getGamesFinishOfPlayer(pageNumber, obj.id).then((result: IGame[]) => {
                this.setState({ fgames: result, loading: false, pageNumber: pageNumber });
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
    nextPage = () => {
        let pageNumber = this.state.pageNumber;
        pageNumber++;
        this.setState({ pageNumber: pageNumber });

        if (this.state.filter == true) {
            this.getByFilter(this.state.filterType);
        }
        else if (this.state.ordered == true) {
            this.orderByLastFinish(pageNumber);
        } else {
            this.getGamesOfUser(pageNumber);
        }
    }
    backPage = () => {
        let pageNumber = this.state.pageNumber;
        pageNumber--;
        this.setState({ pageNumber: pageNumber });
        if (this.state.filter == true) {
            this.getByFilter(this.state.filterType);
        }
        else if (this.state.ordered == true) {
            this.orderByLastFinish(pageNumber);
        } else {
            this.getGamesOfUser(pageNumber);
        }
    }
    deleteGames = () => {
        this.setState({ loading: true });
        this.state.selectedGames.forEach(element => {
            GameService.deleteGame(element.id).then(() => {
            });
        });
        this.getGamesOfUser(this.state.pageNumber);

    }
    handleChange = (e: any) => {
        const { name, value } = e.target;
        this.setState((prevState: IUnfinishGamesState) => (
            {
                ...prevState,
                [name]: value
            }
        ));
    }
    searchGame = () => {
        this.setState({ errorMessage: "" });
        this.setState({ fgames: [], loading: true });
        var listGames: IGame[] = [];
        if (this.state.gameName == "") {
            this.getGamesOfUser(0);
        }
        else {
            GameService.getGameByName(this.state.gameName).then((result: IGame) => {
                if (result.id == undefined) {
                    this.setState({ errorMessage: "Game not found!", loading: false });
                }
                else {
                    listGames.push(result)
                    this.setState({ fgames: listGames, loading: false, pageNumber: 0 });
                }
            });
        }
    }

    getByFilter = (filterType: string) => {
        this.setState({ fgames: [] });
        if (this.state.filter == false) {
            this.setState({ pageNumber: 0 });
        }
        this.setState({ filter: true });
        let currentUser = localStorage.getItem("currentUser");
        if (currentUser != null) {
            var obj = JSON.parse(currentUser);
            GameService.getGamesFinishOfPlayerAndType(this.state.pageNumber, filterType, obj.id).then((result: IGame[]) => {
                this.setState({ fgames: result, loading: false, filterType: filterType });
            });
        }
    }
    unfilter = () => {
        this.setState({ pageNumber: 0, filter: false, ordered: false, filterType: "" });
        this.getGamesOfUser(0);
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
    orderByLastFinish = (pageNumber: number) => {

        this.setState({ fgames: [], loading: true });
        if (this.state.ordered == false) {
            this.setState({ pageNumber: 0 });
            pageNumber = 0;
        }
        this.setState({ ordered: true });
        let currentUser = localStorage.getItem("currentUser");
        if (currentUser != null) {
            var obj = JSON.parse(currentUser);
            GameService.getFinishGamesOrderByEndDate(pageNumber, obj.id).then((result: IGame[]) => {
                this.setState({ fgames: result, loading: false, pageNumber: pageNumber });
            });
        }
    }
    render() {
        if (this.state.redirect) {
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
                <div className="unfinishGameList">
                    <div className="searchAndDropdownPanel">
                        <div className="searchPanel">
                            <input type="text" value={this.state.gameName} name="gameName" onChange={this.handleChange} placeholder="Game Name" />
                            <button className="searchBtn" onClick={this.searchGame}>Search</button>
                        </div>
                        <div className="dropdownFilter">
                            <button onClick={this.showDropdown} className="dropdownFilterBtn">Games</button>
                            <div className="myFilterDropdown" id="idDropdownFilter">
                                <DropdownFilter
                                    displayMenu={this.state.displayMenu}
                                    filter={this.getByFilter}
                                    reset={this.unfilter}
                                    orderByMostRecent={this.orderByLastFinish}
                                />
                            </div>
                        </div>
                    </div>
                    <div className="title">
                        <h1>Finish Game List</h1>
                    </div>
                    {this.state.loading && <h1>Loading</h1>}
                    <div className="unfinishGameL">
                        {!this.state.loading &&
                            this.state.fgames.map((item, index) => (
                                <Game
                                    key={index}
                                    game={item}
                                    selectGame={this.selectGame}
                                    listSelectedGames={this.state.selectedGames}
                                >
                                </Game>
                            )
                            )}
                    </div>
                    <div className="clipLoader">
                        <ClipLoader
                            color={'#123abc'}
                            loading={this.state.loading}
                        />
                    </div>
                    {!this.state.loading &&
                        this.state.fgames.length == 4 && this.state.pageNumber >= 0 && (
                            <button className="nextAndBackBtn" onClick={this.nextPage} >Next</button>
                        )}
                    {(!this.state.loading &&
                        this.state.fgames.length <= 4 && this.state.pageNumber >= 1 &&
                        <button className="nextAndBackBtn" onClick={this.backPage}>Back</button>
                    )}
                    {(!this.state.loading &&
                        this.state.fgames.length > 0 &&
                        <button className="nextAndBackBtn" onClick={this.deleteGames}>Delete Games</button>)}
                    <span style={{ color: "red" }}>{this.state.errorMessage}</span><br />
                </div>

                <div><Footer /></div>
            </div>
        )
    }
}
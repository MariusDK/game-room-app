import * as React from 'react';
import { Link } from 'react-router-dom';

export default class DropdownMenu extends React.Component<any, any>
{
    constructor(props:any)
    {
        super(props);
        this.state = {
            displayMenu: false,
        };
    };
    showDropdown=(e:any)=> {
        e.preventDefault();
        this.setState({displayMenu:true});
      }
    cancelDropdown=(e:any)=> {
        this.setState({displayMenu:false});
      }
    render()
    {
        return (
            <div >
        <button onClick={this.showDropdown}>Games</button>
        
            {console.log(this.state.displayMenu)}
          { this.state.displayMenu ? (
          <ul>
         <li><Link to="/createGameSolo">Create Game Solo</Link></li>
         <li><Link to="/createTeamGame">Create Game Team</Link></li>
         <li><Link to="/games">Game List</Link></li>
          </ul>
        ):
        (
          null
        )
        }

       </div>
        )
    }
}
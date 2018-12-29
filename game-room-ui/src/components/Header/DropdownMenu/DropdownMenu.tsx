import * as React from 'react';
import { Link } from 'react-router-dom';
import './DropdownMenu.css';
export interface IDropdownProps {
    displayMenu:boolean;
}
export default class DropdownMenu extends React.Component<IDropdownProps, any>
{
    constructor(props:any)
    {
        super(props);
        this.state = {
            
        };
    };
    render()
    {
        return (
            <div>
            {this.props.displayMenu ?(
            <div className="dropDownMenu">
          <ul>

         <li><Link to="/createGameSolo">Create Game Solo</Link></li>
         <li><Link to="/createTeamGame">Create Game Team</Link></li>
         <li><Link to="/games">Game List</Link></li>
          </ul>

       </div>
       ):(null)}
       </div>
        )
    }
}
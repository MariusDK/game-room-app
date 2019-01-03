import * as React from 'react';
import { Link} from 'react-router-dom';
import "./Footer.css";
export default class Footer extends React.Component<any, any>
{
    constructor(props: any)
    {
        super(props);
        this.state = {
        }
    }

    public render()
    {
        return (
            <footer>
                <div className="about">
                    <Link to="">About</Link>
                </div>
                <div className="m-year">
                    <img src={require('src/Resurces/year.png')}/>
                </div>
                <div className="conditions">
                    <Link to="">Terms and Conditions</Link>
                </div> 
            </footer>
        )
    }
}
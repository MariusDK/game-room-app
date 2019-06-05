import * as React from 'react';

export interface IDropdownProps {
    displayMenu: boolean;
    filter(gameType: string,pageNumber:number): void;
    reset(): void;
    orderByMostRecent(pageNumber: number): void;
}
export default class DropdownFilter extends React.Component<IDropdownProps, any>
{
    constructor(props: any) {
        super(props);
        this.state = {

        };
    };
    render() {
        return (
            <div>
                {this.props.displayMenu ? (
                    <div className="dropdownFilter">
                        <ul>
                            <li><button onClick={() => this.props.orderByMostRecent(0)}>Order by the most recent</button></li>
                            <li><button onClick={() => this.props.filter("Darts/X01",0)}>Show only DartsX01</button></li>
                            <li><button onClick={() => this.props.filter("Darts/Cricket",0)}>Show only DartsCricket</button></li>
                            <li><button onClick={() => this.props.filter("Foosball",0)}>Show only Foosball</button></li>
                            <li><button onClick={() => this.props.filter("any",0)}>Show the rest of games</button></li>
                            <li><button onClick={() => this.props.reset()}>Reset</button></li>
                        </ul>

                    </div>
                ) : (null)}
            </div>
        )
    }
}
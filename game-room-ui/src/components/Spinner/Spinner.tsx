import * as React from 'react';
import { ClipLoader } from 'react-spinners';
import './Spinner.css';

export class SpinnerComponent extends React.Component<any,any>
{
    constructor(props: any)
    {
        super(props);
        this.state = {
            loading: false,
        }
    }
    render()
    {
        return (
            <div className='sweet-loading'>
            <ClipLoader 
                color={'#123abc'}
                loading={this.props.loading}
                />
            </div>
        )

    }
}
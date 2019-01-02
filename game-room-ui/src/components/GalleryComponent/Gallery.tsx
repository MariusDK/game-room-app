import * as React from 'react';
import './Gallery.css';
import { Link } from 'react-router-dom';

function imagesLoaded(parentNode:any)
    {
        const imgElements = parentNode.querySelectorAll("img");
        for (const img of imgElements)
        {
            if (!img.complete)
            {
                return false;
            }
        }
        return true;
    }
export interface GalleryProps {
    moments:string[];
    listType:string;
    gameName:any;
}
export interface GalleryState {
    loading: boolean;
    moments: string[];
    imgPosition: number;
}
export default class Gallery extends React.Component<GalleryProps,GalleryState>{
    galleryElement: HTMLDivElement | null;

    constructor(props:GalleryProps)
    {
        super(props);
        this.state ={
            loading : true,
            moments : [],
            imgPosition: 0
        };
    }
    componentDidMount()
    {
        this.setState({moments:this.props.moments});
        console.log(this.state.moments);
    }
    componentWillReceiveProps(props:GalleryProps) {
         this.setState({loading:true});
         this.setState({moments:this.props.moments});
       }
    clickImg=(moment:string)=>
    {
        var i=0;
        this.props.moments.forEach(element => {
            //console.log(i);
            if (element==moment)
            {

                console.log(i);
                localStorage.setItem('ImgPositionAndListType', i+","+this.props.listType+","+this.props.gameName);
                localStorage.setItem('clickedImg',`data:image/png;base64,${moment}`);
                
            }
            i++;
        });
    }
    renderImage(moment:string){
        var address = `data:image/png;base64,${moment}`;
        return (
            <div className="imgGallery" onClick={()=>this.clickImg(moment)}>
        
                <Link to='/imageAnalyzer' className="Links" > 
                    <img src={address} onLoad={this.handleStateChange} onError={this.handleStateChange}/>
           </Link> </div>
        );
    }
    
    handleStateChange = () =>
    {
        this.setState({
            loading: !imagesLoaded(this.galleryElement),
        });
    }
    renderSpinner()
    {
        if (!this.state.loading)
        {
            return null;
        }
        return (
            <span className="spinner" />
        );
    }
    render()
    {
        console.log("galerie");
        return (
            <div className="gallery" ref={element=>{this.galleryElement = element;}}>
                {this.renderSpinner()}
                <div className="m_images">
                {this.state.moments.map(moment => this.renderImage(moment))}
                </div>
            </div>
        )
    }
}
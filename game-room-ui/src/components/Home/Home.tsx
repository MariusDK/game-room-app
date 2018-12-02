import * as React from 'react';

import StartPage from '../StartPage/StartPage';
import ProfilePlayer from '../ProfilePlayer/ProfilePlayer';


export default function Home()
{
   let json = localStorage.getItem("currentUser");
   var currentUser = null;
   if (json!=null)
   {
   currentUser = JSON.parse(json)
   }
   
  console.log(currentUser);
  if (currentUser!=null)
  {
    return <ProfilePlayer/>;
  }
  return <StartPage/>;
}

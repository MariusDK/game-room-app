import * as React from 'react';
import './TeamCard.css';

const TeamCard = (props:any) => {
    console.log(props);
    const options = props.results.map((r:any) => (
      <div key={r.id} className="teamCard">
      <article>
        <img src={require('src/Resurces/team.png')}></img>
        </article>
        <section>
        Name: {r.name}
        </section>
        </div>
    ))
    return <ul>{options}</ul>
  }
  
  export default TeamCard
import * as React from 'react'
import './Suggestions.css';

const Suggestions = (props:any) => {
  const options = props.results.map((r:any) => (
    <div key={r.id} className="personCard">
    <article>
      <img src={require('src/Resurces/person.png')}></img>
      </article>
      <section>
      Name: {r.name}
      </section>
      </div>
  ))
  return <ul>{options}</ul>
}

export default Suggestions
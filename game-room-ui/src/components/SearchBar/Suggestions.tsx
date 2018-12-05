import * as React from 'react'

const Suggestions = (props:any) => {
  const options = props.results.map((r:any) => (
    <li key={r.id}>
      {r.name}
    </li>
  ))
  return <ul>{options}</ul>
}

export default Suggestions
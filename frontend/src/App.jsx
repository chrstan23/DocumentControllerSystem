import { useEffect, useState } from 'react'
import { getProcedures } from './api/api'
import MasterlistTable from "./components/MasterlistTable"

function App() {
  const [procedures, setProcedures] = useState([]);
  const [error, setError] = useState("");

  useEffect(() => {
    getProcedures().then(setProcedures).catch((err) => setError(err.message));
  }, []) ;

  return (
    <div>
      <h1>Document Control System</h1>
      <h2>Masterlist</h2>

      {error && <p>error</p>}

      <MasterlistTable procedures={procedures}/>
    </div>
  );
}

export default App

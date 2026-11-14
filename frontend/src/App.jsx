import { useEffect, useState } from 'react'
import { getProcedures } from './api/api'
import MasterlistTable from "./components/MasterlistTable"
import ProcedureForm from './components/ProcedureForm';

function App() {
  const [procedures, setProcedures] = useState([]);
  const [error, setError] = useState("");
  const [refreshKey, setRefreshKey] = useState(0);

  useEffect(() => {
    getProcedures().then(setProcedures).catch((err) => setError(err.message));
  }, [refreshKey]);

  function handleAdded(){
    setRefreshKey(refreshKey + 1);
  }

  return (
    <div>
      <h1>Document Control System</h1>
      <ProcedureForm onAdded={handleAdded}/>

      <h2>Masterlist</h2>
      {error && <p>error</p>}
      <MasterlistTable procedures={procedures}/>
    </div>
  );
}

export default App

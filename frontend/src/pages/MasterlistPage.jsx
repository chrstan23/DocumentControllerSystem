import { useEffect, useState } from "react";
import { getProcedures } from "../api/api";
import MasterlistTable from "../components/MasterlistTable";

function MasterlistPage(){
    const [procedures, setProcedures] = useState([]);
    const [error, setError] = useState("");

    useEffect(() => {
        getProcedures().then(setProcedures).catch((err) => setError(err.message));
    }, []);

    return (
        <div>
            <h2>Document Masterlist</h2>
            {error && <p>{error}</p>}
            <MasterlistTable procedures={procedures}/>
        </div>
    );
}

export default MasterlistPage;
import { useEffect, useState } from "react";
import { getDashboard } from "../api/api";
import DashboardCard from "../components/DashboardCard";

function DashboardPage(){
    const [ summary, setSummary ] = useState(null);
    const [ error, setError ] = useState("");

    useEffect(() => {
        getDashboard().then(setSummary).catch((err) => setError(err.message));
    }, []);

    if (error){
        return <p>{error}</p>;
    }

    if (!summary) {
        return <p>Loading...</p>
    }

    return (
        <div>
            <h2>Document Dashboard</h2>
            <DashboardCard label="Total Documents" value={summary.totalDocuments} />
            <DashboardCard label="Under Revision" value={summary.underRevision} />

            <h2>Document Type Dashboard</h2>
            {summary.docTypeCounts.map((t) => (
                <DashboardCard key={t.name} label={t.name} value={t.total} />
            ))}
        </div>
        
    )
}

export default DashboardPage;
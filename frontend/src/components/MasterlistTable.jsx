import { getFileUrl } from "../api/api";

function MasterlistTable({ procedures }) {
    if (procedures.length === 0 ){
        return <p>No documents yet.</p>;
    }

    return (
        <table>
            <thead>
                <tr>
                    <th>Doc No</th>
                    <th>Title</th>
                    <th>Type</th>
                    <th>Section</th>
                    <th>Rev</th>
                    <th>Effectivity Date</th>
                    <th>Prepared By</th>
                    <th>File</th>
                </tr>
            </thead>
            <tbody>
                {procedures.map((p) => (
                    <tr key = {p.id}>
                        <td>{p.docNo}</td>
                        <td>{p.docTitle}</td>
                        <td>{p.docType}</td>
                        <td>{p.section}</td>
                        <td>{p.revision}</td>
                        <td>{p.effectivityDate}</td>
                        <td>{p.preparedBy}</td>
                        <td>
                            <a href={getFileUrl(p.id)} target = "_blank" rel = "noreffer">View</a>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    )
}

export default MasterlistTable;
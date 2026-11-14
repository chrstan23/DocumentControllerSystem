function RequestorSelect({ requestors, value, onChange}) {
    return (
        <select name="requestorId" value={value} onChange={onChange} required>
            <option value="">-- Select requestor --</option>
            {requestors.map((r) => (
                <option key={r.employeeId} value={r.employeeId}>
                    {r.fullName} ({r.section})
                </option>
            ))}
        </select>
    );
}

export default RequestorSelect;
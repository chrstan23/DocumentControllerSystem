function UserTable({ users, onToggle, onEdit, onDelete }){
    if (users.length === 0){
        return <p>No users found.</p>;
    }

    return (
        <table>
            <thead>
                <tr>
                    <th>Username</th>
                    <th>Fullname</th>
                    <th>Section</th>
                    <th>Role</th>
                    <th>Account Status</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                {users.map((u) => (
                    <tr key={u.employeeId}>
                        <td>{u.username}</td>
                        <td>{u.fullName}</td>
                        <td>{u.section}</td>
                        <td>{u.role}</td>
                        <td>
                            <label>
                                <input type="checkbox" checked={u.accountStatus === "ACTIVE"} onChange={() => onToggle(u)}/>{" "}
                                {u.accountStatus}
                            </label>
                        </td>
                        <td>
                            <button type="button" onClick={() => onEdit(u)}>
                                Edit
                            </button>
                        </td>
                        <td>
                            <button type="button" onClick={() => onDelete(u)}>
                                Delete
                            </button>
                        </td>
                    </tr>
                ))}
            </tbody>
        </table>
    );
}

export default UserTable;
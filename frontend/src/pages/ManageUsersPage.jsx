import { useEffect, useState } from "react";
import { deleteUser, getUsers, setUserStatus, updateUser } from "../api/api";
import UserForm from "../components/UserForm";
import UserTable from "../components/UserTable";

function ManageUsersPage() {
    const [users, setUsers] = useState([]);
    const [editingUser, setEditingUser] = useState(null);
    const [error, setError] = useState("");
    const [reloadKey, setReload] = useState(0);

    useEffect(() => {
        getUsers().then(setUsers).catch((err) => setError(err.message));
    }, [reloadKey]);

    function reload(){
        setReload(reloadKey + 1);
    }

    async function handleToggle(user) {
        setError("");
        try {
            await setUserStatus(user.employeeId, user.accountStatus !== "ACTIVE");
            reload();
        }catch (err) {
            setError(err.message);
        }
    }

    async function handleDelete(user) {
        if (!window.confirm(`Delete ${user.fullName}? This cannot be undone`)) {
            return;
        }

        setError("");
        try {
            await deleteUser(user.employeeId);
            reload();
        } catch (err) {
            setError(err.message);
        }
    }

    async function handleSave(form){
        await updateUser(editingUser.employeeId, form);
        setEditingUser(null);
        reload();
    }

    return (
        <div>
            <h2>Manage Users</h2>

            {error && <p>{error}</p>}

            {editingUser && (
                <UserForm 
                    key={editingUser.employeeId}
                    user={editingUser}
                    onSave={handleSave}
                    onCancel={() => setEditingUser(null)}
                />
            )}

            <UserTable 
                users={users}
                onToggle={handleToggle}
                onEdit={setEditingUser}
                onDelete={handleDelete}
            />
        </div>
    );
}

export default ManageUsersPage;
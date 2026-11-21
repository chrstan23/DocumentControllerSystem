import { useState } from "react";
import { ROLE_ITEMS } from "../roles";
import { USER_SECTION_ITEMS } from "../UserSection";
import LookupSelect from "./LookupSelect";

function UserForm({ user, onSave, onCancel }) {
    const [form, setForm] = useState({
        username: user.username,
        fullName: user.fullName,
        section: user.section, 
        role: user.role,
    });

    const [error, setError] = useState("");
    const [saving, setSaving] = useState(false);

    function handleChange(e){
        const { name, value } = e.target;
        setForm({ ...form, [name]: value });
    }

    async function handleSubmit(e) {
        e.preventDefault();
        setError("");
        setSaving(true);

        try{
            await onSave(form);
        }catch (err){
            setError(err.message);
        }finally{
            setSaving(false);
        }
    }

    return (
        <form onSubmit={handleSubmit}>
            <h3>Edit User</h3>

            <div>
                <label>Employee ID</label>
                <input value={user.employeeId} readOnly />
            </div>
            <div>
                <label>Username</label>
                <input name="username" value={form.username} onChange={handleChange} maxLength={50}  required/>
            </div>
            <div>
                <label>Full Name</label>
                <input name="fullName" value={user.fullName} onChange={handleChange} maxLength={100} required/>
            </div>
            <div>
                <label>Section</label>
                <LookupSelect 
                    name="section"
                    items={USER_SECTION_ITEMS}
                    value={form.section}
                    onChange={handleChange}
                    placeholder="-- Select section --"
                />
            </div>
            <div>                
                <label>Role</label>
                <LookupSelect name="role" items={ROLE_ITEMS} value={form.role} onChange={handleChange} placeholder="-- Select role --"/>
            </div>

            {error && <p>{error}</p>}

            <button type="submit" disabled={saving}>
                {saving ? "Saving..." : "Save"}
            </button>

            <button type="button" onClick={onCancel}>
                Cancel
            </button>
        </form>
    );
}

export default UserForm;
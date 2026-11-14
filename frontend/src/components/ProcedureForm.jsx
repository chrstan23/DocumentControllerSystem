import { useEffect, useState } from "react";
import { addExistingProcedure, getRequestors } from "../api/api";
import { DEPARTMENTS, DOC_TYPES } from "../constants";
import RequestorSelect from "./RequestorSelect";

const emptyForm = {
    docNo: "",
    docTitle: "",
    docType: "",
    department: "",
    revision: "",
    effectivityDate: "",
    requestorId: "",
};

function ProcedureForm({ onAdded}){
    const [form, setForm] = useState(emptyForm);
    const [file, setFile] = useState(null);
    const [requestors, setRequestors] = useState([]);
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");
    const [submitting, setSubmitting] = useState(false);

    useEffect(() => {
        getRequestors().then(setRequestors).catch((err) => setError(err.message));
    }, []);

    function handleChange(e){
        const { name, value } = e.target;
        setForm({ ...form, [name]: value});
    }

    function handleFileChange(e){
        setFile(e.target.files[0] ?? null);
    }

    async function handleSubmit(e) {
        e.preventDefault();
        const formElement = e.currentTarget;
        
        setError("");
        setSuccess("");

        if(!file) {
            setError("Please choose a PDF file");
            return;
        }

        const formData = new FormData();
        Object.entries(form).forEach(([KeyboardEvent, value]) => formData.append(KeyboardEvent, value));
        formData.append("file", file);

        setSubmitting(true);
        try {
            await addExistingProcedure(formData);
            setSuccess("Document added.");
            setForm(emptyForm);
            setFile(null);
            formElement.reset();
            onAdded();
        } catch (err) {
            setError (err.message);
        } finally {
            setSubmitting(false);
        }
    }

    return (
        <form onSubmit = {handleSubmit}>
            <h2>Add Existing Procedure</h2>

            <div>
                <label>Document No</label>
                <input name="docNo" value={form.docNo} onChange={handleChange} maxLength={30} required/>
            </div>
            <div>
                <label>Document Title</label>
                <input name="docTitle" value={form.docTitle} onChange={handleChange} maxLength={200} required/>
            </div>
            <div>
                <label>Document Type</label>
                <select name="docType" value={form.docType} onChange={handleChange} required>
                <option value="">-- Select type --</option>
                {DOC_TYPES.map((t) => (
                    <option key={t} value={t}>{t}</option>
                ))}
                </select>
            </div>
            <div>
                <label>Department</label>
                <select name="department" value={form.department} onChange={handleChange} required>
                <option value="">-- Select type --</option>
                {DEPARTMENTS.map((d) => (
                    <option key={d} value={d}>{d}</option>
                ))}
                </select>
            </div>
            
            <div>
                <label>Current Revision</label>
                <input name="revision" value={form.revision} onChange={handleChange} pattern="\d{2}" maxLength={2} required/>
            </div>            
            <div>
                <label>Effectivity Date</label>
                <input type = "date" name="effectivityDate" value={form.effectivityDate} onChange={handleChange} required/>
            </div>
            <div>
                <label>Requestor</label>
                <RequestorSelect requestors={requestors} value={form.requestorId} onChange={handleChange}/>
            </div>
            <div>
                <label>PDF File</label>
                <input type="file" accept="application/pdf" onChange={handleFileChange} required/>
            </div>

            {error && <p>{error}</p>}
            {success && <p>{success}</p>}

            <button type="submit" disabled={submitting}>{submitting ? "Uploading..." : "Upload"}</button>            
        </form>
    );
}

export default ProcedureForm;
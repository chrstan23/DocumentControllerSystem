import { useEffect, useState } from "react";
import { addExistingProcedure, getRequestors, getDocTypes, getSections } from "../api/api";
import RequestorSelect from "./RequestorSelect";
import LookupSelect from "./LookupSelect";

const emptyForm = {
    docNo: "",
    docTitle: "",
    docType: "",
    section: "",
    revision: "",
    effectivityDate: "",
    requestorId: "",
};

function ProcedureForm({ onAdded}){
    const [form, setForm] = useState(emptyForm);
    const [file, setFile] = useState(null);
    const [requestors, setRequestors] = useState([]);
    const [docTypes, setDocTypes] = useState([]);
    const [sections, setSections] = useState([]);
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");
    const [submitting, setSubmitting] = useState(false);

    useEffect(() => {
        Promise.all([getRequestors(), getDocTypes()])
            .then(([requestorList, docTypeList]) =>{
                setRequestors(requestorList);
                setDocTypes(docTypeList);
            })
        .catch((err) => setError(err.message));
        //getRequestors().then(setRequestors).catch((err) => setError(err.message));
    }, []);

    useEffect(() => {
        if (!form.docType) {
            setSections([]);
            return
        }

        getSections(form.docType)
        .then(setSections)
        .catch((err) => setError(err.message));
    }, [form.docType]);

    function handleChange(e){
        const { name, value } = e.target;

        if (name === "docType"){
            setForm({...form, docType: value, section: ""});
        }else{
            setForm({ ...form, [name]: value});            
        }
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
                <LookupSelect name="docType" items={docTypes} value={form.docType} onChange={handleChange} placeholder="-- Select section --"/>
            </div>
            <div>
                <label>Section</label>
                <LookupSelect name="section" items={sections} value={form.section} onChange={handleChange} placeholder={form.docType ? "-- Select section --" : "-- Select document type first --"} disabled={!form.docType}/>
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
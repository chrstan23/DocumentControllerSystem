const API_URL = import.meta.env.VITE_API_URL;

export async function getRequestors() {
    const response = await fetch(`${API_URL}/api/requestors`);

    if (!response.ok) {
        throw new Error("Failed to load requestors.");
    }

    return response.json();
}

export async function getSections(docType = "") {
    const query = docType ? `?docType=${encodeURIComponent(docType)}` : "";
    const response = await fetch(`${API_URL}/api/lookups/sections${query}`);

    if (!response.ok){
        throw new Error("Failed to load sections.")
    }

    return response.json();
}

export async function getDocTypes() {
    const response = await fetch(`${API_URL}/api/lookups/doctypes`);

    if (!response.ok){
        throw new Error("Failed to load document types.")
    }

    return response.json();
}

export async function getProcedures() {
    const response = await fetch (`${API_URL}/api/procedures`);

    if (!response.ok){
        throw new Error("Failed to load procedures.");
    }

    return response.json();
}

export function getFileUrl(id){
    return `${API_URL}/api/procedures/${id}/file`;
}

function readError(data) {
    if(!data) return "Something went wrong.";
    if (data.message) return data.message;
    if (data.errors) return Object.values(data.errors).flat().join(" ");
    return "Something went wrong.";
}

export async function addExistingProcedure(formData) {
    const response = await fetch(`${API_URL}/api/procedures/existing`, {
        method: "POST",
        body: formData,
    });

    if (!response.ok){
        const data = await response.json().catch(() => null);
        throw new Error(readError(data));        
    }
    return response.json();
}

export async function getDashboard() {
    const response = await fetch(`${API_URL}/api/dashboard`);

    if (!response.ok){
        throw new Error("Failed to load dashboard.");
    }

    return response.json();
}

async function sendRequest(path, options){
    const response = await fetch(`${API_URL}${path}`, options);

    if (!response.ok){
        const data = await response.json().catch(() => null);
        throw new Error(readError(data));
    }
}

export async function getUsers() {
    const response = await fetch(`${API_URL}/api/users`);

    if (!response.ok){
        throw new Error("Failed to load users.");
    }

    return response.json();
}

export function updateUser(employeeId, user){
    return sendRequest(`/api/users/${employeeId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user),
    });
}

export function setUserStatus(employeeId, active){
    return sendRequest(`/api/users/${employeeId}/status`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({active}),
    });
}

export function deleteUser(employeeId){
    return sendRequest(`/api/users/${employeeId}`, {
        method: "DELETE"
    });
}



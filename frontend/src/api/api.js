const API_URL = import.meta.env.VITE_API_URL;

export async function getRequestors() {
    const response = await fetch(`${API_URL}/api/requestors`);

    if (!response.ok) {
        throw new Error("Failed to load requestors.");
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

export async function getFileUrl(id){
    return `${API_URL}/api/procedure/${id}/file`;
}



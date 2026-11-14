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



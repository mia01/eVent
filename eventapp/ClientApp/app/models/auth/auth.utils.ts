export function getJwtToken() {
    return localStorage.getItem("jwt");
}

export function setJwtToken(token) {
    return localStorage.setItem("jwt", token);
}

export function removeJwtToken() {
    return localStorage.removeItem("jwt");
}
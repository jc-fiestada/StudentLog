document.addEventListener("DOMContentLoaded", async () => {
    
    const response = await fetch("/validate-admin-session", {
        method : "GET",
        credentials : "include",
    });

    const receivedData = await response.json();

    if (response.status === 401){
        console.log(receivedData.message);
        window.location.href = "signin.html";
        return;
    }

    console.log(receivedData.message);
    return;
});
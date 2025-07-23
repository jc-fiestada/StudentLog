import * as toastService from "./Toast.js";

document.getElementById("signin-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username-txtbox").value;
    const password = document.getElementById("password-txtbox").value;

    const response = await fetch("/validate-signin", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            username : username,
            password : password
        })
    });

    const dataResponse = await response.json();

    if (response.status === 422){
        dataResponse.data.forEach(error => {
            toastService.showToast("warning", error);
        });
        return;
    }

    if (response.status === 200){
        toastService.showToast("success", dataResponse.message);
        window.location.href = "home.html";
        return;
    }

    toastService.showToast("error", dataResponse.message);
});
document.getElementById("signin-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const username = document.getElementById("username-txtbox").value;
    const password = document.getElementById("password-txtbox").value;

    const response = await fetch("/validate-signin", {
        method : "POST",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            username : username,
            password : password
        })
    })

    const data = await response.json();

    debugger;
})
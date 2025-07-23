document.getElementById("student-form").addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById("name-txtbox").value;
    const sex = document.getElementById("sex-selection").value;
    const birthDate = document.getElementById("birth-date").value;

    const response = await fetch("/insert-student", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            name : name,
            sex : sex,
            birthDate : birthDate
        })});
    
    const responseData = await response.json();

    
});
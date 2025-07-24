import * as toastService from "./../Toast.js";

const deleteModal = document.getElementById("deleteModal");

function closeDeleteModal() {
    deleteModal.classList.add("hidden");
    deleteModal.dataset.deleteName = "";
}

const studentContainer = document.getElementById("student-data-container");

async function refreshList(){

    // too lazy to change the names, maybe later or tommorrow

    studentContainer.innerHTML = "";

    const request = await fetch("/select-all-student", {
        method : "GET",
        credentials : "include"
    });

    const response = await request.json();

    response.data.forEach(student => {
        

        studentContainer.innerHTML += `
            <div data-name="${student.name}" class="student-info-container bg-[#3b1525] p-4 rounded-lg shadow-md flex justify-between items-center">
                <div>
                    <p class="text-lg font-semibold text-white">${student.name}</p>
                    <p class="text-sm text-gray-300">Sex: ${student.sex}</p>
                    <p class="text-sm text-gray-300">Date of Birth: ${student.birthDate}</p>
                </div>
                <div class="space-x-2">
                    <button onclick="openUpdateModal('${student.name}')" class="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded text-sm">Update</button>
                    <button onclick="openDeleteModal('${student.name}')" class="bg-red-600 hover:bg-red-700 text-white px-4 py-2 rounded text-sm">Delete</button>
                </div>
            </div>
        `
    });
}

document.addEventListener("DOMContentLoaded", async () => {
    await refreshList();
})

document.getElementById("delete-yes-btn").addEventListener("click", async () => {
    const deleteModal = document.getElementById("deleteModal");
    const currentUser = deleteModal.dataset.deleteName;

    const response = await fetch("/delete-student", {
        method : "POST",
        credentials : "include",
        headers : {"Content-Type" : "application/json"},
        body : JSON.stringify({
            name : currentUser
        })});
    
    const requestData = await response.json();

    

    if (response.status === 200){
        toastService.showToast("success", requestData.message);
        closeDeleteModal();
        refreshList();
        return;
    }

    toastService.showToast("error", requestData.message);
    closeDeleteModal();
    refreshList();
});
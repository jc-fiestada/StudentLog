import * as toastNotif from "./Toast.js";

const updateModal = document.getElementById("updateModal");
const deleteModal = document.getElementById("deleteModal");
const updateStudentName = document.getElementById("updateStudentName");
const deleteStudentName = document.getElementById("deleteStudentName");

function openUpdateModal(name) {
    updateStudentName.textContent = `Update: ${name}`;
    updateModal.classList.remove("hidden");
}

function closeUpdateModal() {
    updateModal.classList.add("hidden");
}

function openDeleteModal(name) {
    deleteStudentName.textContent = `Are you sure you want to delete ${name}?`;
    deleteModal.classList.remove("hidden");
}

function closeDeleteModal() {
    deleteModal.classList.add("hidden");
}
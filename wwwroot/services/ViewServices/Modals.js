const updateModal = document.getElementById("updateModal");
const deleteModal = document.getElementById("deleteModal");
const updateStudentName = document.getElementById("updateStudentName");
const deleteStudentName = document.getElementById("deleteStudentName");

function openUpdateModal(name) {
    updateStudentName.textContent = `Update: ${name}`;
    updateModal.classList.remove("hidden");
    updateModal.dataset.updateName = name;
}

function closeUpdateModal() {
    updateModal.classList.add("hidden");
    updateModal.dataset.updateName = "";
}

function openDeleteModal(name) {
    deleteStudentName.textContent = `Are you sure you want to delete ${name}?`;
    deleteModal.classList.remove("hidden");
    deleteModal.dataset.deleteName = name;
}

function closeDeleteModal() {
    deleteModal.classList.add("hidden");
    deleteModal.dataset.deleteName = "";
}
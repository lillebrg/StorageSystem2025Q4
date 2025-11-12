const editBtn = document.getElementById("editBtn");
const changePasswordBtn = document.getElementById("changePasswordBtn");
var editUserModal = document.getElementById("editUserModal");
var changePasswordModal = document.getElementById("changePasswordModal");

editBtn.onclick = () => {
    // document.getElementById("uname").value = username;
    // document.getElementById("email").value = email;
    submitBtn.disabled = true;
    editUserModal.style.display = "block";
};
changePasswordBtn.onclick = () => (changePasswordModal.style.display = "block");

// Close modals when clicking on any close button
document.querySelectorAll(".modalClose").forEach((closeBtn) => {
    closeBtn.onclick = () => {
        editUserModal.style.display = "none";
        changePasswordModal.style.display = "none";
        document.getElementById("form-error").innerText = "";
        document.getElementById("form-error").style.display = "none";
    };
});

// Close modals when clicking outside
window.onclick = (event) => {
    if (event.target == editUserModal || event.target == changePasswordModal) {
        editUserModal.style.display = "none";
        changePasswordModal.style.display = "none";
        document.getElementById("form-error").innerText = "";
        document.getElementById("form-error").style.display = "none";
    }
};
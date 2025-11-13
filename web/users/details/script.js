//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
const changePasswordBtn = document.getElementById("changePasswordBtn");
const submitPasswordBtn = document.getElementById("submitPasswordBtn");
//edit User
const editUserModal = document.getElementById("editUserModal");
const submitUserBtn = document.getElementById("submitUserBtn");
const editBtn = document.getElementById("editBtn");


const emailInput = document.getElementById("email");
const nameInput = document.getElementById("name");

const checkForChanges = () => {
    if (emailInput.value !== email || nameInput.value !== name) {
        submitUserBtn.disabled = false; // Enable button if changes were made
    } else {
        submitUserBtn.disabled = true; // Disable button if no changes
    }
};

emailInput.addEventListener("input", checkForChanges);
nameInput.addEventListener("input", checkForChanges);

var name;
var email;

//open modals
editBtn.onclick = () => {
    submitUserBtn.disabled = true;
    editUserModal.style.display = "block";
};
changePasswordBtn.onclick = () => (changePasswordModal.style.display = "block");

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
    closeBtn.onclick = () => {
        editUserModal.style.display = "none";
        changePasswordModal.style.display = "none";
    };
});

// Close modals when clicking outside
window.onclick = (event) => {
    if (event.target == editUserModal || event.target == changePasswordModal) {
        editUserModal.style.display = "none";
        changePasswordModal.style.display = "none";
    }
};



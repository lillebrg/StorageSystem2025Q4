import { get, logout } from "../../services/user.service.js";

var name;
var email;

await get().then((data) => {
    if(!data){
        //displayError()
    }
    name = data.name;
    email = data.email;
 displayProfile(data);

});

function displayProfile(data){
    console.log(data);

      var table = document.getElementById(`profileCard`);
  table.innerHTML += `
       <div class="card-border-top"></div>
          <div class="img">
            <img src="/assets/images/telia.png" width="70px" height="70px" />
          </div>
          <span>${data.name}</span>
          <p class="job">${data.email}</p>
          <p class="job">${data.role}</p>

          <button class="button" id="editBtn">Edit</button>
          <button class="button" id="changePasswordBtn">Change Password</button>
          <button class="button" style="margin-top: 10px; background-color: darkred;" id="logoutBtn">Logout</button>`;
}

//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
const changePasswordBtn = document.getElementById("changePasswordBtn");
document.getElementById("logoutBtn").onclick = () => {
  logout();
};
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

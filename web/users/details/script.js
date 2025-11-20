import { get, update, deleteUser, updatePassword } from "../../services/user.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
//displayUserDetails
var name;
var email;
var role;

await get(id).then((data) => {
  if (!data) {
    //displayError()
  }
  name = data.name;
  email = data.email;
  role = data.role;
  displayProfile(data);
  displayTable(data.borrowed_items);
});

function displayProfile(data) {
  var table = document.getElementById(`profileCard`);
  table.innerHTML += `
       <div class="card-border-top"></div>
          <div class="img">
            <img src="/assets/images/telia.png" width="70px" height="70px" />
          </div>
          <span>${name}</span>
          <p class="job">${email}</p>
          <p class="job">${role}</p>

          <button class="button" id="editBtn">Edit</button>
          <button class="button" id="changePasswordBtn">Reset Password</button>
          <button class="button" style="margin-top: 10px; background-color: darkred;" id="deleteUserBtn">Delete user</button>`;
}

function displayTable(data) {
  //todo, specific items shown
  var table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  for (let i = 0; i < data.length; i++) {
    table.innerHTML += `
         <tr data-id="${data[i].id}">
            <td>${data[i].name}</td>
            <td>${data[i].email}</td>
            <td>${data[i].role}</td>
            <td>${data[i].borrowed_items}</td>
          </tr>`;
  }
}

//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
document.getElementById("changePasswordBtn").onclick = () => (changePasswordModal.style.display = "block");
document.getElementById("submitPasswordBtn").onclick = () => {
  var newPasswordInput = document.getElementById("newPassword").value;
  updatePassword(id, null, newPasswordInput)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
};

//edit User
const editUserModal = document.getElementById("editUserModal");
var emailInput = document.getElementById("email");
var nameInput = document.getElementById("name");
var roleInput = document.getElementById("role");
var cponlInput = document.getElementById("cponl");
document.getElementById("editBtn").onclick = () => {
  nameInput.value = name;
  emailInput.value  = email;
  roleInput.value  = role;
  editUserModal.style.display = "block";
};
document.getElementById("submitUserBtn").onclick = () => {
  if (!emailInput.checkValidity()) {
    alert("Please enter a valid email address.");
    return;
  }

  if (!emailInput || !nameInput) {
    alert("Please fill in both fields.");
    return;
  }

  update(id, emailInput.value, nameInput.value, roleInput.value, cponlInput.checked)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
};

//delete User
const deleteUserModal = document.getElementById("deleteUserModal");
document.getElementById("deleteUserBtn").onclick = () =>
  (deleteUserModal.style.display = "block");
document.getElementById("approveDeleteUserBtn").onclick = () => {
  deleteUser(id)
    .then(() =>   window.location.href = "/users")
    .catch((error) => {
      console.log(error);
    });
};

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    editUserModal.style.display = "none";
    changePasswordModal.style.display = "none";
    deleteUserModal.style.display = "none";
  };
});

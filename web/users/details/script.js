import {
  get,
  update,
  deleteUser,
  updatePassword,
} from "../../services/user.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
//displayUserDetails
var name;
var email;
var role;

  var getUserError = document.getElementById("getUserError");
  getUserError.style.display = "none";

  var noItemsBorrowed = document.getElementById("noItemsBorrowed");
    noItemsBorrowed.style.display = "none";

await get(id)
  .then((data) => {
    name = data.name;
    email = data.email;
    role = data.role;
    displayProfile();
    displayTable(data.borrowed_items);
  })
  .catch((error) => {
    getUserError.style.display = "block";
    getUserError.innerText = error;
  });

function displayProfile() {
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
  if(data.length <= 0) {
    noItemsBorrowed.style.display = "block"
  }

    data.forEach(user => {
    table.innerHTML += `
         <tr data-id="${user.id}">
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>${user.role}</td>
            <td>${user.borrowed_items}</td>
          </tr>`;
  })
}
//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
document.getElementById("changePasswordBtn").onclick = () =>
  (changePasswordModal.style.display = "block");
var changePasswordError = document.getElementById("changePasswordError");
changePasswordError.style.display = "none";

const changePasswordForm = document.getElementById("changePasswordForm");
changePasswordForm.addEventListener("submit", handleChangePassword);

async function handleChangePassword(event) {
  event.preventDefault();
  if (!changePasswordForm.reportValidity()) {
    return;
  }
  var newPasswordInput = document.getElementById("newPassword").value;
  updatePassword(id, null, newPasswordInput)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
}

//edit User
const editUserModal = document.getElementById("editUserModal");
var emailInput = document.getElementById("email");
var nameInput = document.getElementById("name");
var roleInput = document.getElementById("role");
var cponlInput = document.getElementById("cponl");
document.getElementById("editBtn").onclick = () => {
  nameInput.value = name;
  emailInput.value = email;
  roleInput.value = role;
  editUserModal.style.display = "block";
};

const userEditForm = document.getElementById("userEditForm");
userEditForm.addEventListener("submit", handleUserEdit);

async function handleUserEdit(event) {
  event.preventDefault();
  if (!userEditForm.reportValidity()) {
    return;
  }

  update(
    id,
    emailInput.value,
    nameInput.value,
    roleInput.value,
    cponlInput.checked
  )
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
}

//delete User
const userDeleteForm = document.getElementById("userDeleteForm");
userDeleteForm.addEventListener("submit", handleUserDelete);

const deleteUserModal = document.getElementById("deleteUserModal");
document.getElementById("deleteUserBtn").onclick = () =>
  (deleteUserModal.style.display = "block");
async function handleUserDelete(event) {
  event.preventDefault();
  if (!userDeleteForm.reportValidity()) {
    return;
  }
  deleteUser(id)
    .then(() => (window.location.href = "/users"))
    .catch((error) => {
      console.log(error);
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    editUserModal.style.display = "none";
    changePasswordModal.style.display = "none";
    deleteUserModal.style.display = "none";
  };
});

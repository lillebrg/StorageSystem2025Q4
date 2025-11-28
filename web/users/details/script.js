import {
  get,
  update,
  deleteUser,
  updatePassword,
} from "../../services/pages/user.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
//displayUserDetails
let name;
let email;
let role;

  let getError = document.getElementById("getError");
  getError.style.display = "none";

await get(id)
  .then((data) => {
    name = data.name;
    email = data.email;
    role = data.role;
    displayProfile();
    displayTable(data.borrowed_items);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red"
    getError.innerText = error;
  });

function displayProfile() {
  let table = document.getElementById(`profileCard`);
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
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if(data.length <= 0) {
    getError.style.display = "block"
    getError.innerHTML = "No items borrowed"
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
let changePasswordError = document.getElementById("changePasswordError");
changePasswordError.style.display = "none";

const changePasswordForm = document.getElementById("changePasswordForm");
changePasswordForm.addEventListener("submit", handleChangePassword);

async function handleChangePassword(event) {
  event.preventDefault();
  if (!changePasswordForm.reportValidity()) {
    return;
  }
  let newPasswordInput = document.getElementById("newPassword").value;
  updatePassword(id, null, newPasswordInput)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
}

//edit User
const editUserModal = document.getElementById("editUserModal");
let emailInput = document.getElementById("email");
let nameInput = document.getElementById("name");
let roleInput = document.getElementById("role");
let cponlInput = document.getElementById("cponl");
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

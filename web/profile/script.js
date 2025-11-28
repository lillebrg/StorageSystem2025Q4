import { get, update, logout, updatePassword } from "../../services/user.service.js";
//displayUserDetails
let name;
let email;

await get().then((data) => {
  if (!data) {
    //displayError()
  }
  name = data.name;
  email = data.email;
  displayProfile(data);
  displayTable(data.borrowed_items);
});

function displayProfile(data) {
  let table = document.getElementById(`profileCard`);
  table.innerHTML += `
       <div class="card-border-top"></div>
          <div class="img">
            <img src="/assets/images/telia.png" width="70px" height="70px" />
          </div>
          <span>${name}</span>
          <p class="job">${email}</p>
          <p class="job">${data.role}</p>

          <button class="button" id="editBtn">Edit</button>
          <button class="button" id="changePasswordBtn">change Password</button>
          <button class="button" style="margin-top: 10px; background-color: darkred;" id="logoutBtn">logout</button>`;
}

function displayTable(data) {
  //todo, specific items shown
  let table = document.getElementById("tBody");
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

const changePasswordForm = document.getElementById("changePasswordForm");
changePasswordForm.addEventListener("submit", handleChangePassword);

async function handleChangePassword(event) {
 event.preventDefault()
  if (!changePasswordForm.reportValidity()) {
    return;
  }

  let oldPasswordInput = document.getElementById("oldPassword").value;
  let newPasswordInput = document.getElementById("newPassword").value;
  let repeatNewPasswordInput = document.getElementById("repeatNewPassword").value;

  if (newPasswordInput != repeatNewPasswordInput) {
    alert("New passwords do not match");
    return;
  }

  updatePassword(null, oldPasswordInput, newPasswordInput)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
};

//edit User
const editUserModal = document.getElementById("editUserModal");
let emailInput = document.getElementById("email");
let nameInput = document.getElementById("name");
document.getElementById("editBtn").onclick = () => {
  nameInput.value = name;
  emailInput.value  = email;
  editUserModal.style.display = "block";
};

const userEditForm = document.getElementById("userEditForm");
userEditForm.addEventListener("submit", handleUserEdit);

async function handleUserEdit(event){
  event.preventDefault()
  if (!userEditForm.reportValidity()) {
    return;
  }

  update(null, emailInput.value, nameInput.value, null, null)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
};

//logout
document.getElementById("logoutBtn").onclick = () =>(logout());

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    editUserModal.style.display = "none";
    changePasswordModal.style.display = "none";
  };
});

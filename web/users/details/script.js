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
  let table = document.getElementById("tBody");
  table.innerHTML = "";

  if (data.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "No items borrowed";
    return;
  } else {
    getError.style.display = "none";
  }

  data.forEach(item => {
    table.innerHTML += `
      <tr data-id="${item.specific_item_id}"
          data-name="${item.base_item_name}"
          data-description="${item.specific_item_description}"
          data-image="${item.base_item_picture}">
        <td>${item.specific_item_id}</td>
        <td>${item.base_item_name}</td>
        <td><img src="${item.base_item_picture}" style="max-height: 80px; max-width: 80px;"/></td>
        <td>${item.specific_item_description}</td>
      </tr>`;
  });

  table.querySelectorAll("tr").forEach((row) => {
    row.addEventListener("click", () => {
      specificItemId = row.dataset.id;
      document.getElementById("borrowTitle").innerHTML =
        `Do you want to return item "${row.dataset.name}"?`;
      document.getElementById("reviewImg").src = row.dataset.image;

      returnModal.style.display = "block";
    });
  });
}

//return item
let specificItemId;
const returnError = document.getElementById("returnError");
returnError.style.display = "none";

const returnModal = document.getElementById("returnModal");
const returnForm = document.getElementById("returnForm");
returnForm.addEventListener("submit", submitReturn);

async function submitReturn(event) {
  event.preventDefault();
  if (!returnForm.reportValidity()) {
    return;
  }

  returnItem(specificItemId)
    .then(() => window.location.reload())
    .catch((error) => {
      returnError.style.display = "block";
      returnError.innerText = error;
    });
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
      changePasswordError.style.display = "block";
      changePasswordError.innerText = error;
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

let editError = document.getElementById("editError");
editError.style.display = "none";

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
      editError.style.display = "block";
      editError.innerText = error;    });
}

//delete User
const userDeleteForm = document.getElementById("userDeleteForm");
userDeleteForm.addEventListener("submit", handleUserDelete);
let deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";
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
      deleteError.style.display = "block";
      deleteError.innerText = error;    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    editUserModal.style.display = "none";
    changePasswordModal.style.display = "none";
    deleteUserModal.style.display = "none";
    returnModal.style.display = "none";
  };
});

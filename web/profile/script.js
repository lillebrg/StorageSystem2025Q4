import { get, update, updatePassword } from "../../services/pages/user.service.js";
import { logout } from "../services/auth.js";
import { returnItem } from "../services/pages/borrowrequest.service.js";
//displayUserDetails
let name;
let email;

  let getError = document.getElementById("getError");
  getError.style.display = "none";

await get().then((data) => {
  name = data.name;
  email = data.email;
  displayProfile(data);
  displayTable(data.borrowed_items);
})
.catch((error) => {
  console.log(error)
  getError.style.display = "block";
  getError.color = "red"
  getError.innerText = error;
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
          <button class="button" id="changePasswordBtn">Change Password</button>
          <button class="button" style="margin-top: 10px; background-color: darkred;" id="logoutBtn">logout</button>`;
}

function displayTable(data) {
  console.log(data)
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if(data.length <= 0) {
    getError.style.display = "block"
    getError.innerHTML = "No items borrowed"
  }
  data.forEach(data => {
        table.innerHTML += `
      <tr data-id="${data.specific_item_id}" data-name="${data.base_item_name}" data-description="${data.specific_item_description}" data-image="${data.base_item_picture}">
            <td>${data.specific_item_id}</td>
            <td>${data.base_item_name}</td>
            <td><img src="${data.base_item_picture}" style="max-height: 80px; max-width: 80px;"/></td>
            <td>${data.specific_item_description}</td>
          </tr>`;
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

document.querySelectorAll("#tBody tr").forEach((row) => {
  row.addEventListener("click", () => {
    specificItemId = row.dataset.id;
    document.getElementById(
      "borrowTitle"
    ).innerHTML = `Do you want to return item "${row.dataset.name}"?`;
    document.getElementById("reviewImg").src = row.dataset.image;

    returnModal.style.display = "block";
  });
});


//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
document.getElementById("changePasswordBtn").onclick = () => (changePasswordModal.style.display = "block");

const changePasswordError = document.getElementById("changePasswordError");
changePasswordError.style.display = "none";

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
      changePasswordError.style.display = "block";
      changePasswordError.innerText = error;
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
    returnModal.style.display = "none";
  };
});

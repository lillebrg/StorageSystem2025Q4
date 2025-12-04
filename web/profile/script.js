import { get, update, updatePassword } from "../../services/pages/user.service.js";
import { logout } from "../services/auth.js";
import { returnItem } from "../services/pages/borrowrequest.service.js";
import { getBorrowRequests } from "../services/pages/user.service.js";
import { rejectRequest } from "../services/pages/borrowrequest.service.js";
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
  getError.style.display = "block";
  getError.color = "red"
  getError.innerText = error;
});

  let getBorrowRequestsError = document.getElementById("getBorrowRequestsError");
  getBorrowRequestsError.style.display = "none";

await getBorrowRequests().then((data) => {
  displayBorrowRequestTable(data);
})
.catch((error) => {
  getBorrowRequestsError.style.display = "block";
  getBorrowRequestsError.color = "red"
  getBorrowRequestsError.innerText = error;
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

function displayBorrowRequestTable(data) {
  let table = document.getElementById("tBodyBorrowRequests");
  table.innerHTML = ""; // clear old table
  data.forEach((request) => {
    if(request.accepted == true){
      return
    }
    table.innerHTML += `
         <tr data-id="${request.id}" data-name="${request.base_item.name}" data-image="${
      request.base_item.image_url
    }">
            <td>${request.base_item.name}</td>
            <td><img src="${request.base_item.image_url || "/assets/images/placeholder.png"}" style="max-height: 80px; max-width: 80px;"/></td>
            <td>${request.specific_item.description}</td>
            <td>Not handled</td>
          </tr>`;
  });

   table.querySelectorAll("tr").forEach((row) => {
    row.addEventListener("click", () => {
      borrowRequestId = row.dataset.id;
      document.getElementById("cancelRequestTitle").innerHTML =
        `Do you want cancel your borrow request for item "${row.dataset.name}"?`;
      document.getElementById("cancelRequestreviewImg").src = row.dataset.image;

      cancelRequestModal.style.display = "block";
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

  returnItem(specificItemId)
    .then(() => window.location.reload())
    .catch((error) => {
      returnError.style.display = "block";
      returnError.innerText = error;
    });
}

//cancel Request
let borrowRequestId;
const cancelRequestError = document.getElementById("cancelRequestError");
cancelRequestError.style.display = "none";

const cancelRequestModal = document.getElementById("cancelRequestModal");
const cancelRequestForm = document.getElementById("cancelRequestForm");
cancelRequestForm.addEventListener("submit", submitCancel);

async function submitCancel(event) {
  event.preventDefault();

  rejectRequest(borrowRequestId)
    .then(() => window.location.reload())
    .catch((error) => {
      cancelRequestError.style.display = "block";
      cancelRequestError.innerText = error;
    });
}


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
const editError = document.getElementById("editError");
editError.style.display = "none";
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
      editError.style.display = "block";
      editError.innerText = error;
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
    cancelRequestModal.style.display = "none";
  };
});

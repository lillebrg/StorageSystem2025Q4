import { get, logout, deleteUser } from "../../services/user.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
//displayUserDetails
var name;
var email;

await get(id).then((data) => {
  if (!data) {
    //displayError()
  }
  name = data.name;
  email = data.email;
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
          <p class="job">${data.role}</p>

          <button class="button" id="editBtn">Edit</button>
          <button class="button" id="changePasswordBtn">Change Password</button>
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
  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/users/details/?id=" + id;
    });
  });
}

//change Password
const changePasswordModal = document.getElementById("changePasswordModal");
const changePasswordBtn = document.getElementById("changePasswordBtn");
changePasswordBtn.onclick = () => (changePasswordModal.style.display = "block");

//edit User
const editUserModal = document.getElementById("editUserModal");

const emailInput = document.getElementById("email");
const nameInput = document.getElementById("name");
document.getElementById("editBtn").onclick = () => {
  editUserModal.style.display = "block";
};
document.getElementById("submitUserBtn").onclick = () => {
  deleteUser();
  //todo, check on deletion
};

//delete User
const deleteUserModal = document.getElementById("deleteUserModal");
document.getElementById("deleteUserBtn").onclick = () =>
  (deleteUserModal.style.display = "block");
document.getElementById("approveDeleteUserBtn").onclick = () => {
  deleteUser(id)
    .then(() => location.reload())
    .catch((error) => {
      console.log(error)
    });

  //todo, check on deletion
};

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    editUserModal.style.display = "none";
    changePasswordModal.style.display = "none";
    deleteUserModal.style.display = "none";
  };
});

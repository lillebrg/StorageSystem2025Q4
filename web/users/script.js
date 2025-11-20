import { create, getAll } from "../services/user.service.js";

await getAll()
  .then((data) => displayTable(data))
  .catch((error) => {
    console.log(error);
  });

function displayTable(data) {
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

//create User
const createUserModal = document.getElementById("createUserModal");
var nameInput = document.getElementById("name");
var emailInput = document.getElementById("email");
var passwordInput = document.getElementById("password");
var roleInput = document.getElementById("role");

document.getElementById("createUserBtn").onclick = () => {
  createUserModal.style.display = "block";
  nameInput.value = "";
  emailInput.value = "";
  passwordInput.value = "";
  roleInput.value = "";
};

document.getElementById("closeCreateUserBtn").onclick = () => {
  createUserModal.style.display = "none";
};

const createUserForm = document.querySelector(".form");
createUserForm.addEventListener("submit", handleCreateUser);

async function handleCreateUser(event) {
    event.preventDefault();
  if (!createUserForm.reportValidity()) {
    return;
  }

  const name = nameInput.value;
  const email = emailInput.value;
  const password = passwordInput.value;
  const role = roleInput.value;

 await create(name, email, password, role)
    .then(() => location.reload())
    .catch((error) => {
      console.log(error);
    });
}

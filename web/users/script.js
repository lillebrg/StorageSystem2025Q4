import { create, getAll } from "../services/user.service.js";

await getAll()
  .then((data) => displayTable(data))
  .catch((error) => {
    console.log(error);
  });

function displayTable(data) {
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table

  data.forEach(user => {
    table.innerHTML += `
         <tr data-id="${user.id}">
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>${user.role}</td>
            <td>${user.borrowed_items}</td>
          </tr>`;
  })

  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/users/details/?id=" + id;
    });
  });
}

//create User
const createUserModal = document.getElementById("createUserModal");
let nameInput = document.getElementById("name");
let emailInput = document.getElementById("email");
let passwordInput = document.getElementById("password");
let roleInput = document.getElementById("role");
let createUserError = document.getElementById("createUserError");
createUserError.style.display = "none";

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

const createUserForm = document.getElementById("createUserForm");
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
      createUserError.style.display = "block";
      createUserError.innerText = error;
    });
}

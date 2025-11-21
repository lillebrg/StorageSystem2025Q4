import { create, getAll } from "../services/items.service.js";

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

const form = document.querySelector(".form");
form.addEventListener("submit", handleCreateUser);

async function handleCreateUser(event) {
  event.preventDefault();
  const name = nameInput.value;
  const email = emailInput.value;
  const password = passwordInput.value;
  const role = roleInput.value;

 if (!emailInput.checkValidity()) {
    alert("Please enter a valid email address.");
    return;
  }


  // validate all fields
  if (!name || !email || !password || !role) {
    alert("Please fill in all fields.");
    return;
  }

  var response = await create(name, email, password, role)
    .then(() => location.reload())
    .catch((error) => {
      console.log(error);
    });
}

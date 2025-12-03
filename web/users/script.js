import { create, getAll } from "../services/pages/user.service.js";

let getError = document.getElementById("getError");
getError.style.display = "none";

await getAll()
  .then((data) => displayTable(data))
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });

function displayTable(data) {
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if (data.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "No users in the system";
    return;
  } else {
    getError.style.display = "none";
  }
  data.forEach((user) => {
    table.innerHTML += `
         <tr data-id="${user.id}">
            <td>${user.name}</td>
            <td>${user.email}</td>
            <td>${user.role}</td>
            <td>${user.borrowed_items}</td>
          </tr>`;
  });

  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/users/details/?id=" + id;
    });
  });
}

//create User
const createModal = document.getElementById("createModal");
let nameInput = document.getElementById("name");
let emailInput = document.getElementById("email");
let passwordInput = document.getElementById("password");
let roleInput = document.getElementById("role");
const createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  createModal.style.display = "block";
  nameInput.value = "";
  emailInput.value = "";
  passwordInput.value = "";
  roleInput.value = "";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", handleCreate);

async function handleCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }

  const name = nameInput.value;
  const email = emailInput.value;
  const password = passwordInput.value;
  const role = roleInput.value;

  await create(name, email, password, role)
    .then(() => location.reload())
    .catch((error) => {
      createError.style.display = "block";
      createError.innerText = error;
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    createModal.style.display = "none";
  };
});

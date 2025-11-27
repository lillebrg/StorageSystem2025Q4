import { create, getAll } from "../services/storage.service.js";

var getError = document.getElementById("getError");
getError.style.display = "none";

await getAll()
  .then((data) => displayTable(data))
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });

function displayTable(data) {
  var table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
    if (data.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "No storages";
  }

    data.forEach(item => {
    table.innerHTML += `
         <tr data-id="${item.id}">
            <td>${item.name}</td>
          </tr>`;
  })
  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/storages/details/?id=" + id;
    });
  });
}

//create Storage
var createModal = document.getElementById("createModal");
var nameInput = document.getElementById("name");
var createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  createModal.style.display = "block";
  nameInput.value = "";
};

document.getElementById("closeCreateModalBtn").onclick = () => {
  createModal.style.display = "none";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", submitCreate);

async function submitCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }

 await create(nameInput.value)
    .then(() => location.reload())
    .catch((error) => {
      createError.style.display = "block";
      createError.innerText = error;
    });
}

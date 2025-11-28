import { deleteStorage, get } from "../../services/storage.service.js";
import { create } from "../../services/racks.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

let getError = document.getElementById("getError");
getError.style.display = "none";

await get(id)
  .then((data) => {
    document.getElementById("tableTitle").innerHTML = "Storage: " + data.name;
    displayTable(data.racks);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });

function displayTable(data) {
  const table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table

  if (data.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "Storage is empty";
    return;
  }

  // Build all rows
  data.forEach(item => {
    table.innerHTML += `
      <tr data-id="${item.id}">
        <td>${item.rack_no}</td>
      </tr>`;
  });
  
  document.querySelectorAll("#tBody tr").forEach(row => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/racks/?id=" + id;
    });
  });
}


//Create Rack
let createModal = document.getElementById("createModal");
let rack_noInput = document.getElementById("rack_no");
let createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  rack_noInput.value = "";
  createModal.style.display = "block";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", submitCreate);

async function submitCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }

  create(id, rack_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      createError.style.display = "block";
      createError.innerText = error;
    });
}

//delete Storage
let deleteModal = document.getElementById("deleteModal");
let deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";

document.getElementById("deleteBtn").onclick = () => {
  deleteModal.style.display = "block";
};

document.getElementById("deleteForm").addEventListener("submit", submitDelete);

async function submitDelete(event) {
  event.preventDefault();
  deleteStorage(id)
    .then(() =>  window.location.href = "/storages")
    .catch((error) => {
      deleteError.style.display = "block";
      deleteError.innerText = error;
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    createModal.style.display = "none";
    deleteModal.style.display = "none";
  };
});

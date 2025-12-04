import { deleteStorage, get, update } from "../../services/pages/storage.service.js";
import { create } from "../../services/pages/racks.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

let getError = document.getElementById("getError");
getError.style.display = "none";

let name;

await get(id)
  .then((data) => {
    name = data.name;
    document.getElementById("tableTitle").innerHTML = "Storage: " + data.name;
    displayTable(data);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });

function displayTable(data) {
  const table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table

  if (data.racks.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "Storage is empty";
    return;
  }

  // Build all rows
  data.racks.forEach(item => {
    table.innerHTML += `
      <tr data-id="${item.id}">
        <td>${item.rack_no}</td>
      </tr>`;
  });
  
  document.querySelectorAll("#tBody tr").forEach(row => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = `/racks/?id=${id}&storageId=${data.id}`;
    });
  });
}


//Create Rack
const createModal = document.getElementById("createModal");
let rack_noInput = document.getElementById("rack_no");
const createError = document.getElementById("createError");
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

//update storage
let updateModal = document.getElementById("updateModal");
let nameInput = document.getElementById("name");

let updateError = document.getElementById("updateError");
updateError.style.display = "none";

document.getElementById("updateBtn").onclick = () => {
  nameInput.value = name;
  updateModal.style.display = "block";
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", submitUpdate);

async function submitUpdate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

  update(id, nameInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      updateError.style.display = "block";
      updateError.innerText = error;
    });
}

//delete Storage
const deleteModal = document.getElementById("deleteModal");
const deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";

document.getElementById("deleteBtn").onclick = () => {
  deleteModal.style.display = "block";
};

document.getElementById("deleteForm").addEventListener("submit", submitDelete);

async function submitDelete(event) {
  event.preventDefault();
  deleteStorage(id)
    .then(() =>  goBack())
    .catch((error) => {
      deleteError.style.display = "block";
      deleteError.innerText = error;
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    createModal.style.display = "none";
    updateModal.style.display = "none";
    deleteModal.style.display = "none";
  };
});

//go back btn
document.getElementById("backBtn").onclick = () => {goBack()};

function goBack() {
  window.location.href = "/storages"
}

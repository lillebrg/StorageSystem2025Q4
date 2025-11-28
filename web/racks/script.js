import { deleteRack, get, update } from "../services/pages/racks.service.js";
import { create } from "../services/pages/shelves.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const storageId = params.get("storageId");

let getError = document.getElementById("getError");
getError.style.display = "none";

let rack_no;

await get(id)
  .then((data) => {
    console.log(data);
    rack_no = data.rack_no;
    document.getElementById("tableTitle").innerHTML = "Rack: " + data.rack_no;
    displayTable(data);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });

function displayTable(data) {
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if (data.shelves.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "Shelf is empty";
  }

  data.shelves.forEach((item) => {
    table.innerHTML += `
         <tr data-id="${item.id}">
            <td>${item.shelf_no}</td>
            <td>${item.barcode}</td>
          </tr>`;
  });

  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const shelvesId = row.dataset.id;
      window.location.href = `/shelves/?id=${shelvesId}&rackId=${id}&storageId=${storageId}`;
    });
  });
}
//Create shelf
let createModal = document.getElementById("createModal");
let shelf_noInput = document.getElementById("shelf_no");

let createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  shelf_noInput.value = "";
  createModal.style.display = "block";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", submitCreate);

async function submitCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }

  create(id, shelf_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      createError.style.display = "block";
      createError.innerText = error;
    });
}

//update Rack
let updateModal = document.getElementById("updateModal");
let rack_noInput = document.getElementById("rack_no");
let updateError = document.getElementById("updateError");
updateError.style.display = "none";

document.getElementById("updateBtn").onclick = () => {
  rack_noInput.value = rack_no;
  updateModal.style.display = "block";
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", submitUpdate);

async function submitUpdate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

  update(id, rack_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      updateError.style.display = "block";
      updateError.innerText = error;
    });
}

//delete rack
let deleteModal = document.getElementById("deleteModal");
document.getElementById("deleteBtn").onclick = () =>
  (deleteModal.style.display = "block");

let deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";

const deleteForm = document.getElementById("deleteForm");
deleteForm.addEventListener("submit", submitDelete);

async function submitDelete(event) {
  event.preventDefault();
  if (!deleteForm.reportValidity()) {
    return;
  }
  deleteRack(id)
    .then(() => goBack())
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
//go back btn
document.getElementById("backBtn").onclick = () => {goBack();};

function goBack() {
  window.location.href = "/storages/details/?id=" + storageId;
}

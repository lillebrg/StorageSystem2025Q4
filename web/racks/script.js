import { deleteRack, get } from "../services/racks.service.js"
import { create } from "../services/shelves.service.js"
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const storageId = params.get("storageId");

let getError = document.getElementById("getError");
getError.style.display = "none";

await get(id)
  .then((data) => {
    console.log(data)
    document.getElementById("tableTitle").innerHTML = "rack: "+data.rack_no;
    displayTable(data.shelves);
  })
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
    getError.innerHTML = "Shelf is empty";
  }

    data.forEach(item => {
    table.innerHTML += `
         <tr data-id="${item.id}">
            <td>${item.shelf_no}</td>
            <td>${item.barcode}</td>
          </tr>`;
  })

  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/shelves/?id=" + id;
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

//delete rack
let deleteModal = document.getElementById("deleteModal");
document.getElementById("deleteBtn").onclick = () => (deleteModal.style.display = "block");

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
    .then(() => window.location.href = "/storages/details/?id=" + storageId)
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

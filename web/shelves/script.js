import { get, create } from "../services/shelves.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

var getStorageDetailsError = document.getElementById("getStorageDetailsError");
getStorageDetailsError.style.display = "none";

var noRacks = document.getElementById("noRacks");
noRacks.style.display = "none";

await get(id)
  .then((data) => {
    console.log(data);
    document.getElementById("tableTitle").innerHTML = "Shelf: "+data.shelf_no;
    displayTable(data.base_items);
  })
  .catch((error) => {
    getStorageDetailsError.style.display = "block";
    getStorageDetailsError.innerText = error;
  });

function displayTable(data) {
  var table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if (data.length <= 0) {
    noRacks.style.display = "block";
  }
  for (let i = 0; i < data.length; i++) {
    
    table.innerHTML += `
         <tr>
            <td>${data[i].shelf_no}</td>
            <td>${data[i].barcode}</td>
            <td>${data[i].barcode}</td>
            <td>${data[i].barcode}</td>
            <td>${data[i].barcode}</td>
          </tr>`;
  }
}
//Create User
var createRackModal = document.getElementById("createRackModal");
var rack_noInput = document.getElementById("rack_no");
document.getElementById("createRackBtn").onclick = () => {
  rack_noInput.value = "";
  createRackModal.style.display = "block";
};

const createRackForm = document.getElementById("createRackForm");
createRackForm.addEventListener("submit", handleUserEdit);

async function handleUserEdit(event) {
  event.preventDefault();
  if (!createRackForm.reportValidity()) {
    return;
  }

  create(id, rack_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      console.log(error);
    });
}

//delete Storage
const storageDeleteForm = document.getElementById("storageDeleteForm");
storageDeleteForm.addEventListener("submit", handleUserDelete);

const deleteStorageModal = document.getElementById("deleteStorageModal");
document.getElementById("deleteStorageBtn").onclick = () =>
  (deleteStorageModal.style.display = "block");
async function handleUserDelete(event) {
  event.preventDefault();
  if (!storageDeleteForm.reportValidity()) {
    return;
  }
  deleteStorage(id)
    .then(() => (window.location.href = "/storages"))
    .catch((error) => {
      console.log(error);
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    deleteStorageModal.style.display = "none";
  };
});

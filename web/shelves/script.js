import { get, update, deleteShelf } from "../services/pages/shelves.service.js";
import { create, uploadImage} from "../services/pages/baseitem.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");
const rackId = params.get("rackId");
const storageId = params.get("storageId");

let getError = document.getElementById("getError");
getError.style.display = "none";

let shelf_no;

await get(id)
  .then((data) => {
    shelf_no = data.shelf_no;
    document.getElementById("tableTitle").innerHTML = "Shelf: " + data.shelf_no;
    document.getElementById("shelfBarcode").innerHTML = data.barcode;

    displayTable(data.base_items);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerText = error;
  });

function displayTable(data) {
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  if(data.length <= 0){
    getError.style.display = "block";
    getError.innerHTML = "Rack is empty";
  }

  
  data.forEach((baseItem) => {
    table.innerHTML += `
         <tr data-id="${baseItem.id}">
            <td>${baseItem.name}</td>
            <td>${baseItem.description}</td>
            <td>${baseItem.barcode}</td>
            <td><img src="${baseItem.image_url}" style="width: 100px;"/></td>
            <td>${baseItem.specific_items_count}</td>
            <td>${baseItem.specific_items_available_count}</td>
          </tr>`;
  });

    document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/baseitems/details/?id=" + id;
    });
  });
}
//create baseItem
const createModal = document.getElementById("createModal");
let nameInput = document.getElementById("name");
let descriptionInput = document.getElementById("description");
let imageInput = document.getElementById("image");
let barcodeInput = document.getElementById("barcode");

const createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  createModal.style.display = "block";
  nameInput.value = "";
  descriptionInput.value = "";
  imageInput.value = "";
  barcodeInput.value = "";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", handleCreate);

async function handleCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }

  let savedFileName = null;

  try {
    if (imageInput.files.length > 0) {
      savedFileName = await uploadImage(imageInput.files[0]);
    }

    await create(
      nameInput.value,
      descriptionInput.value,
      barcodeInput.value,
      savedFileName.path,
      id
    );

    window.location.reload();
  } catch (error) {
    createError.style.display = "block";
    createError.innerText = error;
  }
}

//update shelf
let updateModal = document.getElementById("updateModal");
let shelf_noInput = document.getElementById("shelf_no");

let updateError = document.getElementById("updateError");
updateError.style.display = "none";

document.getElementById("updateBtn").onclick = () => {
  shelf_noInput.value = shelf_no;
  updateModal.style.display = "block";
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", submitUpdate);

async function submitUpdate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

  update(id, shelf_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      updateError.style.display = "block";
      updateError.innerText = error;
    });
}

//delete shelf
const deleteForm = document.getElementById("deleteForm");
deleteForm.addEventListener("submit", handleUserDelete);

const deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";

const deleteModal = document.getElementById("deleteModal");
document.getElementById("deleteBtn").onclick = () =>
  (deleteModal.style.display = "block");
async function handleUserDelete(event) {
  event.preventDefault();
  if (!deleteForm.reportValidity()) {
    return;
  }
  deleteShelf(id)
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
    updateModal.style.display = "none";
    deleteModal.style.display = "none";
  };
});

//go back btn
document.getElementById("backBtn").onclick = () => {goBack();};

function goBack() {
  window.location.href = `/racks/?id=${rackId}&storageId=${storageId}`
}
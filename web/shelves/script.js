import { get, update, deleteShelf } from "../services/shelves.service.js";
import { create, uploadImage} from "../services/baseitem.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

let getError = document.getElementById("getError");
getError.style.display = "none";

let shelf_no;

await get(id)
  .then((data) => {
    shelf_no = data.shelf_no;
    document.getElementById("tableTitle").innerHTML = "Shelf: " + data.shelf_no;
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
            <td><img src="${baseItem.image_url}" style="width: 100px;"/></td>
            <td>${baseItem.specific_items_count}</td>
            <td>${baseItem.specific_items_available_count}</td>
          </tr>`;
  });
}
//create baseItem
let createModal = document.getElementById("createModal");
let nameInput = document.getElementById("name");
let descriptionInput = document.getElementById("description");
let imageInput = document.getElementById("image");
let barcodeInput = document.getElementById("barcode");

let createError = document.getElementById("createError");
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

//Create shelf
let updateModal = document.getElementById("updateModal");
let shelf_noInput = document.getElementById("shelf_no");

let updateError = document.getElementById("updateError");
updateError.style.display = "none";

document.getElementById("updateBtn").onclick = () => {
  shelf_noInput.value = shelf_no;
  updateModal.style.display = "block";
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", submitCreate);

async function submitCreate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

  update(id, shelf_noInput.value)
    .then(() => window.location.reload())
    .catch((error) => {
      createError.style.display = "block";
      createError.innerText = error;
    });
}

//delete shelf
let deleteForm = document.getElementById("deleteForm");
deleteForm.addEventListener("submit", handleUserDelete);

let deleteModal = document.getElementById("deleteModal");
document.getElementById("deleteBtn").onclick = () =>
  (deleteModal.style.display = "block");
async function handleUserDelete(event) {
  event.preventDefault();
  if (!deleteForm.reportValidity()) {
    return;
  }
  deleteShelf(id)
    .then(() => (window.location.href = "/storages"))
    .catch((error) => {
      console.log(error);
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

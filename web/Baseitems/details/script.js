import { deleteBaseItem, get, update, uploadImage } from "../../services/pages/baseitem.service.js";
import { create } from "../../services/pages/specificitem.service.js";
const params = new URLSearchParams(window.location.search);
const id = params.get("id");

let getError = document.getElementById("getError");
getError.style.display = "none";

let name;
let description;
let barcode

await get(id)
  .then((data) => {
    console.log(data)
    name = data.name
    description = data.description;
    barcode = data.barcode
    document.getElementById("tableTitle").innerHTML = name;
    document.getElementById("baseItemImg").src = data.image_url;
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

  if (data.specific_items.length <= 0) {
    getError.style.display = "block";
    getError.innerHTML = "No Specific items";
    return;
  }

  // Build all rows
  data.specific_items.forEach(item => {
    table.innerHTML += `
      <tr data-id="${item.id}">
        <td>${item.id}</td>
        <td>${item.barcode}</td>
        <td>${item.description}</td>
        <td>${item.barcode}</td>
      </tr>`;
  });
  
  document.querySelectorAll("#tBody tr").forEach(row => {
    row.addEventListener("click", () => {
      const specificItemId = row.dataset.id;
      window.location.href = `/specificitems/?id=${specificItemId}&baseitemId=${id}`;
    });
  });
}


//Create specificItem
const createModal = document.getElementById("createModal");
let specificdescriptionInputInput = document.getElementById("specificdescriptionInput");
const createError = document.getElementById("createError");
createError.style.display = "none";

document.getElementById("createBtn").onclick = () => {
  specificdescriptionInput.value = "";
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

//update baseItem
const updateModal = document.getElementById("updateModal");
let nameInput = document.getElementById("name");
let descriptionInput = document.getElementById("description");
let imageInput = document.getElementById("image");
let barcodeInput = document.getElementById("barcode");
let shelfBarcodeInput = document.getElementById("shelfBarcode");

const updateError = document.getElementById("updateError");
updateError.style.display = "none";

document.getElementById("updateBtn").onclick = () => {
  updateModal.style.display = "block";
  nameInput.value = name;
  descriptionInput.value = description;
  barcodeInput.value = barcode;
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", handleCreate);

async function handleCreate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

let savedFileName;

try {
  if (imageInput.files.length > 0) {
    savedFileName = await uploadImage(imageInput.files[0]);
  }

  await update(
    id,
    nameInput.value,
    descriptionInput.value,
    savedFileName.value,
    savedFileName.path
  );

  window.location.reload();

} catch (error) {
  console.log(error)
  updateError.style.display = "block";
  updateError.innerText = error;
}

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
  deleteBaseItem(id)
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
  window.location.href = "/baseitems"
}

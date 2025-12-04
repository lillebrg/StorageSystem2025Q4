import {
  deleteBaseItem,
  get,
  update,
  uploadImage,
  scanBarcode,
} from "../../services/pages/baseitem.service.js";
import { create, deleteSpecificItem } from "../../services/pages/specificitem.service.js";
import { createBorrowRequest } from "../../services/pages/borrowrequest.service.js";

//role
const createBtn = document.getElementById("createBtn");
const updateBtn = document.getElementById("updateBtn");
const deleteBtn = document.getElementById("deleteBtn");
if (localStorage.getItem("role") == "User") {
  createBtn.style.display = "none";
  updateBtn.style.display = "none";
  deleteBtn.style.display = "none";
}

const params = new URLSearchParams(window.location.search);
const id = params.get("id");

let getError = document.getElementById("getError");
getError.style.display = "none";

let name;
let description;
let barcode;
let shelfBarcode;

await get(id)
  .then((data) => {
    name = data.name;
    description = data.description;
    barcode = data.barcode;
    shelfBarcode = data.shelf_barcode;
    document.getElementById("tableTitle").innerHTML = name;
    document.getElementById("baseItemImg").src = data.image_url || "/assets/images/placeholder.png";
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
  data.specific_items.forEach((item) => {
    const row = document.createElement("tr");
    row.dataset.id = item.id
    row.dataset.description = item.description;

    row.innerHTML = `
      <td>${item.id}</td>
      <td>${item.barcode}</td>
      <td style="text-align: left">${item.description ?? ""}</td>
      <td>${item.loaned_to == null ? "" : item.loaned_to.name}</td>
      <td>
        <button class="button borrow-btn">
          <i class="fa fa-handshake"></i>
        </button>
        <button class="button delete-btn" style="display: none">
          <i class="fa fa-trash"></i>
        </button>
      </td>
    `;

    const role = localStorage.getItem("role");
    if (["Operator", "Admin"].includes(role)) {
      row.querySelector(".delete-btn").style.display = "inline-block";
      row.querySelector(".delete-btn").onclick = async () => {
        specificItemId = item.id;
        deleteSpecificModal.style.display = "block"
      };
    }

    row.querySelector(".borrow-btn").onclick = () => {
      specificItemId = item.id;
      document.getElementById("borrowTitle").innerHTML = `Do you want to send a borrow request for item "${name}"?`;
      document.getElementById("borrowDescription").innerHTML = row.dataset.description;
      borrowModal.style.display = "block";
    };

    table.appendChild(row);
  });
}

//request Borrow
let specificItemId;
const borrowError = document.getElementById("borrowError");
borrowError.style.display = "none";

const borrowModal = document.getElementById("borrowModal");
const borrowForm = document.getElementById("borrowForm");
borrowForm.addEventListener("submit", submitBorrowRequest);

async function submitBorrowRequest(event) {
  event.preventDefault();
  if (!borrowForm.reportValidity()) {
    return;
  }

  createBorrowRequest(specificItemId)
    .then(() => window.location.reload())
    .catch((error) => {
      borrowError.style.display = "block";
      borrowError.innerText = error;
    });
}

//Create specificItem
const createModal = document.getElementById("createModal");
let specificdescriptionInputInput = document.getElementById(
  "specificdescriptionInput"
);
const createError = document.getElementById("createError");
createError.style.display = "none";

createBtn.onclick = () => {
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

  create(id, specificdescriptionInputInput.value)
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

updateBtn.onclick = () => {
  updateModal.style.display = "block";
  nameInput.value = name;
  descriptionInput.value = description;
  barcodeInput.value = barcode;
  shelfBarcodeInput.value = shelfBarcode;
};

const updateForm = document.getElementById("updateForm");
updateForm.addEventListener("submit", handleCreate);

async function handleCreate(event) {
  event.preventDefault();
  if (!updateForm.reportValidity()) {
    return;
  }

  let savedFileName;
  let shelf;

  try {
    if (imageInput.files.length > 0) {
      let response = await uploadImage(imageInput.files[0]);
      savedFileName = response.path;
    }

    if (shelfBarcodeInput.value != "") {
      let response = await scanBarcode(shelfBarcodeInput.value);
      shelf = response.shelf.id;
    }

    await update(
      id,
      nameInput.value,
      descriptionInput.value,
      barcodeInput.value,
      savedFileName,
      shelf
    );

    window.location.reload();
  } catch (error) {
    updateError.style.display = "block";
    updateError.innerText = error;
  }
}

//delete baseitem
const deleteModal = document.getElementById("deleteModal");
const deleteError = document.getElementById("deleteError");
deleteError.style.display = "none";

deleteBtn.onclick = () => {
  deleteModal.style.display = "block";
};

document.getElementById("deleteForm").addEventListener("submit", submitDelete);

async function submitDelete(event) {
  event.preventDefault();
  deleteBaseItem(id)
    .then(() => goBack())
    .catch((error) => {
      deleteError.style.display = "block";
      deleteError.innerText = error;
    });
}


//delete specific
const deleteSpecificModal = document.getElementById("deleteSpecificModal");

const deleteSpecificError = document.getElementById("deleteSpecificError");
deleteSpecificError.style.display = "none";

const deleteSpecificForm = document.getElementById("deleteSpecificForm");
deleteSpecificForm.addEventListener("submit", submitSpecificDelete);

async function submitSpecificDelete(event) {
  event.preventDefault();
  deleteSpecificItem(specificItemId)
    .then(() => window.location.reload())
    .catch((error) => {
      deleteSpecificError.style.display = "block";
      deleteSpecificError.innerText = error;
    });
}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    createModal.style.display = "none";
    borrowModal.style.display = "none";
    updateModal.style.display = "none";
    deleteModal.style.display = "none";
    deleteSpecificModal.style.display = "none";
  };
});

//go back btn
document.getElementById("backBtn").onclick = () => {
  goBack();
};

function goBack() {
  window.location.href = "/baseitems";
}

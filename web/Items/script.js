import { create, getAll, uploadImage } from "../services/items.service.js";

await getAll()
  .then((data) => displayTable(data))
  .catch((error) => {
    console.log(error);
  });

function displayTable(data) {
  var table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
  for (let i = 0; i < data.length; i++) {
    table.innerHTML += `
         <tr data-id="${data[i].id}">
            <td>${data[i].name}</td>
            <td>${data[i].email}</td>
            <td>${data[i].role}</td>
            <td>${data[i].borrowed_items}</td>
          </tr>`;
  }
  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/users/details/?id=" + id;
    });
  });
}

//create User
const createBaseItemModal = document.getElementById("createBaseItemModal");
var nameInput = document.getElementById("name");
var descriptionInput = document.getElementById("description");
var imageInput = document.getElementById("image");
var barcodeInput = document.getElementById("barcode");

document.getElementById("createUserBtn").onclick = () => {
  createBaseItemModal.style.display = "block";
  nameInput.value = "";
  descriptionInput.value = "";
  imageInput.value = "";
  barcodeInput.value = "";
};

document.getElementById("closeCreateUserBtn").onclick = () => {
  createBaseItemModal.style.display = "none";
};

const createBaseItemForm = document.getElementById("createBaseItemForm");
createBaseItemForm.addEventListener("submit", handleBaseItemUser);

async function handleBaseItemUser(event) {
  event.preventDefault();
  if (!createBaseItemForm.reportValidity()) {
    return;
  }

  const name = nameInput.value;
  const description = descriptionInput.value;
  const barcode = barcodeInput.value;
  const image = imageInput.files[0];

  let savedFileName;

  savedFileName = await uploadImage(image)
    .then((response) => console.log(response))
    .catch((error) => {
      console.log(error);
    });



  // await create(name, description, barcode, savedFileName, null)
  //   .then(() => window.location.reload())
  //   .catch((error) => {
  //     console.log(error);
  //   });
}

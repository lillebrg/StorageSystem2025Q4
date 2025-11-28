import { create, getAll, uploadImage } from "../services/pages/baseitem.service.js"
//paginator
let currentPage = 0;
document.getElementById("pageBack").onclick = () =>{
   if(currentPage >= 1){
    currentPage -= 1;
   }
    console.log(currentPage)
    getItems();
};

document.getElementById("pageNext").onclick = () =>{
 
  currentPage += 1;
  getItems()
};


//search
let search;
document.getElementById("searchBtn").onclick = () => {
  search = document.getElementById("searchInput").value;
  getItems();
};

let getError = document.getElementById("getError");
getError.style.display = "none";
getItems();

async function getItems(){
document.getElementById("pageCount").innerHTML = `Page ${currentPage+1}`


await getAll(10, 10 * currentPage, search)
  .then((data) => {
    console.log(data)
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
    if (data.length <= 0) {
      getError.style.display = "block";
      getError.innerHTML = "No items in the system";
      }
  data.forEach(baseItem => {
    table.innerHTML += `
         <tr data-id="${baseItem.id}">
            <td>${baseItem.name}</td>
            <td>${baseItem.description}</td>
            <td><img src="${baseItem.image_url}" style="width: 100px;"/></td>
            <td>${baseItem.specific_items_count}</td>
            <td>${baseItem.specific_items_available_count}</td>
          </tr>`;
  });


  document.querySelectorAll("#tBody tr").forEach((row) => {
    row.addEventListener("click", () => {
      const id = row.dataset.id;
      window.location.href = "/users/details/?id=" + id;
    });
  });
}
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
    savedFileName.path

  );

  window.location.reload();

} catch (error) {
  createError.style.display = "block";
  createError.innerText = error;
}

}

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    createModal.style.display = "none";
  };
});

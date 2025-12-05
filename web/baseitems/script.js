import { create, getAll, uploadImage, scanBarcode } from "../services/pages/baseitem.service.js"
//role
const createBtn = document.getElementById("createBtn")
if(localStorage.getItem("role") == "User"){
  createBtn.style.display = "none"
}

//paginator
let currentPage = 0;
document.getElementById("pageBack").onclick = () =>{
   if(currentPage >= 1){
    currentPage -= 1;
   }
    getItems();
};

document.getElementById("pageNext").onclick = () =>{
 
  currentPage += 1;
  getItems()
};


//search
let search;
const searchForm = document.getElementById("searchForm");
searchForm.addEventListener("submit", handleSearch);

async function handleSearch(event){
  event.preventDefault();
  search = document.getElementById("searchInput").value;
  getItems();
}

let getError = document.getElementById("getError");
getError.style.display = "none";
getItems();

async function getItems(){
document.getElementById("pageCount").innerHTML = `Page ${currentPage+1}`


await getAll(10, 10 * currentPage, search)
  .then((data) => {
    displayTable(data);
  })
  .catch((error) => {
    getError.style.display = "block";
    getError.color = "red";
    getError.innerHTML = error;
  });
}

function displayTable(data) {
  let table = document.getElementById("tBody");
  table.innerHTML = ""; // clear old table
    if (data.length <= 0) {
      getError.style.display = "block";
      getError.innerHTML = "No items in the system";
      return;
  } else {
    getError.style.display = "none";
  }
  data.forEach(baseItem => {
    table.innerHTML += `
         <tr data-id="${baseItem.id}">
            <td>${baseItem.name}</td>
            <td>${baseItem.description}</td>
            <td><img src="${baseItem.image_url || "/assets/images/placeholder.png"}" class="baseitem-image"/></td>
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
let shelfBarcodeInput = document.getElementById("shelfBarcode");


const createError = document.getElementById("createError");
createError.style.display = "none";

createBtn.onclick = () => {
  createModal.style.display = "block";
  nameInput.value = "";
  descriptionInput.value = "";
  imageInput.value = "";
  barcodeInput.value = "";
  shelfBarcodeInput.value = "";
};

const createForm = document.getElementById("createForm");
createForm.addEventListener("submit", handleCreate);

async function handleCreate(event) {
  event.preventDefault();
  if (!createForm.reportValidity()) {
    return;
  }


  let shelf;
  let savedFileName;

try {
  if(shelfBarcodeInput.value != ""){
    let response = await scanBarcode(shelfBarcodeInput.value);
    if(response?.shelf?.id){
      shelf = response.shelf.id
    }
  }

  if (imageInput.files.length > 0) {
    let response = await uploadImage(imageInput.files[0]);
    if(response?.path){
      savedFileName = response.path
    }
  }

  await create(
    nameInput.value,
    descriptionInput.value,
    barcodeInput.value,
    savedFileName,
    shelf
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

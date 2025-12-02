import { acceptRequest, getAll, rejectRequest } from "../services/pages/borrowrequest.service.js";

let getError = document.getElementById("getError");
getError.style.display = "none";

await getAll()
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
    getError.innerHTML = "No borrow requests at the moment";
  }
  data.forEach((request) => {
    table.innerHTML += `
         <tr data-id="${request.id}" data-from="${request.loaned_to.name}" data-name="${request.base_item.name}" data-image="${request.base_item.image_url}">
            <td>${request.loaned_to.name}</td>
            <td>${request.base_item.name}</td>
            <td><img src="${request.base_item.image_url}" style="max-height: 80px; max-width: 80px;"/></td>
            <td>${request.specific_item.description}</td>
            <td>${request.accepted == true ? "accepted" : "Not handled"}</td>
          </tr>`;
  });
}

//review borrowRequest
let specificItemId;
const reviewError = document.getElementById("reviewError");
reviewError.style.display = "none";

const reviewModal = document.getElementById("reviewModal");
const reviewForm = document.getElementById("reviewForm");

reviewForm.addEventListener("submit", submitReview);

async function submitReview(event) {
  event.preventDefault();

  // find out which button was clicked
  const button = event.submitter;

  if (!reviewForm.reportValidity()) {
    return;
  }

  let action;

  if (button.id === "acceptRequestBtn") {
    action = acceptRequest;
  } else {
    action = rejectRequest;
  }

  action(specificItemId)
    .then(() => window.location.reload())
    .catch((error) => {
      reviewError.style.display = "block";
      reviewError.innerText = error;
    });
}

document.querySelectorAll("#tBody tr").forEach((row) => {
  row.addEventListener("click", () => {
    specificItemId = row.dataset.id;
    document.getElementById(
      "borrowTitle"
    ).innerHTML = `Let ${row.dataset.from} borrow item "${row.dataset.name}"?`;
    document.getElementById("reviewImg").src = row.dataset.image;
    reviewModal.style.display = "block";
  });
});

// Close modals when clicking on any close button
document.querySelectorAll(".closeModal").forEach((closeBtn) => {
  closeBtn.onclick = () => {
    reviewModal.style.display = "none";
  };
});

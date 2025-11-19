import { create, getAll } from "../services/user.service.js";

await getAll().then((data) => {
    if(!data){
        console.log(data)
        //displayError()
    }
 displayTable(data);

});

function displayTable(data){
      var table = document.getElementById(`tBody`);
      table.innerHTML = ""; // clear old table
      for (let i = 0; i < data.length; i++) {
        table.innerHTML += `
         <tr>
            <td>${data[i].name}</td>
            <td>${data[i].email}</td>
            <td>${data[i].role}</td>
            <td>${data[i].borrowed_items}</td>
          </tr>`;
      }
}


document.querySelectorAll(".responsive-table tbody tr").forEach((row) => {
  row.addEventListener("click", () => {
    window.location.href = "/users/details";
  });
});

var role = (document.getElementById("role").value = "");
const createUserModal = document.getElementById("createUserModal");
const CreateBtn = document.getElementById("createUserBtn");


  var nameInput = document.getElementById("name")
  var emailInput = document.getElementById("email")
  var passwordInput = document.getElementById("password")
  var roleInput = document.getElementById("role")


CreateBtn.onclick = () => {
  createUserModal.style.display = "block";
  nameInput.value = "";
  emailInput.value = "";
  passwordInput.value = "";
  roleInput.value = "";
};

document.getElementById("closeCreateUserBtn").onclick = () => {
  createUserModal.style.display = "none";
};

const form = document.querySelector(".form");
form.addEventListener("submit", handleCreateUser);

async function handleCreateUser(event) {
  event.preventDefault();
 var name = nameInput.value.trim();
 var email =  emailInput.value.trim();
 var password =  passwordInput.value.trim();
 var role =  roleInput.value; //set value so no need for trim

  // validate all fields
  if (!name || !email || !password || !role) {
    alert("Please fill in all fields.");
    return;
  }

    var response = await create(name, email, password, role);
    if(response){
        window.location.href='/users'
    }
  
}
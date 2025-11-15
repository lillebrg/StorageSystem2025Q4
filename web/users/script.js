  document.querySelectorAll('.responsive-table tbody tr').forEach(row => {
    row.addEventListener('click', () => {
        window.location.href = "/users/details";
    });
  });

var role = document.getElementById('role').value = '';
const createUserModal = document.getElementById("createUserModal");
const submitUserBtn = document.getElementById("submitUserBtn");
const CreateBtn = document.getElementById("createUserBtn");

CreateBtn.onclick = () => {
    submitUserBtn.disabled = true;
    createUserModal.style.display = "block";
};

document.getElementById('closeCreateUserBtn').onclick = () => {
    createUserModal.style.display = "none";
};
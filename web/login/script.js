import { login, updatePassword } from "../services/user.service.js";

const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");

const loginForm = document.getElementById("loginForm");
loginForm.addEventListener("submit", handleLogin);
var loginAlert = document.getElementById("loginError");
loginAlert.style.display = "none";

const newPasswordForm = document.getElementById("newPasswordForm");
newPasswordForm.addEventListener("submit", handleNewPassword);
var newPasswordError = document.getElementById("newPasswordError");
newPasswordError.style.display = "none";

async function handleLogin(event) {
  event.preventDefault();

  if (!loginForm.reportValidity()) {
    return;
  }

  const email = emailInput.value;
  const password = passwordInput.value;

  await login(email, password)
    .then((response) => {
      console.log(response);
      localStorage.setItem("token", response.access_token);
      localStorage.setItem("role", response.role);
      if (response.change_password_on_next_login) {
        const changePasswordModal = document.getElementById(
          "changePasswordModal"
        );
        changePasswordModal.style.display = "block";
      } else {
        window.location.href = "/items";
      }
    })
    .catch((error) => {
      loginAlert.style.display = "block";
      loginAlert.innerText = error;
    });
}

async function handleNewPassword(event) {
  event.preventDefault();
  if (!newPasswordForm.reportValidity()) {
    return;
  }

  var newPasswordInput = document.getElementById("newPassword").value;
  var repeatNewPasswordInput =
    document.getElementById("repeatNewPassword").value;
  if (newPasswordInput != repeatNewPasswordInput) {
    alert("New passwords do not match");
    return;
  }
  updatePassword(null, passwordInput.value, newPasswordInput)
    .then(() => (window.location.href = "/items"))
    .catch((error) => {
      newPasswordError.style.display = "block";
      newPasswordError.innerText = error;
    });
}

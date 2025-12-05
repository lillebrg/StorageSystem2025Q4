import { login } from "../services/auth.js";
import { updatePassword } from "../services/pages/user.service.js";

const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");

const loginForm = document.getElementById("loginForm");
loginForm.addEventListener("submit", handleLogin);
let loginError = document.getElementById("loginError");
loginError.style.display = "none";

const newPasswordForm = document.getElementById("newPasswordForm");
newPasswordForm.addEventListener("submit", handleNewPassword);
let newPasswordError = document.getElementById("newPasswordError");
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
      localStorage.setItem("token", response.access_token);
      localStorage.setItem("refreshToken", response.refresh_token);
      localStorage.setItem("role", response.role);
      if (response.change_password_on_next_login) {
        let changePasswordModal = document.getElementById("changePasswordModal"
        );
        changePasswordModal.style.display = "block";
      } else {
        window.location.href = "/baseitems";
        //TODO: call subscribetonotifications to get notifications on borrowrequests
      }
    })
    .catch((error) => {
      loginError.style.display = "block";
      loginError.innerText = error;
    });
}

async function handleNewPassword(event) {
  event.preventDefault();
  if (!newPasswordForm.reportValidity()) {
    return;
  }

  let newPasswordInput = document.getElementById("newPassword").value;
  let repeatNewPasswordInput = document.getElementById("repeatNewPassword").value;
  if (newPasswordInput != repeatNewPasswordInput) {
    changePasswordError.style.display = "block";
    changePasswordError.innerText = "New passwords do not match";
    return;
  }
  updatePassword(null, passwordInput.value, newPasswordInput)
    .then(() => (window.location.href = "/baseitems"))
    .catch((error) => {
      newPasswordError.style.display = "block";
      newPasswordError.innerText = error;
    });
}

import { login } from "../services/user.service.js";

const emailInput = document.getElementById("email");
const passwordInput = document.getElementById("password");

const form = document.querySelector(".form");
form.addEventListener("submit", handleLogin);

async function handleLogin(event) {
  event.preventDefault();

  const email = emailInput.value.trim();
  const password = passwordInput.value.trim();
//TODO valid email dosent work
   if (!emailInput.checkValidity()) {
    alert("Please enter a valid email address.");
    return;
  }

  if (!email || !password) {
    alert("Please fill in both fields.");
    return;
  }

  var response = await login(email, password);
  if (response) {
    sessionStorage.setItem("token", response.access_token);
    sessionStorage.setItem("role", response.role);
    window.location.href = "/users";
  }
}

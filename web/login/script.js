import { loginRequest } from '../api.js'

const usernameInput = document.getElementById("email");
const passwordInput = document.getElementById("password");

const form = document.querySelector(".form");
form.addEventListener("submit", handleLogin);

async function handleLogin(event) {
  event.preventDefault(); // ⛔ stops the browser from submitting/reloading

  const username = usernameInput.value.trim();
  const password = passwordInput.value.trim();

  // basic email format check (HTML also validates type="email")
  if (!username || !password) {
    alert("Please fill in both fields.");
    return;
  }

  if (!username.includes("@",".")) {
    alert("Please enter a valid email address.");
    return;
  }

  await loginRequest(username, password);
}
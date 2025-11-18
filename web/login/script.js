import { login } from "../services/user.service.js";

const usernameInput = document.getElementById("email");
const passwordInput = document.getElementById("password");

const form = document.querySelector(".form");
form.addEventListener("submit", handleLogin);

async function handleLogin(event) {
  event.preventDefault();

  const username = usernameInput.value.trim();
  const password = passwordInput.value.trim();

  // basic email format check (HTML also validates type="email")
  if (!username || !password) {
    alert("Please fill in both fields.");
    return;
  }

  // if (!username.includes("@",".")) {
  //   alert("Please enter a valid email address.");
  //   return;
  // }

  var response = await login(username, password);
  if (response) {
    sessionStorage.setItem("token", response.access_token);
    sessionStorage.setItem("role", response.role);
    window.location.href = "/users";
  }
}

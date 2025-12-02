import { request } from "./api.js";

export function login(email, password) {
  return request("POST", "/user/login", {
    email,
    password,
  });
}

export function logout() {
  let refresh_token = localStorage.getItem("refreshToken");
  request("POST", "/user/logout", {refresh_token})
  .then(() => {
    localStorage.removeItem("token");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("role");
    window.location.href = "/";
  })
}

export function refreshAuthToken() {
    let refresh_token = localStorage.getItem("refreshToken");
    request("POST", "/user/refresh", {refresh_token})
    .then((response) => {
      console.log(response);
      localStorage.setItem("token", response.access_token);
      localStorage.setItem("refreshToken", response.refresh_token);
      localStorage.setItem("role", response.role);
  })
  .catch(() => {
    window.location.href = "/"
  });
}

import { request } from "./api.js";

export function get() {
  return request("GET", `/user`);
}

export function getAll() {
  return request("GET", `/users`);
}

export function create(name, email, password, role) {
  return request("POST", "/users/create", {
    name,
    email,
    password,
    role,
  });
}

export function update(email, username) {
  return request("PUT", "/user/update", {
    email,
    username,
  });
}

export function updatePassword(oldPassword, newPassword) {
  return request("PUT", "/user/change-password", {
    oldPassword,
    newPassword,
  });
}

export function login(email, password) {
  return request("POST", "/user/login", {
    email,
    password,
  });
}

export function logout() {
  sessionStorage.removeItem("token");
  sessionStorage.removeItem("role");
  window.location.href = "/";
}

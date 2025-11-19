import { request } from "./api.js";

export function get(id = null) {
  if (id) {
    return request("GET", `/users/${id}`);
  } else {
    return request("GET", `/user`);
  }
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

export function update(id = null, email, name, role, cponl = null) {
  if (id) {
    return request("PUT", `/users/${id}`, {
      email,
      name,
      role,
      cponl,
    });
  } else {
    return request("PUT", `/user`, {
      email,
      name,
    });
  }
}

export function updatePassword(id = null, oldPassword = null, newPassword) {
  if (id) {
    return request("POST", `/users/${id}/reset-password`, {
      new_password: newPassword,
    });
  } else {
    return request("POST", "/user/change-password", {
      oldPassword,
      newPassword,
    });
  }
}

export function deleteUser(id) {
  return request("DELETE", `/users/${id}`);
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

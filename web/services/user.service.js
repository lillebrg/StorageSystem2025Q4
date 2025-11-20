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

export function update(email, username, id = null) {
  if (id) {
    console.log(id);
    return request("PUT", `/users/${id}`, {
      email,
      username,
    });
  } else {
    return request("PUT", `/user`, {
      email,
      username,
    });
  }
}

export function updatePassword(oldPassword = null, newPassword, id = null) {
  if (id) {
    console.log(id);
    return request("PUT", `/users/${id}/reset-password`, {
      newPassword,
    });
  } else {
    return request("PUT", "/user/change-password", {
      oldPassword,
      newPassword,
    });
  }
}

export function deleteUser(id) {
  console.log(id)
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

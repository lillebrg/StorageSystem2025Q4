import { request } from "../api.js";

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

export function getBorrowRequests() {
  return request("GET", `/user/borrow-requests`);
}

export function create(name, email, password, role) {
  return request("POST", "/users/create", {
    name,
    email,
    password,
    role,
  });
}

export function update(id = null, email, name, role, change_password_on_next_login = null) {
  if (id) {
    return request("PUT", `/users/${id}`, {
      email,
      name,
      role,
      change_password_on_next_login,
    });
  } else {
    return request("PUT", `/user`, {
      email,
      name,
    });
  }
}

export function updatePassword(id = null, current_password = null, new_password) {
  if (id) {
    return request("POST", `/users/${id}/reset-password`, {
      new_password,
    });
  } else {
    return request("POST", "/user/change-password", {
      current_password,
      new_password,
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

export async function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("role");

  // Unsubscribe from notifications
  if ("serviceWorker" in navigator && "PushManager" in window) {
      const registration = await navigator.serviceWorker.getRegistration();

      if (registration) {
          const subscription = await registration.pushManager.getSubscription()

          if (subscription)
              await subscription.unsubscribe();

          await registration.unregister();
      }
  }

  window.location.href = "/";
}

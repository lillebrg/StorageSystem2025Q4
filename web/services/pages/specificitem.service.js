import { request } from "../api.js";

export function get(id) {
  return request("GET", `/base-items/${id}`);
}

export function create(id, description) {
  return request("POST", `/base-items/${id}/specific-items`, {
    description,
  });
}

export function update(id, description,) {
  return request("PUT", `/specific-items/${id}`, {
    description,
  });
}

export function deleteSpecificItem(id) {
  return request("DELETE", `/specific-items/${id}`);
}

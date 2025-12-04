import { request } from "../api.js";

export function get(id) {
    return request("GET", `/storages/${id}`);
}

export function getAll() {
  return request("GET", `/storages`);
}

export function create(name) {
  return request("POST", "/storages", {
    name,
  });
}

export function update(id, name) {
    return request("PUT", `/storages/${id}`, {
      name,
    });
}

export function deleteStorage(id) {
  return request("DELETE", `/storages/${id}`);
}

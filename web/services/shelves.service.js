import { request } from "./api.js";

export function get(id) {
  return request("GET", `/shelves/${id}`);
}

export function create(rackId, shelf_no) {
  return request("POST", `/racks/${rackId}/shelves`, {
    shelf_no,
  });
}

export function update(id, shelf_no) {
  return request("PUT", `/shelves/${id}`, {
    shelf_no,
  });
}

export function deleteShelf(id) {
  return request("DELETE", `/shelves/${id}`);
}

import { request } from "./api.js";

export function get(id) {
  return request("GET", `/shelves/${id}`);
}

export function create(rackId, shelf_no) {
  return request("POST", `/racks/${rackId}/shelves`, {
    rack_no: shelf_no,
  });
}

export function update(id, rack_no) {
  return request("PUT", `/shelves/${id}`, {
    rack_no,
  });
}

export function deleteRack(id) {
  return request("DELETE", `/shelves/${id}`);
}

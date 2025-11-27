import { request } from "./api.js";

export function get(id) {
  return request("GET", `/racks/${id}`);
}

export function create(storageId, rack_no) {
  return request("POST", `/storages/${storageId}/racks`, {
    rack_no,
  });
}

export function update(id, rack_no) {
  return request("PUT", `/racks/${id}`, {
    rack_no,
  });
}

export function deleteRack(id) {
  return request("DELETE", `/racks/${id}`);
}

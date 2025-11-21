import { request } from "./api.js";

export function get(id) {
  return request("GET", `/items/${id}`);
}

export function getAll(limit, offset, search) {
  return request("GET", `/base-items`,{
    limit,
    offset,
    search
  });
}

export function create(name, description, barcode, image_path, shelf_id) {
  return request("POST", "/bate-items", {
    name,
    description,
    barcode,
    image_path,
    shelf_id
  });
}

export function update(id, name, description, barcode, image_path, shelf_id) {
  return request("PUT", `/bate-items/${id}`, {
    name,
    description,
    barcode,
    image_path,
    shelf_id
  });
}

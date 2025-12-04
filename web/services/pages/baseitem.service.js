import { request } from "../api.js";

export function get(id) {
  return request("GET", `/base-items/${id}`);
}

export function getAll(limit, offset, search) {
  if(search == null){
    search = "%20"
  }
  return request("GET", `/base-items?limit=${limit}&offset=${offset}&search=${search}`,);
}

export function create(name, description, barcode, image_path = "", shelf_id) {
  return request("POST", "/base-items", {
    name,
    description,
    barcode,
    image_path,
    shelf_id,
  });
}

export function update(id, name, description, barcode, image_path = "", shelf_id) {
  return request("PUT", `/base-items/${id}`, {
    name,
    description,
    barcode,
    image_path,
    shelf_id,
  });
}


export function deleteBaseItem(id) {
  return request("DELETE", `/base-items/${id}`);
}

export function uploadImage(image) {
  return request("POST", `/images`, null, image);
}

export function scanBarcode(barcode) {
  return request("POST", `/barcodes/scan?barcode=${barcode}`);
}
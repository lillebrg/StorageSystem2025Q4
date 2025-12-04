import { request } from "../api.js";

export function getAll() {
  return request("GET", `/borrow-requests`);
}

export function createBorrowRequest(specific_item) {
  return request("POST", `/borrow-requests`, {specific_item});
}

export function acceptRequest(id) {
  return request("POST", `/borrow-requests/${id}/accept`, );
}

export function rejectRequest(id) {
  return request("POST", `/borrow-requests/${id}/reject`, );
}

export function returnItem(id) {
  return request("POST", `/borrow-requests/${id}/return`, );
}
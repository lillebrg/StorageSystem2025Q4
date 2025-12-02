import { request } from "../api.js";

export function getAll() {
  return request("GET", `/borrow-requests`);
}

export function createBorrowRequest(specific_item) {
  return request("POST", `/borrow-requests`, {specific_item});
}

export function acceptRequest(specific_itemId) {
  return request("POST", `/borrow-requests/${specific_itemId}/accept`, );
}

export function rejectRequest(specific_itemId) {
  return request("POST", `/borrow-requests/${specific_itemId}/reject`, );
}

export function returnItem(specific_itemId) {
  return request("POST", `/borrow-requests/${specific_itemId}/return`, );
}
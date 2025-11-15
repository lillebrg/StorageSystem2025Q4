import { url } from "/services/apiurl.js";

export async function loginRequest(email, password) {
      try {
    const response = await fetch(`${url}/user/login`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ email, password }),
    });

    if (!response.ok) {
      return false;
    }
    
    const data = await response.json();
    sessionStorage.setItem("role", data.role);
    sessionStorage.setItem("token", data.access_token);
    
    return true;

  } catch (error) {
    alert(error.message);
    return false;
  }
}
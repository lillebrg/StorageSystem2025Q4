export async function loginRequest(username, password) {
    console.log("login")
      try {
    const response = await fetch("https://your-api-url.com/login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ username, password }),
    });

    if (!response.ok) {
      throw new Error("Login failed");
    }

    const data = await response.json();
    console.log("Login success:", data);

    // You could redirect or store a token
    // window.location.href = "dashboard.html";

  } catch (error) {
    alert(error.message);
  }
}
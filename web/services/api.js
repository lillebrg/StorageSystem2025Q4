import { url } from "/services/apiurl.js";

export async function request(method, path, body = null) {
  var token = localStorage.getItem("token");
  const headers = {};
  headers["Authorization"] = `Bearer ${token}`;

  if (body) headers["Content-Type"] = "application/json";
  return new Promise((resolve, reject) => {
    fetch(url + path, {
      method,
      headers,
      body: body ? JSON.stringify(body) : undefined,
    })
      .then(async (response) => {
        try {
          const json = await response.json();

          if (response.ok) return resolve(json);

          if (json.error) return reject(json.error);

          if (json.message) return reject(json.message);

          if (json.title) return reject(json.title);

          if (json.errors) return reject(Object.values(json.errors)[0][0]);
        } finally {
          reject("Request failed with HTTP code " + response.status);
        }
      })
      .catch((err) => reject(err.message));
  });
}

export async function requestUploadImage(image) {
  var token = localStorage.getItem("token");
  const headers = {};
  headers["Authorization"] = `Bearer ${token}`;

  return new Promise((resolve, reject) => {
    fetch(url + "/images", {
      method: "POST",
      headers,
      body: image,
    })
      .then(async (response) => {
        console.log(response)
        const json = await response.json();
          console.log(json)
        try {
          

          if (response.ok) return resolve(json);

        } finally {
          reject("Request failed with HTTP code " + response.status);
        }
      })
      .catch((err) => reject(err.message));
  });
}

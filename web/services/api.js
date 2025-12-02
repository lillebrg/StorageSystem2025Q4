import { url } from "/services/apiurl.js";
import { refreshAuthToken } from "./auth.js";

export async function request(method, path, body = null, image = undefined, canRetry = true) {
  let token = localStorage.getItem("token");
  const headers = {};
  headers["Authorization"] = `Bearer ${token}`;

  if (body) headers["Content-Type"] = "application/json";
  return new Promise((resolve, reject) => {
    fetch(url + path, {
      method,
      headers,
      body: body ? JSON.stringify(body) : image,
    })
      .then(async (response) => {
        try {
        if(response.status == 401) {
          if (canRetry){
            await refreshAuthToken();
            return request(method, path, body, image, false)
          };
          //window.location.href = "/"
        }
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

//TODO, implement requestupload inside the normal request, just make it a nullable value and then go from there
export async function requestUploadImage(image) {
  const token = localStorage.getItem("token");
  const headers = {
    "Authorization": `Bearer ${token}`,
  };

  return new Promise((resolve, reject) => {
    fetch(url + "/images", {
      method: "POST",
      headers,
      body: image,
    })
      .then(async (response) => {
        if (!response.ok) {
          reject("Request failed with HTTP code " + response.status);
          return;
        }

        // If the backend returns a plain string
        const text = await response.text();
        resolve(text);
      })
      .catch((err) => reject(err.message));
  });
}

import { url } from "/services/apiurl.js";
import { refreshAuthToken } from "./auth.js";

export async function request(
  method,
  path,
  body = null,
  image = undefined,
  canRetry = true
) {
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
          if (response.status == 401) {
            if (canRetry) {
              try {
                refreshAuthToken();
                console.log("refreshtoken done");
                console.log(method);
                console.log(path);
                console.log(body);
                console.log(image);

                return request(method, path, body, image, false);
              } catch {
                //window.location.href = "/";
              }
            }
            window.location.href = "/";
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

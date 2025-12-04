import { url } from "/services/config.js";
import { refreshAuthToken } from "/services/auth.js";

export async function request(
  method,
  path,
  body = null,
  image = undefined,
  canRetry = true
) {
  let token = localStorage.getItem("token");

  const headers = {
    Authorization: `Bearer ${token}`,
  };
  if (body) headers["Content-Type"] = "application/json";

  return new Promise((resolve, reject) => {
    fetch(url + path, {
      method,
      headers,
      body: body ? JSON.stringify(body) : image,
    })
      .then(async (response) => {

        // ---------- 401 HANDLING ----------
        if (response.status === 401) {
          if (canRetry) {
            try {
              await refreshAuthToken();

              // Return the RETRY result!
              return resolve(
                await request(method, path, body, image, false)
              );
            } catch {
              return window.location.href = "/";
            }
          }

          return window.location.href = "/";
        }

        // ---------- NORMAL RESPONSE ----------
        const json = await response.json();

        if (response.ok) return resolve(json);

        if (json.error) return reject(json.error);
        if (json.message) return reject(json.message);
        if (json.title) return reject(json.title);
        if (json.errors) return reject(Object.values(json.errors)[0][0]);

        reject("HTTP " + response.status);
      })
      .catch((err) => reject(err.message));
  });
}

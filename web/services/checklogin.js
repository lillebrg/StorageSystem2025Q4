// Runs on every page that includes this file
(function checkLogin() {
  const token = localStorage.getItem("token");
  const currentPage = window.location.pathname.split("/").pop();

  if (!token) {
    // Not logged in → redirect
    window.location.href = "/login";
  }
})();

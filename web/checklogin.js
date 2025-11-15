// Runs on every page that includes this file
(function checkLogin() {
    const token = sessionStorage.getItem("token");

    // List of pages that should NOT redirect (e.g., login, signup)
    const publicPages = ["login.html"];

    const currentPage = window.location.pathname.split("/").pop();

    if (!token && !publicPages.includes(currentPage)) {
        // Not logged in → redirect
        window.location.href = "/login";
    }
})();

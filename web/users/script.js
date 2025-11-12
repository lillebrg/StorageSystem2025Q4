  document.querySelectorAll('.responsive-table tbody tr').forEach(row => {
    row.addEventListener('click', () => {
        window.location.href = "/users/details";
    });
  });
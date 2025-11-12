  document.querySelectorAll('.responsive-table tbody tr').forEach(row => {
    row.addEventListener('click', () => {
      const url = row.getAttribute('data-url');
      if (url) {
        window.location.href = url;
      }
    });
  });
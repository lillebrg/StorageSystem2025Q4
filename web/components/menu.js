class MenuNav extends HTMLElement {
  constructor() {
    super();

    const shadow = this.attachShadow({ mode: "open" });

    const role = localStorage.getItem("role") || "user";
    
    const menus = {
      Admin: [
        { label: "Items", url: "/baseitems" },
        { label: "Storage", url: "/storages" },
        { label: "Borrow Requests", url: "/borrowrequests" },
        { label: "Users", url: "/users" },
        { label: "Profile", url: "/profile" }
      ],
      Operator: [
        { label: "Items", url: "/baseitems" },
        { label: "Storage", url: "/storages" },
        { label: "Borrow Requests", url: "/borrowrequests" },
        { label: "Profile", url: "/profile" }
      ],
      User: [
        { label: "Items", url: "/baseitems" },
        { label: "Profile", url: "/profile" }
      ]
    };

    const selectedMenu = menus[role] || menus["user"];

    shadow.innerHTML = `
      <style>
        nav {
          background-color: #9a03e4;
          margin: 0 auto;
          box-shadow: 0 5px 5px rgba(0, 0, 0, 0.1);
        }
        nav ul {
          list-style: none;
          display: flex;
          justify-content: center;
          margin: 0;
          padding: 0;
        }
        nav a {
          color: white;
          text-decoration: none;
          display: block;
          height: 3rem;
          line-height: 3rem;
          padding: 0 1.5rem;
          transition: background-color ease-in-out 200ms;
        }
        nav a:hover {
          background-color: #b31efc;
        }
        nav a:active {
          background-color: #7802b2;
        }
      </style>

      <nav>
        <ul>
          ${selectedMenu
            .map(item => `<li><a href="${item.url}">${item.label}</a></li>`)
            .join("")}
        </ul>
      </nav>
    `;
  }
}

customElements.define("menu-nav", MenuNav);

class MenuNav extends HTMLElement {
  constructor() {
    super();

    const shadow = this.attachShadow({ mode: "open" });

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
           <li><a href="/baseitems">Items</a></li>
           <li><a href="/storages">Storage</a></li>
           <li><a href="/users">Users</a></li>
           <li><a href="/profile">Profile</a></li>

         </ul>
       </nav>
    `;
  }
}

customElements.define("menu-nav", MenuNav);

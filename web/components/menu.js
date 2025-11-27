class MenuNav extends HTMLElement {
  constructor() {
    super();

    const shadow = this.attachShadow({ mode: "open" });

    shadow.innerHTML = `
     <style>
      nav {
  width: 100%;
  margin: 0 auto;
  box-shadow: 0px 5px 0px lightgray;
}

/* By Dominik Biedebach @domobch */
nav ul {
  list-style: none;
  text-align: center;
}
nav ul li {
  display: inline-block;
}
nav ul li a {
  display: block;
  padding: 5px;
  text-decoration: none;
  color: #7d2ae8;
  font-weight: 800;
  text-transform: uppercase;
  margin: 0 10px;
}
nav ul li a,
nav ul li a:after,
nav ul li a:before {
  transition: all .5s;
}
nav ul li a:hover {
  color: #a885eb;
}
nav.stroke ul li a,
nav.fill ul li a {
  position: relative;
}
nav.stroke ul li a:after,
nav.fill ul li a:after {
  position: absolute;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
  width: 0%;
  content: '.';
  color: transparent;
  height: 1px;
}
nav.stroke ul li a:hover:after {
  width: 100%;
}
      </style>

      <nav class="stroke">
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

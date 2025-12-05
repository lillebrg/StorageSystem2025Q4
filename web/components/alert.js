class MyAlert extends HTMLElement {
  constructor() {
  super();
    const shadow = this.attachShadow({ mode: "open" });

    shadow.innerHTML = `
      <style>
        .alert {
          padding: 20px;
          margin: 10px 0;
          border-radius: 4px;
          background-color: var(--color, #f44336);
          color: white;
          position: relative;
        }

        .closebtn {
          position: absolute;
          top: 5px;
          right: 10px;
          font-size: 22px;
          cursor: pointer;
        }
      </style>

      <div class="alert">
        <span class="closebtn">&times;</span>
        <slot></slot>
      </div>
    `;
  }

  connectedCallback() {
    const color = this.getAttribute("color") || "red";
    this.shadowRoot.querySelector(".alert").style.setProperty("--color", color);

    const closeBtn = this.shadowRoot.querySelector(".closebtn");
    closeBtn.onclick = () => {
      this.style.display = "none";
    };
  }

  set color(value) {
  this.shadowRoot.querySelector(".alert")
      .style.setProperty("--color", value);
}
}

customElements.define("my-alert", MyAlert);

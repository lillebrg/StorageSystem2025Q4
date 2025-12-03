console.log("Ready");

self.addEventListener("push", event => {
    const message = event.data.text();

    event.waitUntil(self.registration.showNotification("New notification", {
        body: message,
        icon: "/assets/images/box.png",
    }));
});

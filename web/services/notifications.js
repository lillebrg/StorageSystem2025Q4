import { request } from "./api.js";
import { vapidPublicKey } from "./config.js";

window.addEventListener("load", () => {
    if ("serviceWorker" in navigator && "PushManager" in window && localStorage.getItem("token")) {
        if (Notification.permission === "default")
            showNotificationPopup();
        else if (Notification.permission === "granted")
            subscribeToNotifications();
    }
});

function showNotificationPopup() {
    document.head.innerHTML += `
        <style>
            #notification-dialog {
                position: fixed;
                bottom: 3rem;
                right: 3rem;
                left: auto;
                background: white;
                padding: 1rem;
                border: none;
                border-radius: 0.5rem;
                box-shadow: 0 5px 5px rgba(0, 0, 0, 0.1);
                text-align: left;
                max-width: 270px;
            }

            #notification-dialog form {
                display: flex;
                justify-content: space-between;
                align-items: center;
                gap: 4rem;
            }

            #notification-dialog form button {
                border: none;
                background: transparent;
                color: #BDBDBD;
                font-size: 1.5rem;
            }

            #notification-dialog div {
                font-size: 0.8rem;
                color: #424242;
                margin-bottom: 1rem;
            }
        </style>
    `;

    document.body.innerHTML += `
        <dialog id="notification-dialog">
            <form method="dialog">
                <b>Allow notifications</b>

                <button type="submit">&times;</button>
            </form>

            <div>
                Get notified when a borrow request is accepted or rejected
            </div>

            <button id="allow-notifications" class="button">Allow</button>
        </dialog>
    `;

    document.getElementById("notification-dialog").show();

    document.getElementById("allow-notifications").onclick = () => {
        document.getElementById("notification-dialog").close();

        subscribeToNotifications();
    }
}

async function subscribeToNotifications() {
    const permission = await requestPermission();

    if (permission === "denied") return;

    if (!await navigator.serviceWorker.getRegistration())
        await navigator.serviceWorker.register("/service-worker.js");

    const registration = await navigator.serviceWorker.ready;

    if (await registration.pushManager.getSubscription()) {
        return;
    }

    const subscription = await registration.pushManager.subscribe({
        applicationServerKey: vapidPublicKey,
    });

    await request("POST", "/notifications/subscribe", {
        endpoint: subscription.endpoint,
        p256dh: arrayBufferToBase64(subscription.getKey("p256dh")),
        auth: arrayBufferToBase64(subscription.getKey("auth")),
    });
}

function arrayBufferToBase64(arrayBuffer) {
    return btoa(String.fromCharCode.apply(null, new Uint8Array(arrayBuffer)));
}

function requestPermission() {
    return new Promise(resolve => {
        if (Notification.permission !== "default") resolve(Notification.permission);

        const result = Notification.requestPermission(resolve);

        if (result) result.then(resolve);
    });
}

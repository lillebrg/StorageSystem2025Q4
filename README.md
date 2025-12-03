# ![](android/app/src/main/res/mipmap-mdpi/ic_launcher.png) Storage System

A storage system with a web interface and Android app for scanning barcodes     

## Setting up

### Backend

Within the `backend/SwaggerRestApi/SwaggerRestApi` folder, copy `appsettings.example.json`
to `appsettings.json` and fill out the values.

Run `dotnet run` to start the server.

### Web

Within the `web/services` folder, copy `apiurl.example.js` to `apiurl.js` and fill out the
url to the backend.

Start any static HTTP server inside the `web` folder to run the code.

### Android

Open the `android` folder in Android Studio and build the project from there.

You can change the API url inside `android/app/gradle.properties`. The IP address `10.0.2.2`
is used to access localhost from the Android emulator.

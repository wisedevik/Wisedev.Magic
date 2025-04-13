# Wisedev.Magic Server
A minimal private server implementation for Clash of Clans v5.2.4 (2014 version).  
Educational Clash of Clans 2014 private server emulation in .NET  
⚠️ **Note**: Project is under active development.

## 📋 Current Features
- Player authentication system
- Minimal game command processing (EndClientTurn)
- Minimal core gameplay systems
- MongoDB integration

## ⚙️ Requirements
- .NET 9.0
- MongoDB
- Port 9339 accessible

## 🛠 Configuration
 Open ``config.json``
 ```json
"UseLocalResources": false
 ```
 Resource Modes: 
 - CDN Mode (default)
    - Set ``"UseLocalResources": false``
    - Files download from:
        - ``API_URL/assets/fingerprint.json``
        - ``API_URL/assets/level/starting_home.json``
        - Assets:
        ```json 
        https://api.example.com/
        ├─ {FINGERPRINT_SHA}/csv/characters.csv
        ├─ {FINGERPRINT_SHA}/sc/effects.sc
        └─ {FINGERPRINT_SHA}/logic/locale.json
        ```
 - Local Mode
    - Set ``"UseLocalResources": true``
 - Security Options
    - ``ApiShield: true``: Enforces API token validation
    - ``ApiShield: false``: Disables auth (development only)

## 🌐 CDN Version Control System
 The server implements a robust version-checking mechanism that uses CDN resources when client assets are outdated

## 🔄 Resource Loading Logic
 CDN Update

 When version mismatch detected:
 - Client sends ``LoginMessage`` with SHA, Client Version
 - If client version and sha != server version and sha, server sends ``LoginFailedMessage``
 - Client receives ``LoginFailedMessage`` with:
    - ErrorCode (7)
    - CDN endpoint URLs
    - Latest fingerprint.json
 - After that, the client checks the files on the server, and if the file is sha != the sha client file, it downloads a new file

## 📌 Important
 1. When using CDN mode:
    - Ensure your API endpoint is accessible
    - Rotate tokens regularly if ApiShield enabled
 2. For local mode:
    - Requires original game assets from v5.2.4
    - CSV files must match client version

## 🔮 Roadmap
 - Alliance system
 - Battles
 - Achievement system
 - Global chat

## ⚠️ Legal Notice
 This project is for educational purposes only. All game assets are copyright of Supercell. Use original client files only if you legally own them.
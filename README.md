# CS-Setup-Hub
A tool for programmer; helps install favorite softwares in a few clicks. Best for setting up in a new coding environments.
CE Setup Hub
A Windows‑only installer that helps Computer Engineering students set up essential tools from one clean checklist‑based interface.

✨ Features
Native Windows desktop app (C# WinForms).

Simple installer UI with:

Preset buttons: Essential, Full CE, AI + Coding, Clear

Category filters and checkbox lists

Details panel, progress bar, and live log

Tools catalog stored in Resources/catalog.json for easy updates.

Free and premium tools clearly marked.

Install verification (python --version, java --version, etc.).

Logs written to C:\ProgramData\CE Setup Hub.

📦 Categories Included
Package Managers

Developer Tools

Editors and IDEs

AI Coding Tools

Languages and Runtimes

Frameworks and Libraries

C and C++ Toolchain

Databases

Browsers

Learning Resources

🚀 Installation
Download the latest release from the [Looks like the result wasn't safe to show. Let's switch things up and try something else!].

Extract if zipped, then run CESetupHub.exe as Administrator.

Choose a preset or select tools manually.

Watch the progress bar and live log as installs complete.

🛠 Build Instructions (for developers)
To compile from source:

powershell
cd CE-Setup-Hub
.\build.ps1 -SelfContained
Output will be at:

Code
outputs\CE-Setup-Hub\CESetupHub.exe
For a single‑file executable:

powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
📖 Usage Notes
Requires administrator privileges.

Uses winget when available, otherwise falls back to App Installer.

Supports custom commands for WSL, Chocolatey, Scoop, pip, npm, VS Code extensions, and resource links.

🤝 Contributing
Pull requests are welcome!

Fork the repo

Create a feature branch

Commit changes

Submit a PR

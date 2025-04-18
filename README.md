# 📝 NotepadPlusPlusSessionManagerLauncher  
**Smart startup tool for Notepad++ with automatic session and workspace control via the ["Session Manager"](https://github.com/mike-foster/npp-session-manager) plugin.**

> 🛈 Although the original Session Manager plugin is no longer actively maintained,
> However, its true potential – such as launching sessions directly, context-based workspace loading, and seamless integration – is only unlocked through this launcher.
> The Session Manager Plugin is still listed as an official Notepad++ plugin and available for both x86 and x64:  
> [https://github.com/notepad-plus-plus/nppPluginList/blob/master/doc/plugin_list_x64.md](https://github.com/notepad-plus-plus/nppPluginList/blob/master/doc/plugin_list_x64.md)  
> [https://github.com/notepad-plus-plus/nppPluginList/blob/master/doc/plugin_list_x86.md](https://github.com/notepad-plus-plus/nppPluginList/blob/master/doc/plugin_list_x86.md)


## What is it?

This tool replaces the default `notepad++.exe` with an intelligent launcher that:

- Can be called directly with either a **session file** (`.npp`) or **any regular file**  
- Automatically loads the `default.npp` session when launched **without parameters**  
- Opens the **current directory as a workspace** in Notepad++ when a session file is used (`-openFoldersAsWorkspace`)

The existing Notepad++ installation is enhanced, not broken – the original EXE is safely renamed to `notepad++_original.exe`, and all launcher files are installed alongside it.

The **Session Manager plugin** is fully supported and required. The plugin’s `settings.xml` is actively updated (automatic switching of `currentSession`, `loadIntoCurrent`, etc.).

---

## Requirements

- Windows  
- Notepad++ installed  
- Session Manager plugin installed  

---

## Usage

### 1. After installation  
You can now use `notepad++.exe` just like before:

```sh
notepad++.exe                        # Opens the default session  
notepad++.exe my-file.txt           # Opens file + loads default session  
notepad++.exe my-session.npp        # Loads session and workspace  
```

### 2. No parameters?  
It always opens the `default.npp` session. If it doesn’t exist, it’s automatically created as fallback.

### 3. On first launch  
You’ll be guided to:

- Select the **Notepad++ installation folder**  
- Select the **`settings.xml`** from the Session Manager plugin  
- The `notepad++.exe` is **automatically renamed** to `notepad++_original.exe`  
- All launcher files (including the EXE) are copied into the installation folder – **no manual steps required**

Configuration is saved persistently in the Windows Registry:  
```
HKEY_CURRENT_USER\Software\NotepadPlusPlusSessionManagerLauncher
```

This includes the paths to Notepad++ and the `settings.xml`.

---

## Installation

1. Download the project as ZIP or build it with Visual Studio.  
2. Run the launcher manually once (without any parameters) to start the setup:  
   - Choose the Notepad++ installation path  
   - Choose the `settings.xml` file  
     *(Find it in: Notepad++ > Plugins > Session Manager > Settings)*  
3. The launcher will then:  
   - Rename `notepad++.exe` to `notepad++_original.exe`  
   - Copy itself and all required files into the directory  

From now on, launching Notepad++ includes full session intelligence – automatically!

---

## Key Advantages

✅ Native session + project switching  
✅ No more manual session switching  
✅ Seamless integration with Explorer and context menus  
✅ Fully backward compatible with the original Notepad++

---

## License

**Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)**  
You may freely use, modify, and share this project – **but not for commercial use**.  
👉 [Read full license terms](https://creativecommons.org/licenses/by-nc/4.0/)

---

## Author & Contact

**Marcel Schmitz**, 2025  
Made with ❤️ and ☕ – for everyone who uses Notepad++ seriously and productively.  

For feedback or suggestions: [GitHub Issues or contact me directly]

---

## Notes for Developers

- Written in **C# (.NET)**  
- Entry point: `Program.Main`  
- Helper classes: `SessionEditor` and `NppAutomation` encapsulate core logic (e.g., tab switching)  
- `.lnk` shortcuts can be used for testing – adjust paths as needed  
- In **Debug mode**, no registry is used – fallback paths are hardcoded  

🛠️ Pull requests & feature suggestions always welcome!

--- 

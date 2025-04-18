# NotepadPlusPlusSessionManagerLauncher
**Starttool für Notepad++ mit automatischer Session- und Workspace-Steuerung via „Session Manager“-Plugin.**

---

## Was ist das?

Dieses Tool ersetzt die normale `notepad++.exe` durch einen intelligenten Launcher, der:

- direkt mit einer **Session-Datei** (`.npp`) oder einer **beliebigen Datei** aufgerufen werden kann,
- bei Start **ohne Parameter** automatisch die `default.npp`-Session lädt,
- und bei Session-Dateien das aktuelle Verzeichnis als Workspace in Notepad++ öffnet (`-openFoldersAsWorkspace`).

Dabei wird die bestehende Notepad++-Installation **automatisch erweitert**, nicht beschädigt – das Original wird einfach in `notepad++_original.exe` umbenannt und alle Launcher-Dateien werden übernommen.

Das Plugin **„Session Manager“** wird dabei vollständig unterstützt und vorausgesetzt. Die `settings.xml` des Plugins wird aktiv angepasst (automatische Umschaltung von `currentSession`, `loadIntoCurrent`, etc.).

---

## Voraussetzungen

- Windows
- Notepad++ installiert
- Plugin **Session Manager** installiert
  
---

## Verwendung

### 1. Nach Installation:
Du kannst `notepad++.exe` ab sofort genauso verwenden wie vorher – mit:

```sh
notepad++.exe                          # Öffnet die Default-Session
notepad++.exe meine-datei.txt         # Öffnet Datei + lädt Default-Session
notepad++.exe projekt.npp             # Lädt die Session + Workspace aus dem Pfad
```

### 2. Start ohne Parameter:
Öffnet immer die `default.npp`-Session. Falls `default.npp` noch nicht existiert, wird sie automatisch als Fallback verwendet.

### 3. Beim ersten Start:
- Wird der Benutzer nach dem Speicherort von Notepad++ gefragt
- Und nach dem Ort der `settings.xml` des Plugins „Session Manager“
- Die `notepad++.exe` wird automatisch umbenannt in `notepad++_original.exe`
- Alle Dateien des Launchers (inkl. EXE) werden automatisch ins Installationsverzeichnis kopiert – keine manuelle Aktion nötig

Die Konfiguration wird dauerhaft in der **Windows-Registry** gespeichert:
```
HKEY_CURRENT_USER\Software\NotepadPlusPlusSessionManagerLauncher
```
Dort werden die Pfade zu Notepad++ und zur `settings.xml` hinterlegt.

---

## Installation

1. Lade das Projekt als ZIP oder baue es über Visual Studio.
2. Starte den Launcher einmal manuell (ohne Parameter), um die Einrichtung durchzuführen:
   - Auswahl des Notepad++-Installationspfads
   - Auswahl der `settings.xml`-Datei
     - Die `settings.xml` findest du unter:  `Notepad++ > Plugins > Session Manager > Settings`
3. Der Launcher übernimmt alles Weitere automatisch:
   - `notepad++.exe` wird umbenannt zu `notepad++_original.exe`
   - Die neue EXE und Hilfsdateien werden ins Verzeichnis kopiert

Danach kannst du Notepad++ ganz normal nutzen – ab sofort mit Session-Intelligenz.

---

## Vorteile auf einen Blick

- **Sessions + Projekte** endlich nativ aufrufbar
- Keine manuelle Session-Umschaltung mehr nötig
- Nahtlose Integration in Datei-Explorer und Kontextmenüs
- Völlig rückwärtskompatibel zu originalem Notepad++

---

## Lizenz

**Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)**

Du darfst dieses Projekt frei verwenden, verändern und weitergeben – **aber nicht kommerziell nutzen**.  
[Details zur Lizenz](https://creativecommons.org/licenses/by-nc/4.0/)

---

## Autor & Kontakt

**Marcel Schmitz**, 2025  
Made with Love and Coffee – für alle, die Notepad++ ernsthaft produktiv nutzen.

Bei Fragen oder Ideen: [GitHub-Issues oder direkt an mich]

---

## Hinweise für Entwickler

- Der Code ist in C# (.NET) geschrieben
- Der Einstiegspunkt ist `Program.Main`
- Die `SessionEditor`- und `NppAutomation`-Hilfsklassen kapseln interne Logik (z. B. Setzen des aktiven Tabs)
- `.lnk`-Verknüpfungen können zum Testen verwendet werden – Pfade ggf. manuell anpassen
- Im **Debug-Modus** werden keine Registry-Werte verwendet – stattdessen werden vordefinierte Entwicklungswerte genutzt

Pull Requests & Feature Requests sind immer herzlich willkommen! 🎉

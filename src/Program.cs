using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace NotepadPlusPlusLauncher
{
    class Program
    {
        [STAThread]
        static async Task Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string registryBase = "HKEY_CURRENT_USER\\Software\\NotepadPlusPlusLauncher";
            string sessionExtension = ".npp";
            string defaultDefaultSession = "default";
            string defaultDefaultSessionName = defaultDefaultSession + sessionExtension;

#if DEBUG
            string settingsXmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Notepad++\\plugins\\config\\SessionMgr\\settings.xml");
            string notepadExePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Notepad++\\notepad++_original.exe");
#else
            string settingsXmlPath = (string)Registry.GetValue(registryBase, "SettingsXmlPath", null);
            string notepadExePath = (string)Registry.GetValue(registryBase, "NotepadExePath", null);

            if (string.IsNullOrWhiteSpace(settingsXmlPath) || !File.Exists(settingsXmlPath) ||
                string.IsNullOrWhiteSpace(notepadExePath) || !File.Exists(notepadExePath))
            {
                MessageBox.Show("Bitte wähle dein Notepad++-Installationsverzeichnis aus.", "Pfad erforderlich", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using var folderDialog = new FolderBrowserDialog();
                if (folderDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Abgebrochen.");
                    return;
                }

                var installDir = folderDialog.SelectedPath;
                var originalExe = Path.Combine(installDir, "notepad++.exe");
                var renamedExe = Path.Combine(installDir, "notepad++_original.exe");

                if (!File.Exists(originalExe))
                {
                    MessageBox.Show("notepad++.exe wurde im gewählten Verzeichnis nicht gefunden.");
                    return;
                }

                File.Move(originalExe, renamedExe);
                notepadExePath = renamedExe;

                string launcherExe = Process.GetCurrentProcess().MainModule?.FileName ?? "";
                foreach (var file in Directory.GetFiles(Path.GetDirectoryName(launcherExe)!))
                {
                    string target = Path.Combine(installDir, Path.GetFileName(file));
                    File.Copy(file, target, true);
                }

                using var openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Wähle die settings.xml von SessionMgr";
                openFileDialog.Filter = "XML Dateien (*.xml)|*.xml";
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Abgebrochen.");
                    return;
                }

                settingsXmlPath = openFileDialog.FileName;
                Registry.SetValue(registryBase, "NotepadExePath", notepadExePath);
                Registry.SetValue(registryBase, "SettingsXmlPath", settingsXmlPath);

                MessageBox.Show("Setup abgeschlossen. Du kannst jetzt wie gewohnt Notepad++ öffnen – alles läuft nun über den Launcher!", "Fertig", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
#endif

            if (args.Length < 1)
            {
                args = new[] { defaultDefaultSessionName };
            }
            else if (args.Length > 1)
            {
                Console.WriteLine("Zu viele Parameter übergeben. Es wird nur der erste Parameter verwendet.");
            }

            string inputPath = args[0];
            string xml = File.ReadAllText(settingsXmlPath);

            string sessionDirectory = xml.ReadXmlAttributeValue("sessionDirectory") ??
                throw new InvalidOperationException("sessionDirectory nicht gefunden.");

            string current = xml.ReadXmlAttributeValue("currentSession") ??
                throw new InvalidOperationException("currentSession nicht gefunden.");

            string startArgs = "";
            if (inputPath.EndsWith(sessionExtension, StringComparison.OrdinalIgnoreCase))
            {
                string newSession = Path.GetFileNameWithoutExtension(inputPath);
                xml = xml.ReplaceXmlAttributeValue("automaticLoad", "1") ?? throw new InvalidOperationException();
                xml = xml.ReplaceXmlAttributeValue("loadIntoCurrent", "0") ?? throw new InvalidOperationException();
                xml = xml.ReplaceXmlAttributeValue("previousSession", current) ?? throw new InvalidOperationException();
                xml = xml.ReplaceXmlAttributeValue("currentSession", newSession) ?? throw new InvalidOperationException();
                string cwd = Directory.GetCurrentDirectory();
                startArgs = $"-openFoldersAsWorkspace \"{cwd}\"";
            }
            else
            {
                xml = xml.ReplaceXmlAttributeValue("automaticLoad", "0") ?? throw new InvalidOperationException();
                xml = xml.ReplaceXmlAttributeValue("loadIntoCurrent", "1") ?? throw new InvalidOperationException();
                var defaultSession = xml.ReadXmlAttributeValue("defaultSession") ?? defaultDefaultSessionName;
                xml = xml.ReplaceXmlAttributeValue("previousSession", current) ?? throw new InvalidOperationException();
                xml = xml.ReplaceXmlAttributeValue("currentSession", defaultSession) ?? throw new InvalidOperationException();
                startArgs = $"\"{inputPath}\"";
            }

            File.WriteAllText(settingsXmlPath, xml);
            current = xml.ReadXmlAttributeValue("currentSession") ?? throw new InvalidOperationException();
            string sessionPath = Path.Combine(sessionDirectory, Path.GetFileNameWithoutExtension(current) + sessionExtension);
            SessionEditor.SetActiveFileInSession(sessionPath, inputPath);

            Process? process = null;
            try
            {
                process = Process.Start(new ProcessStartInfo
                {
                    FileName = notepadExePath,
                    Arguments = startArgs,
                    UseShellExecute = false
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Starten von Notepad++: {ex.Message}");
                return;
            }

            if (process == null)
            {
                Console.WriteLine("Notepad++ konnte nicht gestartet werden.");
                return;
            }

            try
            {
                await Task.Delay(1500);
                NppAutomation.SetProfile(process, current.FirstOrDefault().ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Setzen des Profils: {ex.Message}");
            }
        }
    }
}

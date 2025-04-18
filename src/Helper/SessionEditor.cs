using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Linq;

public static class SessionEditor
{
    public static void SetActiveFileInSession(string sessionPath, string filePath)
    {
        if (!File.Exists(sessionPath))
        {
            Console.WriteLine($"Sessiondatei nicht gefunden: {sessionPath}");
            return;
        }

        // if (!File.Exists(filePath))
        // {
        //     Console.WriteLine($"Ziel-Datei nicht gefunden: {filePath}");
        //     return;
        // }

        try
        {
            // 🔁 EncodingProvider für Codepage 1252 registrieren
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding1252 = Encoding.GetEncoding(1252);

            // 🔁 Datei direkt in Speicher laden, Änderungen vornehmen
            string xmlRaw = File.ReadAllText(sessionPath, encoding1252);
            var doc = XDocument.Parse(xmlRaw, LoadOptions.PreserveWhitespace); // var doc = XDocument.Load(sessionPath);

            var mainView = doc.Descendants("mainView").FirstOrDefault();
            if (mainView == null)
            {
                Console.WriteLine("mainView nicht gefunden.");
                return;
            }

            var files = mainView.Elements("File").ToList();

            // Datei suchen (Pfadvergleich kann optional case-insensitive sein)
            var index = files.FindIndex(f =>
            {
                var attr = f.Attribute("filename")?.Value;
                return string.Equals(attr, filePath, StringComparison.OrdinalIgnoreCase);
            });

            if (index == -1)
            {
                Console.WriteLine("Datei nicht in Session-Datei gefunden.");
                return;
            }

            mainView.SetAttributeValue("activeIndex", index.ToString());
            // 🔒 Alles zurück in Text – UTF-8 Header bleibt wie er ist
            string result =doc.Declaration?.ToString() + doc.ToString(SaveOptions.DisableFormatting);

            // 💾 Physisch speichern mit Codepage 1252 – aber Header bleibt UTF-8!
            File.WriteAllText(sessionPath, result, encoding1252);

            // // ⚠️ Speichern mit ANSI (Codepage 1252)
            // var encoding = Encoding.GetEncoding(1252); // Standard-Encoding (GetEncoding(1252))
            // var settings = new XmlWriterSettings
            // {
            //     Encoding = encoding,
            //     Indent = true,
            //     OmitXmlDeclaration = false
            // };

            // using (var writer = XmlWriter.Create(sessionPath, settings))
            // {
            //     doc.Save(writer);
            // }

            // doc = null;
            // GC.Collect();

            // // Xml Deklaration anpassen
            // var fileLines = File.ReadLines(sessionPath, encoding).Skip(1); // Erste Zeile (XML-Deklaration) überspringen
            // var xmlDeclaration = $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>"; // Erste Zeile (XML-Deklaration) erstellen
            // var newLines = new[] { xmlDeclaration }.Concat(fileLines);
            // File.WriteAllLines(sessionPath, newLines, encoding);

            Console.WriteLine($"activeIndex auf {index} gesetzt für Datei: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Bearbeiten der Session-Datei: {ex.Message}");
        }
    }
}

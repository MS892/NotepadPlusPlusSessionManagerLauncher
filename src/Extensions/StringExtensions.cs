using System;
using System.Text.RegularExpressions;

public static class StringExtensions
{

    public static string? ReadXmlAttributeValue(this string xml, string tagName, string attributeName = "value")
    {
        // Regex für den Tag und das Attribut
        string pattern = $@"<{tagName}\s+{attributeName}\s*=""([^""]+)""\s*/>";
        var match = Regex.Match(xml, pattern);
        if (!match.Success)
        {
            Console.WriteLine($"Konnte {tagName} mit Attribut {attributeName} nicht finden.");
            return null;
        }

        return match.Groups[1].Value;
    }
    public static string? ReplaceXmlAttributeValue(this string xml, string tagName, string newValue, string attributeName = "value")
    {
        // Regex Replace für den Tag und das Attribut
        xml = Regex.Replace(xml, $@"<{tagName}\s+{attributeName}\s*=""([^""]+)""\s*/>",
                                 $"<{tagName} {attributeName}=\"{newValue}\"/>");

        // Überprüfen, ob der Ersetzungsprozess erfolgreich war
        if (string.IsNullOrEmpty(xml))
        {
            Console.WriteLine($"Konnte {tagName} mit Attribut {attributeName} nicht ersetzen.");
            return null;
        }

        return xml;
    }
    
    public static string? ReplaceXmlAttributeValue(this string xml, string tagName, string newValue, out string oldValue, string attributeName = "value")
    {
        // Aktuellen Wert von <tagName attributeName=""/> auslesen
        oldValue = xml.ReadXmlAttributeValue(tagName, attributeName) ??
            throw new InvalidOperationException($"Konnte {tagName} mit Attribut {attributeName} nicht finden.");

        // Regex Replace für den Tag und das Attribut
        xml = Regex.Replace(xml, $@"<{tagName}\s+{attributeName}\s*=""([^""]+)""\s*/>",
                                 $"<{tagName} {attributeName}=\"{newValue}\"/>");

        // Überprüfen, ob der Ersetzungsprozess erfolgreich war
        if (string.IsNullOrEmpty(xml))
        {
            Console.WriteLine($"Konnte {tagName} mit Attribut {attributeName} nicht ersetzen.");
            return null;
        }

        return xml;
    }
}

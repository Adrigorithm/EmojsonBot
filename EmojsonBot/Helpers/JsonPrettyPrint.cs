using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmojsonBot.Helpers;

internal static class JsonPrettyPrint
{
    public static string JsonPP(string json)
    {
        using (var stringReader = new StringReader(json))
        using (var stringWriter = new StringWriter())
        {
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }

    public static void WriteFile(string fileName, JObject json)
    {
        using (StreamWriter file = File.CreateText(fileName))
        using (JsonTextWriter writer = new JsonTextWriter(file) { Formatting = Formatting.Indented })
        {
            json.WriteTo(writer);
        }
    }

}
using Microsoft.Xna.Framework.Content.Pipeline;
using System.ComponentModel;
using System.IO;

[ContentImporter(".json", DisplayName = "JSON Importer - AmpBoi", DefaultProcessor = nameof(JSONMapProcessor))]
public class JSONMapImporter : ContentImporter<string>
{
    public override string Import(string filename, ContentImporterContext context)
    {
        // Read the raw CSV file and return the content as a string.
        // You can also preprocess the data here if needed.
        return File.ReadAllText(filename);
    }
}

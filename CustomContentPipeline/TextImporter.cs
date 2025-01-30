using Microsoft.Xna.Framework.Content.Pipeline;
using System.IO;

[ContentImporter(".txt", DisplayName = "Text Importer - AmpBoi", DefaultProcessor = nameof(TextProcessor))]
public class TextImporter : ContentImporter<string>
{
    public override string Import(string filename, ContentImporterContext context)
    {
        // Read the raw CSV file and return the content as a string.
        // You can also preprocess the data here if needed.
        return File.ReadAllText(filename);
    }
}

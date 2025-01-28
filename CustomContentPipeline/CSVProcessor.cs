using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;

[ContentProcessor(DisplayName = "CSV Processor - AmpBoi")]
public class CsvProcessor : ContentProcessor<string, Dictionary<Vector2, string>>
{
    public override Dictionary<Vector2, string> Process(string input, ContentProcessorContext context)
    {
        // Create the dictionary to store the parsed data.
        Dictionary<Vector2, string> dictionary = new Dictionary<Vector2, string>();

        // Split the input string into lines.
        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        // Iterate over each row.
        for (int y = 0; y < lines.Length; y++)
        {
            // Split the line by commas to get individual values (columns).
            var columns = lines[y].Split(',');

            // Iterate over each column (this is the x index).
            for (int x = 0; x < columns.Length; x++)
            {
                if (String.IsNullOrEmpty(columns[x]))
                {
                    continue;
                }
                // Add the parsed data to the dictionary with Vector2 as the key (x, y).
                dictionary[new Vector2(x, y)] = columns[x].Trim();
            }
        }

        return dictionary;
    }
}

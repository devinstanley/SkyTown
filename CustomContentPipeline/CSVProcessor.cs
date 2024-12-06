using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using System;
using System.Collections.Generic;
using System.Linq;

[ContentProcessor(DisplayName = "CSV Processor - AmpBoi")]
public class CsvProcessor : ContentProcessor<string, Dictionary<Vector2, int>>
{
    public override Dictionary<Vector2, int> Process(string input, ContentProcessorContext context)
    {
        // Create the dictionary to store the parsed data.
        Dictionary<Vector2, int> dictionary = new Dictionary<Vector2, int>();

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
                // Parse the value in the CSV cell.
                if (int.TryParse(columns[x], out int value))
                {
                    // Add the parsed data to the dictionary with Vector2 as the key (x, y).
                    dictionary[new Vector2(x, y)] = value;
                }
                else
                {
                    // Handle any parsing errors (e.g., log or handle as needed).
                    context.Logger.LogImportantMessage($"Error parsing value '{columns[x]}' at ({x}, {y})");
                }
            }
        }

        return dictionary;
    }
}

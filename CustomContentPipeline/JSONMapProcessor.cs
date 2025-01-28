using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;

[ContentProcessor(DisplayName = "JSON Processor - AmpBoi")]
//One day I will add a message library to pass tiles, but today is not that day...
public class JSONMapProcessor : ContentProcessor<string, Dictionary<string, List<int>>>
{
    public override Dictionary<string, List<int>> Process(string input, ContentProcessorContext context)
    {
        var result = new Dictionary<string, List<int>>();

        var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            line.Trim().Trim('"');

            // Split the line into key and value
            var parts = line.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) continue;

            string key = parts[0].Trim().Trim('"'); // Remove quotes from the key
            string value = parts[1].Replace("(", "").Replace(")", "");

            int mos = 0;
            result[key] = value.Split(',')
                    .Select(m => { int.TryParse(m, out mos); return mos; })
                    .ToList();
        }


        return result;
    }
}

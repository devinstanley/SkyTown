using Microsoft.Xna.Framework.Content.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;

[ContentProcessor(DisplayName = "Text Processor - AmpBoi")]
//One day I will add a message library to pass tiles, but today is not that day...
public class TextProcessor : ContentProcessor<string, string>
{
    public override string Process(string input, ContentProcessorContext context)
    {
        return input;
    }
}

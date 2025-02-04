using SkyTown.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SkyTown.Parsing
{
    //Parses TextureID and Animation Info From Any Object
    public class IAnimatorConverter : JsonConverter<IAnimator>
    {
        public override IAnimator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Read the parent JsonElement
            var parentElement = JsonDocument.ParseValue(ref reader).RootElement;

            // We now have the parent, so get the TextureID from it
            string textureID = parentElement.GetProperty("TextureID").GetString();

            // Now handle the IAnimator deserialization based on the type in the JSON
            if (parentElement.TryGetProperty("IAnimator", out JsonElement animatorElement))
            {
                if (animatorElement.ValueKind == JsonValueKind.Array)
                {
                    // Handle static animation: Convert a single int[4] array into a Rectangle
                    var rect = new Rectangle(
                        animatorElement[0].GetInt32(), // X
                        animatorElement[1].GetInt32(), // Y
                        animatorElement[2].GetInt32(), // Width
                        animatorElement[3].GetInt32()  // Height
                    );
                    return new Animation(textureID, 0, new List<Rectangle> { rect }); // Static animation with frame time 0
                }
                else if (animatorElement.ValueKind == JsonValueKind.Object)
                {
                    // More complex sequence
                    if (animatorElement.TryGetProperty("FrameTime", out JsonElement frameTimeElement) &&
                        animatorElement.TryGetProperty("SourceRectangles", out JsonElement rectElements))
                    {
                        double frameTime = frameTimeElement.GetDouble();
                        List<Rectangle> frames = new List<Rectangle>();
                        foreach (var item in rectElements.EnumerateArray())
                        {
                            frames.Add(new Rectangle(
                                item[0].GetInt32(), // X
                                item[1].GetInt32(), // Y
                                item[2].GetInt32(), // Width
                                item[3].GetInt32()  // Height
                            ));
                        }
                       
                        return new Animation(textureID, frameTime, frames);
                    }
                }
            }

            throw new JsonException("Unknown IAnimator type.");
        }

        public override void Write(Utf8JsonWriter writer, IAnimator value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}

using SkyTown.Entities.Interfaces;
using SkyTown.Map;
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
    public class TileDictionaryConverter : JsonConverter<Dictionary<string, BaseTile>>
    {
        public override Dictionary<string, BaseTile> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var tiles = new Dictionary<string, BaseTile>();

            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                foreach (JsonElement element in doc.RootElement.EnumerateArray())
                {
                    // Extract base properties
                    string objectID = element.GetProperty("ObjectID").GetString();
                    string textureID = element.GetProperty("TextureID").GetString();
                    string fullID = $"{textureID}::{objectID}";

                    // Deserialize CollisionRectangle if present
                    Rectangle? collisionRectangle = null;
                    if (element.TryGetProperty("CollisionRectangle", out JsonElement collisionElement) &&
                        collisionElement.ValueKind == JsonValueKind.Array &&
                        collisionElement.GetArrayLength() == 4)
                    {
                        int[] rectValues = JsonSerializer.Deserialize<int[]>(collisionElement.GetRawText(), options);
                        collisionRectangle = new Rectangle(rectValues[0], rectValues[1], rectValues[2], rectValues[3]);
                    }

                    // Deserialize IAnimator (using existing logic)
                    IAnimator animator = null;
                    if (element.TryGetProperty("IAnimator", out JsonElement animatorElement))
                    {
                        animator = JsonSerializer.Deserialize<IAnimator>(element.GetRawText(), options);
                    }

                    // Create the tile object
                    BaseTile tile = new BaseTile(fullID, animator, collisionRectangle);

                    // Store it in the dictionary
                    tiles[objectID] = tile;
                }
            }

            return tiles;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<string, BaseTile> value, JsonSerializerOptions options)
        {
            throw new NotImplementedException(); // Only implementing deserialization for now
        }
    }
}

using SharpDX.D3DCompiler;
using SharpDX;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SkyTown.Entities.Interfaces;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace SkyTown.Logic
{
    public static class ObjectParser
    {
        //Formats
        //Regular Tile - Static Single Frame Animation
        //path_to_png::id {Rect: (x, y, w, h)}
        //path_to_png::id {Rect: (x, y, w, h), CollisionRect: (x, y, w, h)}
        //Regular Tile - Multi-Frame Animation
        //path_to_png::id {Rects: [<frame_time> (x, y, w, h), (x, y, w, h)]}
        //path_to_png::id {Rects: [<frame_time> (x, y, w, h), (x, y, w, h)], CollisionRect: (x, y, w, h)}
        public static KeyValuePair<string, BaseTile> ParseTileLine(string input)
        {
            // Extract TextureID and TileID
            string[] parts = input.Split(new[] { "::" }, StringSplitOptions.None);
            if (parts.Length < 2) throw new FormatException("Invalid tile format: Missing '::'");

            string TextureID = parts[0].Trim('"');
            string TileID = parts[1].Split('{')[0].Trim('"', ' ', ':');
            string FullID = TextureID + "::" + TileID;

            // Extract content inside { ... }
            string content = input.Substring(input.IndexOf('{')).Trim();

            // Collision Rectangle (optional)
            Rectangle? CollisionRectangle = null;
            Match collisionMatch = Regex.Match(content, @"CollisionRect:\s*\((\d+),\s*(\d+),\s*(\d+),\s*(\d+)\)");
            if (collisionMatch.Success)
            {
                CollisionRectangle = new Rectangle(
                    int.Parse(collisionMatch.Groups[1].Value),
                    int.Parse(collisionMatch.Groups[2].Value),
                    int.Parse(collisionMatch.Groups[3].Value),
                    int.Parse(collisionMatch.Groups[4].Value)
                );
            }

            // Check for animation frames
            Match animationMatch = Regex.Match(content, @"Rects:\s*\[([\d\.]+),((?:\s*\(\d+,\s*\d+,\s*\d+,\s*\d+\),?)+)\]");
            if (animationMatch.Success)
            {
                // Parse frame time
                float frameTime = float.Parse(animationMatch.Groups[1].Value);
                List<Rectangle> animationFrames = new List<Rectangle>();

                // Parse all animation frame rectangles
                MatchCollection frameMatches = Regex.Matches(animationMatch.Groups[2].Value, @"\((\d+),\s*(\d+),\s*(\d+),\s*(\d+)\)");
                foreach (Match match in frameMatches)
                {
                    animationFrames.Add(new Rectangle(
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value),
                        int.Parse(match.Groups[4].Value)
                    ));
                }

                Animation animation = new Animation(TextureID, frameTime, animationFrames);
                return new KeyValuePair<string, BaseTile>(TileID, new BaseTile(FullID, animation, CollisionRectangle));
            }

            // Otherwise, check for a static single-frame tile
            Match staticMatch = Regex.Match(content, @"Rect:\s*\((\d+),\s*(\d+),\s*(\d+),\s*(\d+)\)");
            if (staticMatch.Success)
            {
                Rectangle sourceRect = new Rectangle(
                    int.Parse(staticMatch.Groups[1].Value),
                    int.Parse(staticMatch.Groups[2].Value),
                    int.Parse(staticMatch.Groups[3].Value),
                    int.Parse(staticMatch.Groups[4].Value)
                );
                Animation animation = new Animation(TextureID, 1, new List<Rectangle>([ sourceRect ]));
                return new KeyValuePair<string, BaseTile>(TileID, new BaseTile(FullID, animation, CollisionRectangle));
            }

            throw new FormatException("Invalid tile format: Missing Rects or Rect");
        }

        public static Dictionary<string, BaseTile> ParseTileManifest(string input)
        {
            var result = new Dictionary<string, BaseTile>();
            foreach (string line in input.Split(new char[] { '\r', '\n' }))
            {
                if (String.IsNullOrWhiteSpace(line))
                {
                    continue;   
                }
                else
                {
                    try
                    {
                        KeyValuePair<string, BaseTile> temp = ParseTileLine(line);
                        result.Add(temp.Key, temp.Value);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed to Parse Line: ", line);
                    }
                }
            }
            return result;
        }
    }
}

using Microsoft.Xna.Framework;
using SkyTown.Entities.GameObjects.Items;
using SkyTown.Entities.Interfaces;
using SkyTown.Map;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;


namespace SkyTown.Logic
{
    public static class ObjectParser
    {
        public class ParsedLine
        {
            public string TextureID { get; set; }
            public string ObjectID { get; set; }
            public string FullID { get; set; }
            public string Content { get; set; }
        }
        public static ParsedLine ParseLine(string input)
        {
            // Extract TextureID and TileID
            string[] parts = input.Split(new[] { "::" }, StringSplitOptions.None);
            if (parts.Length < 2) throw new FormatException("Invalid tile format: Missing '::'");

            string TextureID = parts[0].Trim('"');
            string TileID = parts[1].Split('{')[0].Trim('"', ' ', ':');
            string FullID = TextureID + "::" + TileID;

            // Extract content inside { ... }
            string content = input.Substring(input.IndexOf('{')).Trim();

            return new ParsedLine
            {
                TextureID = TextureID,
                ObjectID = TileID,
                FullID = FullID,
                Content = content
            };
        }

        public static Rectangle? ParseCollisionRectangle(string input)
        {
            Rectangle? CollisionRectangle = null;
            Match collisionMatch = Regex.Match(input, @"CollisionRect:\s*\((\d+),\s*(\d+),\s*(\d+),\s*(\d+)\)");
            if (collisionMatch.Success)
            {
                CollisionRectangle = new Rectangle(
                    int.Parse(collisionMatch.Groups[1].Value),
                    int.Parse(collisionMatch.Groups[2].Value),
                    int.Parse(collisionMatch.Groups[3].Value),
                    int.Parse(collisionMatch.Groups[4].Value)
                );
            }

            return CollisionRectangle;
        }

        public static IAnimator ParseAnimationInformation(string input, string textureID)
        {
            Match animationMatch = Regex.Match(input, @"Rects:\s*\[([\d\.]+),((?:\s*\(\d+,\s*\d+,\s*\d+,\s*\d+\),?)+)\]");
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
                return new Animation(textureID, frameTime, animationFrames);
            }
            // Otherwise, check for a static single-frame tile
            Match staticMatch = Regex.Match(input, @"Rect:\s*\((\d+),\s*(\d+),\s*(\d+),\s*(\d+)\)");
            if (staticMatch.Success)
            {
                Rectangle sourceRect = new Rectangle(
                    int.Parse(staticMatch.Groups[1].Value),
                    int.Parse(staticMatch.Groups[2].Value),
                    int.Parse(staticMatch.Groups[3].Value),
                    int.Parse(staticMatch.Groups[4].Value)
                );
                Animation animation = new Animation(textureID, 1, new List<Rectangle>([sourceRect]));
                return animation;
            }
            return null;
        }

        //Formats
        //Regular Tile - Static Single Frame Animation
        //path_to_png::id {Rect: (x, y, w, h)}
        //path_to_png::id {Rect: (x, y, w, h), CollisionRect: (x, y, w, h)}
        //Regular Tile - Multi-Frame Animation
        //path_to_png::id {Rects: [<frame_time> (x, y, w, h), (x, y, w, h)]}
        //path_to_png::id {Rects: [<frame_time> (x, y, w, h), (x, y, w, h)], CollisionRect: (x, y, w, h)}
        public static KeyValuePair<string, BaseTile> ParseTileLine(string input)
        {
            ParsedLine parsedLine = ParseLine(input);

            // Collision Rectangle (optional)
            Rectangle? CollisionRectangle = ParseCollisionRectangle(parsedLine.Content);

            IAnimator animator = ParseAnimationInformation(parsedLine.Content, parsedLine.TextureID);
            return new KeyValuePair<string, BaseTile>(parsedLine.ObjectID, new BaseTile(parsedLine.FullID, animator, CollisionRectangle));
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

        public static KeyValuePair<string, ItemConstructor> ParseItemLine(string input)
        {
            ParsedLine parsedLine = ParseLine(input);

            IAnimator animator = ParseAnimationInformation(parsedLine.Content, parsedLine.TextureID);

            return new KeyValuePair<string, ItemConstructor>(parsedLine.ObjectID, new ItemConstructor(parsedLine.FullID, animator, 1));
        }

        public static Dictionary<string, ItemConstructor> ParseItemManifest(string input)
        {

            var result = new Dictionary<string, ItemConstructor>();
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
                        KeyValuePair<string, ItemConstructor> temp = ParseItemLine(line);
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

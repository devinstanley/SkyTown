using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SkyTown.Map;
using System.Collections.Generic;

namespace SkyTown.Logic
{
    public static class ResourceManager
    {
        public static ContentManager content;
        static Dictionary<string, Texture2D> textureLibrary = new Dictionary<string, Texture2D>();
        static Dictionary<string, Song> songLibrary = new Dictionary<string, Song>();
        static Dictionary<string, SpriteFont> fontLibrary = new Dictionary<string, SpriteFont>();

        public static Song LoadSong(string songID)
        {
            if (!songLibrary.ContainsKey(songID))
            {
                string songPath = string.Join("\\", songID.Split("."));
                Song song = content.Load<Song>(songPath);
                songLibrary.Add(songID, song);
            }

            return songLibrary[songID];
        }
        public static Texture2D LoadTexture(string textureID)
        {
            if (!textureLibrary.ContainsKey(textureID))
            {
                string texturePath = string.Join("\\", textureID.Split("."));
                Texture2D texture = content.Load<Texture2D>(texturePath);
                textureLibrary.Add(textureID, texture);
                //referenceCount.Add(textureName, 1);  // First reference
            }
            else
            {
                //referenceCount[textureName]++;
            }

            return textureLibrary[textureID];
        }

        public static SpriteFont LoadFont(string fontID)
        {
            if (!fontLibrary.ContainsKey(fontID))
            {
                string fontPath = string.Join("\\", fontID.Split("."));
                SpriteFont font = content.Load<SpriteFont>(fontPath);
                fontLibrary.Add(fontID, font);
            }
            else
            {
            }

            return fontLibrary[fontID];
        }

        public static Dictionary<string, Tile> LoadTiles(string manifestID)
        {
            Dictionary<string, Tile> result = new Dictionary<string, Tile>();
            var tiles = content.Load<Dictionary<string, List<int>>>(manifestID);

            foreach (var pair in tiles)
            {
                Rectangle rect1;
                Rectangle? rect2 = null;
                if (pair.Value.Count < 4)
                {
                    continue;
                }
                rect1 = new Rectangle(pair.Value[0], pair.Value[1], pair.Value[2], pair.Value[3]);
                if (pair.Value.Count == 8)
                {
                    rect2 = new Rectangle(pair.Value[4], pair.Value[5], pair.Value[6], pair.Value[7]);
                }
                result.Add(pair.Key.Split('.')[1], new Tile(pair.Key, rect1, rect2));
            }
            return result;
        }
    }
}

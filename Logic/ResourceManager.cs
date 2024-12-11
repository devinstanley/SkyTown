using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SkyTown.Logic
{
    public static class ResourceManager
    {
        public static ContentManager content;
        static Dictionary<string, Texture2D> textureLibrary = new Dictionary<string, Texture2D>();
        static Dictionary<string, Song> songLibrary = new Dictionary<string, Song>();
        
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
    }
}

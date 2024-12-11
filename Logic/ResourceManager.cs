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
        
        public static Song LoadSong(string songName)
        {
            if (!songLibrary.ContainsKey(songName))
            {
                Song song = content.Load<Song>(songName);
                songLibrary.Add(songName, song);
            }

            return songLibrary[songName];
        }
        public static Texture2D LoadTexture(string textureName)
        {
            if (!textureLibrary.ContainsKey(textureName))
            {
                Texture2D texture = content.Load<Texture2D>(textureName);
                textureLibrary.Add(textureName, texture);
                //referenceCount.Add(textureName, 1);  // First reference
            }
            else
            {
                //referenceCount[textureName]++;
            }

            return textureLibrary[textureName];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SkyTown.Logic
{
    internal class ResourceManager
    {
        ContentManager content;
        Dictionary<string, Texture2D> textureLibrary = new Dictionary<string, Texture2D>();
        public ResourceManager(ContentManager content)
        {
            this.content = content;
        }
        public Texture2D LoadTexture(string textureName)
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

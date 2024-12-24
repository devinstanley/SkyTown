using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.Entities.Characters
{
    public class NPCManager
    {
        public List<NPC> NPCs = new List<NPC>();

        public NPCManager()
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            foreach (NPC n in NPCs)
            {
                n.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (NPC n in NPCs)
            {
                n.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (NPC n in NPCs)
            {
                n.Draw(spriteBatch);
            }
        }

        public void Add(NPC npc)
        {
            NPCs.Add(npc);
        }

        public void Remove(NPC npc)
        {
            NPCs.Remove(npc);
        }
    }
}

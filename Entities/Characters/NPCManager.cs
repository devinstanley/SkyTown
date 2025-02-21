using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SkyTown.Entities.Characters
{
    public class NPCManager
    {
        public List<NPC> NPCs = new List<NPC>();

        public NPCManager()
        {

        }

        public void Update()
        {
            foreach (NPC n in NPCs)
            {
                n.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
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

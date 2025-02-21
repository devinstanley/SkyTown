using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.Interfaces;
using SkyTown.HUD;
using SkyTown.LogicManagers;
using SkyTown.Map;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Entities.Characters
{
    public class NPC : GameObject, IInteractor
    {
        public string Name { get; set; }
        public Dialogue Dialogue { get; set; }

        public NPC(string id) : base(id)
        {

        }

        new public void Update(InputManager inputManager)
        {
            base.Update();
            if (Dialogue.DialogueRunning)
            {
                Dialogue.Update(inputManager);
            }
        }

        public void Interact(Player player, MapScene mapScene)
        {
            Dialogue.StartDialogue();
        }


        new public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (Dialogue.DialogueRunning)
            {
                Dialogue.Draw(spriteBatch);
            }
        }
    }
}

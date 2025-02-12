using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.HUD
{
    public class Dialogue
    {
        Texture2D DialogueTexture { get => ResourceManager.LoadTexture("Assets.HUDs.Dialogue"); }
        Vector2 DialogueStartLoc = new(92, 7);
        Vector2 PortraitStartLoc = new(24, 7);
        Vector2 CursorPosition = new();
        AnimationManager Portrait;
        public bool DialogueRunning = false;
        public Dialogue(string npcID)
        {

        }

        public void StartDialogue()
        {
            DialogueRunning = true;
        }

        public void Update()
        {
            if (!DialogueRunning)
            {
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DialogueRunning)
            {
                return;
            }
            Vector2 pos = new Vector2(
                
                );
            spriteBatch.Draw(DialogueTexture, pos, new Color());
            Portrait.Draw(spriteBatch, pos + PortraitStartLoc);
        }
    }
}

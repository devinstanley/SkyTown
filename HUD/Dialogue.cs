using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Interfaces;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyTown.HUD
{
    public class Dialogue
    {
        Texture2D DialogueTexture { get => ResourceManager.LoadTexture("Assets.HUDs.DialogueHUD"); }
        Vector2 DialogueStartLoc = new(76, 8);
        Vector2 PortraitStartLoc = new(8, 8);
        Vector2 CursorPosition = new();
        AnimationManager Portrait;
        public bool DialogueRunning = true;
        public Dialogue(string NPC_ID)
        {
            Portrait = new AnimationManager();
            Portrait.AddAnimation(0, new Animation("Assets.Sprites.NPCs.Blurg_Portrait", 1, new List<Rectangle>([
                new Rectangle(0, 0, 64, 64),
                new Rectangle(64, 0, 64, 64)
                ]))
            );
        }

        public void StartDialogue()
        {
            DialogueRunning = true;
        }

        public void Update(InputManager inputManager)
        {
            if (!DialogueRunning)
            {
                return;
            }
            Portrait.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!DialogueRunning)
            {
                return;
            }
            Vector2 hudPos = new Vector2(
                Camera.Position.X,
                Camera.Position.Y + Camera.ResolutionHeight/2 - DialogueTexture.Height/2);
            spriteBatch.Draw(
                DialogueTexture,
                hudPos,
                null,
                Color.White,
                0f,
                new Vector2(DialogueTexture.Width / 2, DialogueTexture.Height / 2),
                1f, SpriteEffects.None, 0f);
            Vector2 portraitPos = new Vector2(
                hudPos.X - DialogueTexture.Width / 2 + PortraitStartLoc.X + Portrait.Width/2,
                hudPos.Y - DialogueTexture.Height/2 + PortraitStartLoc.Y + Portrait.Height/2);
            Portrait.Draw(spriteBatch, portraitPos);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.GameObjects;
using SkyTown.Entities.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Entities.Characters
{
    public class NPC : GameObject, IInteractor
    {
        public string Name { get; set; }

        public NPC(string id) : base(id)
        {
            List<string> parsedID = new List<string>(id.Split(','));
            Name = parsedID.Last();
        }

        new public void LoadContent(ContentManager content)
        {

        }

        new public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void Interact(Player player)
        {

        }


        new public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}

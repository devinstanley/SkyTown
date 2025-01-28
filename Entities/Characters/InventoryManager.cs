using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyTown.Entities.Characters
{
    public class InventoryManager
    {
        public static int INVENTORYWIDTH = 9;
        public static int INVENTORYHEIGHT = 4;
        public static int MAXSLOTS = INVENTORYHEIGHT * INVENTORYWIDTH;

        public Dictionary<int, InventorySlot> Items { get; private set; }

        public int CurrentItemKey { get; set; } = -1;
        public Item CurrentItem
        {
            get
            {
                if (Items.ContainsKey(CurrentItemKey))
                {
                    return Items[CurrentItemKey].Item;
                }
                else
                {
                    return null;
                }
            }
        }
        public InventoryManager()
        {
            Items = new Dictionary<int, InventorySlot>();
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var item in Items)
            {
                item.Value.Item.LoadContent(content);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var item in Items)
            {
                item.Value.Update(gameTime);
            }
        }

        public void Sort()
        {

        }

        public void SwapOrStack(int key1, int key2)
        {
            //Make sure keys are valid and we are not swapping with self
            if (key1 < 0 || key2 < 0 || key1 >= MAXSLOTS || key2 >= MAXSLOTS || key1 == key2)
            {
                return;
            }

            //Case 1: Both keys are present
            if (Items.Keys.Contains(key1) && Items.Keys.Contains(key2))
            {

                // Case 1a: Both keys point to the same item -> Stack
                if (Items[key1].Item.ID == Items[key2].Item.ID)
                {
                    while (Items[key2].AddedQuantity())
                    {
                        Items[key1].Quantiy -= 1;
                        if (Items[key1].Quantiy == 0)
                        {
                            Items.Remove(key1);
                            return;
                        }
                    }
                }
                // Case 1b: Keys point to different items -> Swap
                else
                {
                    var temp = Items[key1];
                    Items[key1] = Items[key2];
                    Items[key2] = temp;
                    return;
                }
            }
            if (Items.Keys.Contains(key1) && !Items.Keys.Contains(key2))
            {
                // Case 2: key1 is present, key2 is not
                Items[key2] = Items[key1]; // Add key2 with the value from key1
                Items.Remove(key1);       // Remove key1
                return;
            }
            if (!Items.Keys.Contains(key1) && Items.Keys.Contains(key2))
            {
                // Case 3: key2 is present, key1 is not
                Items[key1] = Items[key2]; // Add key1 with the value from key2
                Items.Remove(key2);        // Remove key2
                return;
            }
        }

        public void SplitStack(int itemKey)
        {
            if (!Items.Keys.Contains(itemKey))
            {
                return;
            }
            for (int i = 0; i < MAXSLOTS; i++)
            {
                if (!Items.ContainsKey(i))
                {
                    int newStackCount = (int)Math.Floor(Items[itemKey].Quantiy / 2d);
                    int curStackCount = (int)Math.Ceiling(Items[itemKey].Quantiy / 2d);

                    Items[itemKey].Quantiy = curStackCount;
                    if (newStackCount > 0)
                    {
                        Items.Add(i, new InventorySlot(Items[itemKey].Item, newStackCount));
                    }
                    return;
                }
            }
        }

        public void AddItem(Item item)
        {
            //First check if item can be stacked into existing slot
            foreach (var slot in Items.Where(u => u.Value.Item.ID.Equals(item.ID)).Select(u => u.Value))
            {
                if (slot.AddedQuantity())
                {
                    return;
                }
            }

            //Next, try to add to hotbar
            for (int i = INVENTORYWIDTH * (INVENTORYHEIGHT - 1); i < MAXSLOTS; i++)
            {
                if (!Items.ContainsKey(i))
                {
                    Items.Add(i, new InventorySlot(item));
                    return;
                }
            }

            //Lastly, try to add to inventory
            for (int i = 0; i < INVENTORYWIDTH * (INVENTORYHEIGHT - 1); i++)
            {
                if (!Items.ContainsKey(i))
                {
                    Items.Add(i, new InventorySlot(item));
                    return;
                }
            }
        }

        public int GetItemAtKey(int key)
        {
            return Items.Where(u => u.Key.Equals(key)).Select(u => u.Key).FirstOrDefault(-1);
        }

        public void RemoveItem(int slot)
        {
            if (Items.ContainsKey(slot))
            {
                if (Items[slot].Quantiy > 1)
                {
                    Items[slot].Quantiy -= 1;
                }
                else if (Items[slot].Quantiy == 1)
                {
                    RemoveStack(slot);
                }
            }
        }

        public void RemoveStack(int slot)
        {
            if (Items.ContainsKey(slot))
            {
                Items.Remove(slot);
            }
        }
    }

    public class InventorySlot
    {
        public Item Item { get; private set; }
        public int MaxQuantity { get; private set; }
        public int Quantiy { get; set; }

        public InventorySlot(Item item, int quantity = 1)
        {
            this.Item = item;
            this.Quantiy = quantity;
            this.MaxQuantity = item.MaxInventoryStack;
        }

        public bool AddedQuantity()
        {
            if (Quantiy < MaxQuantity)
            {
                Quantiy += 1;
                return true;
            }
            return false;
        }

        public void Update(GameTime gameTime)
        {
            Item.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
            Item.Draw(spriteBatch, pos, scale);
            string amt = Quantiy.ToString();
            SpriteFont font = ResourceManager.LoadFont("Assets.Font.Arial");
            Vector2 size = font.MeasureString(amt);
            spriteBatch.DrawString(
                font,
                amt,
                pos + new Vector2(16 * scale, 16 * scale) - size * scale,
                Color.White,
                0f,
                new Vector2(),
                scale * 1.2f,
                SpriteEffects.None,
                0f
                );
        }
    }
}

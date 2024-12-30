using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Items;
using SkyTown.HUD.Inventory;
using SkyTown.Logic;
using SkyTown.LogicManagers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SkyTown.Entities.Characters
{
    public class InventoryManager
    {
        public Dictionary<int, InventorySlot> Items { get; private set; }
        public int CurrentSelectedItem = -1; 
        public Item CurrentItem
        {
            get
            {
                if (Items.ContainsKey(CurrentSelectedItem))
                {
                    return Items[CurrentSelectedItem].Item;
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
                item.Value.Item.Update(gameTime);
            }
        }

        public void Sort()
        {

        }

        public void Swap(int key1, int key2)
        {
            if (Items.Keys.Contains(key1) && Items.Keys.Contains(key2))
            {
                // Case 1: Both keys are present, swap their values
                var temp = Items[key1];
                Items[key1] = Items[key2];
                Items[key2] = temp;
            }
            else if (Items.Keys.Contains(key1) && !Items.Keys.Contains(key2))
            {
                // Case 2: key1 is present, key2 is not
                Items[key2] = Items[key1]; // Add key2 with the value from key1
                Items.Remove(key1);       // Remove key1
            }
            else if (!Items.Keys.Contains(key1) && Items.Keys.Contains(key2))
            {
                // Case 3: key2 is present, key1 is not
                Items[key1] = Items[key2]; // Add key1 with the value from key2
                Items.Remove(key2);        // Remove key2
            }
            else
            {
                // Optional: Handle case where neither key is present (if needed)
                Debug.WriteLine("Neither key is present in the inventory.");
            }
        }

        public void SplitStack(int itemKey)
        {
            if (!Items.Keys.Contains(itemKey))
            {
                return;
            }
            for (int i = 0; i < InventoryHUD.MAXSLOTS; i++)
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
            foreach (var slot in Items.Where(u => u.Value.Item.ID.Equals(item.ID)).Select(u => u.Value))
            {
                if (slot.AddedQuantity())
                {
                    return;
                }
            }

            //Try to add to hotbar first
            for (int i = InventoryHUD.INVENTORYWIDTH * (InventoryHUD.INVENTORYHEIGHT-1); i < InventoryHUD.MAXSLOTS; i++)
            {
                if (!Items.ContainsKey(i))
                {
                    Items.Add(i, new InventorySlot(item));
                    return;
                }
            }

            //Try to add to rest of inventory
            for (int i = 0; i < InventoryHUD.INVENTORYWIDTH * (InventoryHUD.INVENTORYHEIGHT - 1); i++)
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
                //If it was the selected item, remove selection on empty slot
                if (slot == CurrentSelectedItem)
                {
                    CurrentSelectedItem = -1;   
                }
            }
        }
    }

    public class InventorySlot
    {
        public Item Item { get; private set; }
        public int MaxQuantity {  get; private set; }
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

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
            Item.Draw(spriteBatch, pos, scale);
            string amt = Quantiy.ToString();
            SpriteFont font = ResourceManager.LoadFont("Assets.Font.Arial");
            spriteBatch.DrawString(
                font,
                amt,
                pos + new Vector2(16*scale, 16 * scale - 1),
                Color.White,
                0f,
                new Vector2(0, 0),
                scale,
                SpriteEffects.None,
                0f
                );
        }
    }
}

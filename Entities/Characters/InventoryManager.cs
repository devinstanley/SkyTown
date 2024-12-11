using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SkyTown.Entities.Items;
using SkyTown.HUD.Inventory;
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

        public void AddItem(Item item)
        {
            foreach (var slot in Items.Where(u => u.Value.Item.ID.Equals(item.ID)).Select(u => u.Value))
            {
                if (slot.AddedQuantity())
                {
                    return;
                }
            }
            for (int i = 0; i < InventoryHUD.MAXSLOTS; i++)
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
                Items.Remove(slot);
            }
        }
    }

    public class InventorySlot
    {
        public Item Item { get; private set; }
        public int MaxQuantity {  get; private set; }
        public int Quantiy { get; private set; }

        public InventorySlot(Item item, int quantity = 1)
        {
            this.Item = item;
            this.Quantiy = quantity;
            this.MaxQuantity = item.MaxInventoryStack;
        }

        public bool AddedQuantity()
        {
            if (Quantiy <= MaxQuantity)
            {
                Quantiy += 1;
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 pos, float scale)
        {
            Item.Draw(spriteBatch, pos, scale);
        }
    }
}

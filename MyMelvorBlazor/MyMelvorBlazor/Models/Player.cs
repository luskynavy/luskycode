using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;

namespace MyMelvorBlazor.Models
{
	public static class Player
	{
		public static int Xp { get; set; } = 0;
		public static int Money { get; set; } = 0;
		public static int CookingLevel { get; set; } = 0;
		public static int FishingLevel { get; set; } = 0;
		public static int WoodcuttingLevel { get; set; } = 0;
		public static List<InventoryItemClass> Inventory { get; set; } = new List<InventoryItemClass>();

		public static void LoadValues()
		{
			Xp = 170;
			Money = 5000;
			CookingLevel = 5;
			FishingLevel = 4;
			WoodcuttingLevel = 3;
			Inventory = new List<InventoryItemClass> {
				new(ItemId.Wood, 1),
				new(ItemId.RawFish, 1),
				new(ItemId.Teak, 10),
				new(ItemId.Catfish, 2)
			};
		}

		//Add nb 'count' item of id 'idItem'
		public static void AddToInventory(ItemId idItem, int count)
		{
			//Search the item in inventory
			var foundInventory = Inventory.FirstOrDefault(i => i.Id == idItem);

			//Create the item if not found
			if (foundInventory == null)
			{
				var foundItem = ItemArray.Items.First(i => i.Id == idItem);
				if (foundItem != null)
				{
					Inventory.Add(new InventoryItemClass(idItem, count));
				}
			}
			//Update item count if found
			else
			{
				foundInventory.Count += count;

				//Remove item if quantity <= 0
				if (foundInventory.Count <= 0)
				{
					Inventory.Remove(foundInventory);
				}
			}
		}

		//Get item count in inventory
		public static int GetNbItemInInventory(ItemId idItem)
		{
			var foundInventory = Inventory.FirstOrDefault(i => i.Id == idItem);
			if (foundInventory == null)
			{
				return 0;
			}
			else
			{
				return foundInventory.Count;
			}
		}

		//True if has item in inventory
		public static bool HasItemInInventory(ItemId idItem)
		{
			return Inventory.FirstOrDefault(i => i.Id == idItem) != null;
		}

		//Sell nb 'count' item of id 'idItem'
		public static void SellItem(ItemId? idItem, int count)
		{
			if (idItem != null)
			{
				AddToInventory((ItemId)idItem, -count);
				Money += count * 100;
			}
		}
	}
}
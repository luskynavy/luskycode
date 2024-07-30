namespace MyMelvorBlazor.Models
{
	public class InventoryItemClass
	{
		public ItemId? Id { get; set; } = null;
		public string Name { get; set; } = "";
		public string Description { get; set; } = "";
		public int Count { get; set; } = 0;

		public InventoryItemClass(ItemId id, int count)
		{
			Id = id;
			Count = count;

			var foundItem = ItemArray.Items.First(i => i.Id == id);
			if (foundItem != null)
			{
				Name = foundItem.Name;
				Description = foundItem.Description;
			}
		}
	}
}
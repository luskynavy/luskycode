namespace MyMelvorBlazor.Models
{
	public class ItemClass
	{
		public ItemId? Id { get; set; } = null;
		public string Name { get; set; } = "";
		public string Description { get; set; } = "";

		public ItemClass(ItemId id, string name, string descritpion)
		{
			Id = id;
			Name = name;
			Description = descritpion;
		}
	}
}
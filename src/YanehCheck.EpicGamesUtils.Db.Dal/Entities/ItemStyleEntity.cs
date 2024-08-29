namespace YanehCheck.EpicGamesUtils.Db.Dal.Entities;

public class ItemStyleEntity : IEntity
{
    public required Guid Id { get; set; }
    public string FortniteId { get; set; }
    public string Channel { get; set; }
    public string Property { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public required string ItemFortniteId { get; set; }
    public ItemEntity? Item { get; set; }
}
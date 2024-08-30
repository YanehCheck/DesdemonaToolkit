using Microsoft.EntityFrameworkCore;

namespace YanehCheck.EpicGamesUtils.Db.Dal.Entities;

[Index(nameof(ItemFortniteId))]
public class ItemStyleEntity : IEntity
{
    public required Guid Id { get; set; }
    public string FortniteId { get; set; }
    public string Channel { get; set; }
    public string Property { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    // We have 0..1 - 0..n relationship, which is problematic
    // We can't use FK due to constraints as both style X item can be in the database first
    public required string ItemFortniteId { get; set; }
}
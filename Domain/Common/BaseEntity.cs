namespace exam_system.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = default!;
    public DateTime CreatedAtUtc { get; set; } 
    public DateTime? UpdatedAtUtc { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAtUtc{ get; set; }
}

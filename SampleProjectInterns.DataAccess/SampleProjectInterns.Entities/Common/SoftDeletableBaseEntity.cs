namespace SampleProjectInterns.Entities.Common;

public abstract class SoftDeletableBaseEntity : BaseEntity, ISoftDeletable
{
    public bool IsDeleted { get; set; }
}

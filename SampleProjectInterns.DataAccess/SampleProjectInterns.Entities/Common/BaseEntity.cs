using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities.Common;

public abstract class BaseEntity : ICreatedAt, IUpdatedAt
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Status { get; set; }
}

public abstract class BaseGuidEntity : ICreatedAt, IUpdatedAt
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Status { get; set; }
}
public abstract class BaseByteEntity : ICreatedAt, IUpdatedAt
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Status { get; set; }
}

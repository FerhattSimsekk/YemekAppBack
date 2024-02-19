using SampleProjectInterns.Entities.Common;
using static SampleProjectInterns.Entities.Common.Enums;

namespace SampleProjectInterns.Entities;

public class Customer : BaseEntity
{
    public long CompanyId {  get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public Gender Gender { get; set; }
    public long? Phone {  get; set; }
    public string? Mail {  get; set; }
    public string Address { get; set; } = null!;
    public string? Description {  get; set; }


}

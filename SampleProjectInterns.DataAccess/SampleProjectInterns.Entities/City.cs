using SampleProjectInterns.Entities.Common;

namespace SampleProjectInterns.Entities;
public class City : BaseByteEntity
{
    public string Name { get; set; } = null!;
    public int Key { get; set; }
}
public class County : BaseByteEntity
{
    public string Name { get; set; } = null!;
    public int Key { get; set; }
    public int CitiesKey { get; set; }
}

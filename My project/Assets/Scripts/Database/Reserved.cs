using Realms;
using MongoDB.Bson;
public partial class Reserved : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId? Id { get; set; }
    [MapTo("_partition")]
    public string? Partition { get; set; }
    [MapTo("date")]
    public int? Date { get; set; }
    [MapTo("hour")]
    public int? Hour { get; set; }
    [MapTo("location")]
    public string? Location { get; set; }
    [MapTo("min")]
    public int? Min { get; set; }
    [MapTo("month")]
    public int? Month { get; set; }
    [MapTo("name")]
    public string? Name { get; set; }
    [MapTo("year")]
    public int? Year { get; set; }

    public Reserved(string loc, int date, int month, int year, int hr, int min, string name)
    {
        Id = ObjectId.GenerateNewId();
        Partition = "FYP";
        Date = date;
        Hour = hr;
        Min = min;
        Month = month;
        Name = name;
        Year = year;
        Location = loc;
    }
}

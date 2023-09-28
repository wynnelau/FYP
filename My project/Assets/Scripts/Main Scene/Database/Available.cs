using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;

public partial class Available : IRealmObject
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
    [MapTo("year")]
    public int? Year { get; set; }

    public Available()
    {
        Id = ObjectId.GenerateNewId();
        Partition = "FYP";
        Date = 0;
        Hour = 0;
        Min = 0;
        Month = 0;
        Year = 0;
        Location = "lab2";
    }
}


using System;
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public class Available : RealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public string Id { get; set; }
    public int FromDateDate { get; set; }
    public int FromDateMonth { get; set; }
    public int FromDateYear { get; set; }
    public bool FromTimeAM { get; set; }
    public int FromTimeHrs { get; set; }
    public int FromTimeMin { get; set; }
    public string Location { get; set; }
    public int ToDateDate { get; set; }
    public int ToDateMonth { get; set; }
    public int ToDateYear { get; set; }
    public bool ToTimeAM { get; set; }
    public int ToTimeHrs { get; set; }
    public int ToTimeMin { get; set; }
    [MapTo("_partition")]
    public string Partition { get; set; }
}
using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public partial class Meetings : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId? Id { get; set; }
    [MapTo("_partition")]
    public string? Partition { get; set; }
    [MapTo("date")]
    public string? Date { get; set; }
    [MapTo("description")]
    public string? Description { get; set; }
    [MapTo("duration")]
    public string? Duration { get; set; }
    [MapTo("host_email")]
    public string? HostEmail { get; set; }
    [MapTo("join_code")]
    public string? JoinCode { get; set; }
    [MapTo("participant_emails")]
    public IList<Meetings_participant_emails> ParticipantEmails { get; }
    [MapTo("start_time_hr")]
    public int? StartTimeHr { get; set; }
    [MapTo("start_time_min")]
    public int? StartTimeMin { get; set; }
}
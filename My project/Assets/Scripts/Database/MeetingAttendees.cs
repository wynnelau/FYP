using System.Collections.Generic;
using Realms;
using MongoDB.Bson;
public partial class MeetingAttendees : IRealmObject
{
    [MapTo("_id")]
    [PrimaryKey]
    public ObjectId? Id { get; set; }
    [MapTo("_partition")]
    public string? Partition { get; set; }
    [MapTo("host_email")]
    public string? HostEmail { get; set; }
    [MapTo("meetingId")]
    public string? MeetingId { get; set; }
    [MapTo("participant_emails")]
    public IList<MeetingAttendees_participant_emails> ParticipantEmails { get; }

    public MeetingAttendees(string meetingId, string hostEmail)
    {
        Id = ObjectId.GenerateNewId();
        Partition = "FYP";
        HostEmail = hostEmail;
        MeetingId = meetingId;
    }

    public void AddMeetingAttendees(string participantEmail)
    {
        ParticipantEmails.Add(new MeetingAttendees_participant_emails(participantEmail));
    }
}
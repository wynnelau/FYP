using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.UI;
using MongoDB.Bson;
using System.Text;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage send and retrieve data from MongoDB
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c, https://www.youtube.com/watch?v=q4_997QEQww
 */
public class RealmControllerClassRoom : MonoBehaviour
{
    private Realm realm;
    private bool isRealmInitialized = false;

    /*
     * Purpose: Get the realm instance from the previous scene
     * Input: NA
     * Output: Get the realm instance
     */
    private void Start()
    {
        RealmController RealmController = FindObjectOfType<RealmController>();
        if (RealmController != null)
        {
            realm = RealmController.RealmInstance;
            if (realm == null)
            {
                Debug.Log("RealmControllerClassRoom Start RealmIsNull");
            }
            else
            {
                isRealmInitialized = true;
                Debug.Log("RealmControllerClassRoom Start RealmInitialised");
            }
            
        }
        else
        {
            Debug.LogWarning("RealmControllerClassRoom Start RealmControllerIsNull");
        }
    }
    

    /*
     * Purpose: Update meeting details according to objectId and joinCode passed in
     * Input: Called by Relay using CreateRelay() and RemoveJoinCodeSuccess()
     * Output: Call PerformRealmWriteUpdateMeetingDetails()
     *         return if there is an error
     */
    public void UpdateMeetingDetails(string meetingId, string joinCode)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmControllerClassRoom UpdateMeetingDetails RealmNotInitialized");
            return;
        }

        StartCoroutine(PerformRealmWriteUpdateMeetingDetails(meetingId, joinCode));

    }

    /*
     * Purpose: Update meeting attendees according to meetingId and hostEmail passed in
     * Input: Called by Relay using CreateRelay()
     * Output: Call PerformRealmWriteUpdateMeetingAttendeesHost()
     *         return if there is an error
     */
    public void UpdateMeetingAttendeesHost(string meetingId, string hostEmail)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmControllerClassRoom UpdateMeetingAttendeesHost RealmNotInitialized");
            return;
        }

        StartCoroutine(PerformRealmWriteUpdateMeetingAttendeesHost(meetingId, hostEmail));

    }

    /*
     * Purpose: Update meeting attendees according to meetingId and participantEmail passed in
     * Input: Called by Relay using JoinRelay()
     * Output: Call PerformRealmWriteUpdateMeetingAttendeesParticipant()
     *         return if there is an error
     */
    public void UpdateMeetingAttendeesParticipant(string meetingId, string participantEmail)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmControllerClassRoom UpdateMeetingAttendeesParticipant RealmNotInitialized");
            return;
        }

        StartCoroutine(PerformRealmWriteUpdateMeetingAttendeesParticipant(meetingId, participantEmail));

    }

    /*
     * Purpose: Get meeting details according to joinCode passed in
     * Input: Called by Relay using JoinRelay() to get the meetingId
     * Output: Call PerformRealmWriteRetrieveMeetingDetails()
     *         return if there is an error
     */
    public string GetMeetingDetails(string joinCode)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmControllerClassRoom GetMeetingDetails RealmNotInitialized");
            return null;
        }

        var queryResults = PerformRealmWriteRetrieveMeetingDetails(joinCode);
        var meetingId = queryResults.Id.ToString();
        return meetingId;
    }

    /*
     * Purpose: Update the Meeting details according to the meetingId and joinCode
     * Input: Called by UpdateMeetingDetails()
     * Output: Update Meeting details
     */
    private IEnumerator PerformRealmWriteUpdateMeetingDetails(string meetingId, string joinCode)
    {
        try
        {
            realm.Write(() =>
            {
                Meetings result = realm.Find<Meetings>(ObjectId.Parse(meetingId));
                result.JoinCode = joinCode;
            });

        }
        catch (Exception ex)
        {
            Debug.LogError("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails Completed.");
    }

    /*
     * Purpose: Update the Meeting Attendees according to the meetingId and hostEmail
     * Input: Called by UpdateMeetingAttendeesHost()
     * Output: Update Meeting Attendees
     */
    private IEnumerator PerformRealmWriteUpdateMeetingAttendeesHost(string meetingId, string hostEmail)
    {
        try
        {
            realm.Write(() =>
            {
                realm.Add(new MeetingAttendees(meetingId, hostEmail));
            });

        }
        catch (Exception ex)
        {
            Debug.LogError("RealmControllerClassRoom PerformRealmWriteUpdateMeetingAttendeesHost ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmControllerClassRoom PerformRealmWriteUpdateMeetingAttendeesHost Completed.");
    }

    /*
     * Purpose: Update the Meeting Attendees according to the meetingId and participantEmail
     * Input: Called by UpdateMeetingAttendeesParticipant()
     * Output: Update Meeting Attendees
     */
    private IEnumerator PerformRealmWriteUpdateMeetingAttendeesParticipant(string meetingId, string participantEmail)
    {
        try
        {
            realm.Write(() =>
            {
                MeetingAttendees result = realm.All<MeetingAttendees>()
                    .LastOrDefault(item => item.MeetingId == meetingId);
                result.AddMeetingAttendees(participantEmail);
            });

        }
        catch (Exception ex)
        {
            Debug.LogError("RealmControllerClassRoom UpdateMeetingAttendeesParticipant ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmControllerClassRoom UpdateMeetingAttendeesParticipant Completed.");
    }

    /*
     * Purpose: Get the meeting that is ongoing according to the joinCode
     * Input: Called by GetMeetingDetails()
     * Output: return the Meetings according the the joinCode
     */
    private Meetings PerformRealmWriteRetrieveMeetingDetails(string joinCode)
    {
        try
        {
            Meetings result = realm.All<Meetings>()
                .LastOrDefault(item => item.JoinCode == joinCode);
            return result;
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRetrieveMeetingDetails ErrorQueryingRealm: " + ex.Message);
            return null;
        }
    }


}

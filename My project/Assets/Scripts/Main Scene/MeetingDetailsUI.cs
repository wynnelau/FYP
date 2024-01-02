using PlayFab.ClientModels;
using PlayFab;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the MeetingDetails UI
 */
public class MeetingDetailsUI : MonoBehaviour
{
    public GameObject meetingDetails, meetingSchedule;
    public DynamicButtonCreator buttonCreator;
    public RealmController RealmController;
    public MeetingScheduleUI meetingScheduleUI;
    public Text detailsText;
    public Button meetingDetailsStart, meetingDetailsDelete;

    /*
     * Purpose: Close the MeetingDetails UIs when the "closeMeetingDetails" button is clicked
     * Input: Click on the "closeMeetingDetails" button
     * Output: Close the MeetingDetails UI and delete all dynamic buttons
     */
    public void CloseMeetingDetails()
    {
        Debug.Log("MeetingDetailsUI closeMeetingDetails");
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        buttonCreator.DeleteAllButtons();

        detailsText.text = "";
        meetingDetails.SetActive(false);
        meetingSchedule.SetActive(true);
    }

    /*
     * Purpose: Save the meetingID and change scene when the "startMeeting" button is clicked
     * Input: Click on the "startMeeting" button
     * Output: Attempt to save the objectID of the meeting to PlayFab
     */
    public void StartMeeting()
    {
        if (meetingDetailsStart.GetComponent<Image>().color == Color.white)
        {
            string[] stringSplit = detailsText.text.Split('\n');
            string meetingId = stringSplit[0];
            Debug.Log("MeetingDetailsUI StartMeeting");
            var request = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                    {
                        {"MeetingID", meetingId}
                    }
            };
            PlayFabClientAPI.UpdateUserData(request, StartMeetingSuccess, StartMeetingFail);
        }
    }

    /*
     * Purpose: Load the ClassRoom Scene when attempt to save meetingId is successful
     * Input: Attempt to save meetingId is successful
     * Output: Load the ClassRoom Scene
     */
    void StartMeetingSuccess(UpdateUserDataResult result)
    {
        SceneManager.LoadScene("ClassRoom Scene");
    }

    /*
    * Purpose: Log the error when attempt to save meetingId failed
    * Input: Attempt to save meetingId failed
    * Output: Log the error
    */
    void StartMeetingFail(PlayFabError error)
    {
        Debug.Log("MeetingDetailsUI StartMeetingFail" + error);
    }

    /*
     * Purpose: Delete the schedule meeting when the "deleteMeeting" button is clicked
     * Input: Click on the "deleteMeeting" button
     * Output: Call RemoveMeeting()
     */
    public void DeleteMeeting()
    {
        if (meetingDetailsDelete.GetComponent<Image>().color == Color.white)
        {
            string[] stringSplit = detailsText.text.Split('\n');
            string meetingId = stringSplit[0];
            Debug.Log("MeetingDetailsUI DeleteMeeting");
            RealmController = FindObjectOfType<RealmController>();
            RealmController.RemoveMeeting(meetingId);
            RefreshMeetingDetails();
        }
    }

    /*
     * Purpose: Refresh MeetingDetails UI with the updated meetings
     * Input: Called by DeleteMeeting() 
     * Output: Call CreateButton() to create updated time buttons
     */
    void RefreshMeetingDetails()
    {
        Debug.Log("MeetingDetailsUI RefreshMeetingDetails");
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        meetingScheduleUI = FindObjectOfType<MeetingScheduleUI>();
        buttonCreator.DeleteAllButtons();
        meetingDetailsStart.GetComponent<Image>().color = Color.grey;
        meetingDetailsDelete.GetComponent<Image>().color = Color.grey;
        detailsText.text = "";

        if (RealmController != null)
        {
            Debug.Log("MeetingDetailsUI RefreshMeetingDetails notNull");
            var meetingList = RealmController.GetMeetings();
            if (meetingList != null && meetingList.Count > 0)
            {
                foreach (var meeting in meetingList)
                {
                    buttonCreator.CreateButton(meeting.Id.ToString() + "\nStart time:     " + meetingScheduleUI.TimeConvert(meeting.StartTimeHr) + ":" + meetingScheduleUI.TimeConvert(meeting.StartTimeMin));
                }

            }
        }

    }
}

    

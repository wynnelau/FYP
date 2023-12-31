using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the MeetingDetails UI
 */
public class MeetingDetailsUI : MonoBehaviour
{
    public GameObject meetingDetails, meetingSchedule;
    public RealmController RealmController;
    public DynamicButtonCreator buttonCreator;

    /*
     * Purpose: Close the MeetingDetails UIs when the "closeMeetingDetails" button is clicked
     * Input: Click on the "closeMeetingDetails" button
     * Output: Close the MeetingDetails UI
     */
    public void closeMeetingDetails()
    {
        Debug.Log("MeetingDetailsUI closeMeetingDetails");
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        buttonCreator.DeleteAllButtons();

        meetingDetails.SetActive(false);
        meetingSchedule.SetActive(true);
    }
}

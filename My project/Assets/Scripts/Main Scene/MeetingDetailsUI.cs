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
    public RealmController RealmController;
    public DynamicButtonCreator buttonCreator;
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
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        buttonCreator.DeleteAllButtons();

        detailsText.text = "";
        meetingDetails.SetActive(false);
        meetingSchedule.SetActive(true);
    }

    public void StartMeeting()
    {
        if (meetingDetailsStart.GetComponent<Image>().color == Color.white)
        {
            SceneManager.LoadScene("ClassRoom Scene");
        }
    }

    public void DeleteMeeting()
    {
        if (meetingDetailsDelete.GetComponent<Image>().color == Color.white)
        {
            Debug.Log("Delete");
        }
    }
}

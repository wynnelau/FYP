using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetingDetailsUI : MonoBehaviour
{
    public GameObject meetingDetails, meetingSchedule;

    /*
     * Purpose: Close the MeetingDetails UIs when the "closeMeetingDetails" button is clicked
     * Input: Click on the "closeMeetingDetails" button
     * Output: Close the MeetingDetails UI
     */
    public void closeMeetingDetails()
    {
        Debug.Log("MeetingDetailsUI closeMeetingDetails");
        meetingDetails.SetActive(false);
        meetingSchedule.SetActive(true);
    }
}

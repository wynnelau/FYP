using UnityEngine;
using UnityEngine.UI;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the TimeDetailsUI
 */
public class TimeDetailsUI : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf, timeDetailsStaff, timeDetailsProf;
    public DynamicButtonCreator buttonCreator;
    public RealmController RealmController;
    public Text timeDetailsDateProf, timeDetailsLocationProf, timeDetailsDateStaff, timeDetailsTimeStaff, timeDetailsUserStaff;

    /*
     * Purpose: Close the TimeDetails UIs when the "closeTimeDetails" button is clicked
     * Input: Click on the "closeTimeDetails" button
     * Output: Delete dynamic buttons in the timeDetails UI, go to the respective dateDetails UI and 
     *         then create dynamic buttons in the dateDetails UI
     */
    public void CloseTimeDetails()
    {
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();

        // Clear dynamic buttons, addReservationList and removeReservationList
        buttonCreator.DeleteAllButtons();
        buttonCreator.ClearAddReservationList();
        buttonCreator.ClearRemoveReservationList();

        string date = "";

        if (timeDetailsProf.activeSelf == true)
        {
            Debug.Log("TimeDetailsUI closeTimeDetails Prof");
            timeDetailsProf.SetActive(false);
            dateDetailsProf.SetActive(true);
            date = timeDetailsDateProf.text;
        }
        else if (timeDetailsStaff.activeSelf == true)
        {
            Debug.Log("TimeDetailsUI closeTimeDetails Staff");
            timeDetailsTimeStaff.text = "";
            timeDetailsUserStaff.text = "";
            timeDetailsStaff.SetActive(false);
            dateDetailsStaff.SetActive(true);
            date = timeDetailsDateStaff.text;
        }

        if (RealmController != null)
        {
            var locationList = RealmController.GetLocations(date);
            if (locationList != null && locationList.Count > 0)
            {
                foreach (var location in locationList)
                {
                    buttonCreator.CreateButton(location);
                }

            }
        }

    }

    /*
     * Purpose: Attempt to add reservation slots
     * Input: Click on the "addReservationButton" button in "timeDetailsProf" UI
     * Output: Call AddReservation() and then clears the list
     */
    public void AddReservationSlots()
    {
        Debug.Log("TimeDetailsUI AddReservationSlots");
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        RealmController.AddReservation();
        buttonCreator.ClearAddReservationList();
        buttonCreator.ClearRemoveReservationList();
        buttonCreator.DeleteAllButtons();
        RefreshTimeDetails();
    }

    /*
     * Purpose: Attempt to remove reservation slots
     * Input: Click on the "removeRservationButton" button in "timeDetailsProf" UI
     * Output: Call RemoveReservation() and then clears the list
     */
    public void RemoveReservationSlots()
    {
        Debug.Log("TimeDetailsUI RemoveReservationSlots");
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        RealmController.RemoveReservation();
        buttonCreator.ClearAddReservationList();
        buttonCreator.ClearRemoveReservationList();
        buttonCreator.DeleteAllButtons();
        RefreshTimeDetails();
    }

    /*
     * Purpose: Refresh TimeDetails UI with the updated time button colors
     * Input: Called by AddReservationSlots() and RemoveReservationSlots()
     * Output: Call CreateButton() to create updated time buttons
     */
    void RefreshTimeDetails()
    {
        Debug.Log("TimeDetailsUI RefreshTimeDetails");
        RealmController = FindObjectOfType<RealmController>();
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();

        if (RealmController != null)
        {
            Debug.Log("TimeDetailsUI RefreshTimeDetails notNull");
            var timingList = RealmController.GetTimings(timeDetailsLocationProf.text, timeDetailsDateProf.text);
            if (timingList != null && timingList.Count > 0)
            {
                var convertedList = buttonCreator.ConvertToRange(timingList);
                foreach (var timing in convertedList)
                {
                    buttonCreator.CreateButton(timing);
                }

            }
        }
    }

}

using UnityEngine;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the DateDetailsUI, which contains location buttons that are available
 */
public class DateDetailsUI : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf, resourceReservationProf, resourceReservationStaff;
    public DynamicButtonCreator buttonCreator;

    /*
     * Purpose: Close the datedetails UI when the "closeDateDetailsButton" button is clicked
     * Input: Click on the "closeDateDetailsButton" button
     * Output: If user is Prof, delete all dynamic buttons and go to "resourceReservationProf" UI
     *         else if user is Staff, delete all dynamic buttons and go to "resourceReservationStaff" UI
     */
    public void CloseDetails()
    {
        Debug.Log("DateDetailsUI closeDetails");
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        buttonCreator.DeleteAllButtons();

        if (dateDetailsProf.activeSelf == true)
        {
            Debug.Log("DateDetailsUI closeDetails Prof");
            dateDetailsProf.SetActive(false);
            resourceReservationProf.SetActive(true);
        }
        else if (dateDetailsStaff.activeSelf == true)
        {
            Debug.Log("DateDetailsUI closeDetails Staff");
            dateDetailsStaff.SetActive(false);
            resourceReservationStaff.SetActive(true);
        }

    }
}

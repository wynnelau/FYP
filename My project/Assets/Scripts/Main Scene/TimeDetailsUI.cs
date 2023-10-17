using System.Collections.Generic;
using UnityEngine;

/*
 * Location: MainSceneControls
 * Purpose: Manage the TimeDetailsUI
 */
public class TimeDetailsUI : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf, timeDetailsStaff, timeDetailsProf;
    public DynamicButtonCreator buttonCreator;
    public RealmController RealmController;

    /*
     * Purpose: Close the TimeDetails UIa when the "closeTimeDetails" button is clicked
     * Input: Click on the "closeTimeDetails" button
     * Output: Delete dynamic buttons in the timeDetails UI, go to the respective dateDetails UI and 
     *         then create dynamic buttons in the dateDetails UI
     */
    public void closeTimeDetails()
    {
        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        buttonCreator.DeleteAllButtons();

        if (timeDetailsProf.activeSelf == true)
        {
            Debug.Log("TimeDetailsUI closeTimeDetails Prof");
            timeDetailsProf.SetActive(false);
            dateDetailsProf.SetActive(true);
            RealmController = FindObjectOfType<RealmController>();
            buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        }
        else if (timeDetailsStaff.activeSelf == true)
        {
            Debug.Log("TimeDetailsUI closeTimeDetails Staff");
            timeDetailsStaff.SetActive(false);
            dateDetailsStaff.SetActive(true);
            RealmController = FindObjectOfType<RealmController>();
            buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        }

        if (RealmController != null)
        {
            var locationList = RealmController.GetLocations();
            if (locationList != null && locationList.Count > 0)
            {
                foreach (var location in locationList)
                {
                    buttonCreator.CreateButton(location);
                }

            }
        }

    }

}

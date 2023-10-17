using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/*
 * Location: MainSceneControls
 * Purpose: Used to create the dynamic buttons for dateDetails UI and timeDetails UI
 */
public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf;
    public GameObject timeDetailsStaff, timeDetailsProf;
    public Text dateDetailsDateStaff, dateDetailsDateProf, timeDetailsDateStaff, timeDetailsDateProf;
    public GameObject buttonDatePrefabStaff, buttonDatePrefabProf, buttonTimePrefabStaff, buttonTimePrefabProf; // Assign your button prefab in the inspector
    public Transform buttonDateParentStaff, buttonDateParentProf, buttonTimeParentStaff, buttonTimeParentProf; // Assign the parent transform for the buttons in the inspector
    public RealmController RealmController;

    private Color lightBlueColor = new Color(0.678f, 0.847f, 0.902f, 1.0f);
    private Color lightGreenColor = new Color(0.678f, 0.902f, 0.678f, 1.0f);
    private Color lightRedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    private List<GameObject> createdButtons = new List<GameObject>();

    private List<string> addReservationList = new List<string>();
    private List<string> removeReservationList = new List<string>();

    /*
     * Purpose: Create a dynamic button with the given text and with a click event listener
     * Input: Called by ResourceReservationUI using openDetails() to create buttons in the respective dateDetails UIs
     *        Called by TimeDetailsUI using closeTimeDetails() to create buttons in the respective dateDetails UIs after closing timeDetails UI
     *        Click on the buttons in dateDetails UIs to create buttons in the respective timeDetails UI
     * Output: Create dynamic button according to the string passed into it and add a click event listener accordingly
     */
    public void CreateButton(string buttonText)
    {
        GameObject newButton = null;
        if (dateDetailsStaff.activeSelf)
        {
            newButton = Instantiate(buttonDatePrefabStaff, buttonDateParentStaff);
        } 
        else if (dateDetailsProf.activeSelf)
        {
            newButton = Instantiate(buttonDatePrefabProf, buttonDateParentProf);
        }
        else if (timeDetailsStaff.activeSelf)
        {
            newButton = Instantiate(buttonTimePrefabStaff, buttonTimeParentStaff);
        }
        else if (timeDetailsProf.activeSelf)
        {
            newButton = Instantiate(buttonTimePrefabProf, buttonTimeParentProf);
        }
        else
        {
            return;
        }

        Button buttonComponent = newButton.GetComponent<Button>();
        Text buttonTextComponent = newButton.GetComponentInChildren<Text>();
        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = buttonText;
        }

        /*RealmController = FindObjectOfType<RealmController>();
        if (RealmController != null)
        {
            Debug.Log("DynamicButtonCreator RealmController not null");
            var timingList = RealmController.GetReservations(buttonText);
            if (timingList != null && timingList.Count > 0)
            {
                var convertedList = ConvertToRange(timingList);
                foreach (var timing in convertedList)
                {
                    buttonComponent.GetComponent<Image>().color = lightBlueColor;
                }

            }
        }*/


        buttonComponent.onClick.AddListener(() =>
        {
            /*
             * Purpose: Add a click event listener to the button created in dateDetails UI
             * Input: Click on the button in dateDetails UI
             * Output: Go the respective timeDetails UI and call GetTiming() passing the location string to 
             *         get a list of strings containing the buttons to be created in the timeDetails UI
             */
            if (dateDetailsProf.activeSelf == true)
            {
                Debug.Log("DynamicButtonCreator buttonOnClick Prof");
                RealmController = FindObjectOfType<RealmController>();
                dateDetailsProf.SetActive(false);
                timeDetailsProf.SetActive(true);
                timeDetailsDateProf.text = dateDetailsDateProf.text;
                if (RealmController != null)
                {
                    Debug.Log("DynamicButtonCreator RealmController not null");
                    var timingList = RealmController.GetTimings(buttonText);
                    if (timingList != null && timingList.Count > 0)
                    {
                        var convertedList = ConvertToRange(timingList);
                        foreach (var timing in convertedList)
                        {
                            CreateButton(timing);
                        }

                    }
                }
            }
            else if (dateDetailsStaff.activeSelf == true)
            {
                Debug.Log("DynamicButtonCreator buttonOnClick Staff");
                RealmController = FindObjectOfType<RealmController>();
                dateDetailsStaff.SetActive(false);
                timeDetailsStaff.SetActive(true);
                timeDetailsDateStaff.text = dateDetailsDateStaff.text;
                if (RealmController != null)
                {
                    Debug.Log("DynamicButtonCreator RealmController not null");
                    var timingList = RealmController.GetTimings(buttonText);
                    if (timingList != null && timingList.Count > 0)
                    {
                        var convertedList = ConvertToRange(timingList);
                        foreach (var timing in convertedList)
                        {
                            CreateButton(timing);
                        }

                    }
                }
            }
            
            /*
             * Purpose: Add a click event listener to the button created in timeDetails UI
             * Input: Click on the button in timeDetails UI
             * Output: If available (white) pressed, turn it to selected (lightGreen) and add to addReservationList
             *         else if selected (lightGreen) pressed, turn it to available (white) and remove it from addReservationList
             *         else if reservedByMe (lightBlue) pressed, turn it to removeReservation (lightRed) and add it to removeReservationList
             *         else if removeReservation (lightRed) pressed, turn it to reservedByMe (lightBlue) and remove it from removeReservationList
             *         grey (reservedByOthers) cannot be clicked
             */
            else if (timeDetailsProf.activeSelf == true)
            {
                if (buttonComponent.GetComponent<Image>().color == Color.white)
                {
                    buttonComponent.GetComponent<Image>().color = lightGreenColor;
                    addReservationList.Add(buttonTextComponent.text);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightGreenColor)
                {
                    buttonComponent.GetComponent<Image>().color = Color.white;
                    addReservationList.Remove(buttonTextComponent.text);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightBlueColor)
                {
                    buttonComponent.GetComponent<Image>().color = lightRedColor;
                    removeReservationList.Add(buttonTextComponent.text);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightRedColor)
                {
                    buttonComponent.GetComponent<Image>().color = lightBlueColor;
                    removeReservationList.Remove(buttonTextComponent.text);
                }
            }
            
            Debug.Log("Button Clicked: " + buttonText);
        });

        createdButtons.Add(newButton);
    }

    /*
     * Purpose: Delete all the dynamic buttons created in a UI
     * Input: Called by DateDetailsUI by closeDetails() to delete all buttons in the respective dateDetails UI
     *        Called by TimeDetailsUI by closeTimeDetails() to delete all buttons in the respective timeDetails UI
     * Output: Destroy all the dynamic buttons in the UI and clear the list of buttons
     */
    public void DeleteAllButtons()
    {
        foreach (GameObject button in createdButtons)
        {
            // Destroy each button GameObject
            Destroy(button);
        }

        // Clear the list of created buttons
        createdButtons.Clear();
    }

    /*
     * Purpose: Convert the timings in "hr:min" to a slot of time in "hr:min to hr:min" when button in dateDetails is clicked
     * Input: Called by when button in dateDetails UI is clicked and a list of strings is passed in
     * Output: Returns a list of converted strings used for creating buttons in timeDetails UI
     */
    private List<string> ConvertToRange(List<string> timingList)
    {
        List<string> convertedList = new List<string>();
        string[] splitResult;
        string converted, endTime;
        foreach (var timing in timingList)
        {
            splitResult = timing.Split(":");

            endTime = GetEndTiming(splitResult[0], splitResult[1]);

            if (splitResult[0].Length == 1)
            {
                splitResult[0] = "0" + splitResult[0];
            }
            if (splitResult[1] == "0")
            {
                splitResult[1] = "0" + splitResult[1];
            }
            converted = splitResult[0] + ":" + splitResult[1] + " to " + endTime;
            convertedList.Add(converted);
        }
        return convertedList;
    }

    /*
     * Purpose: Convert a slot of time in "hr:min to hr:min" to the timings in "hr:min" 
     * Input: 
     * Output: Returns a list of converted strings used to send data to database
     */
    private List<string> ConvertToTiming(List<string> timingList)
    {
        List<string> convertedList = new List<string>();
        string[] firstSplit, splitResult;
        string converted;
        foreach (var timing in timingList)
        {
            firstSplit = timing.Split(new string[] { " to " }, StringSplitOptions.None);
            
            splitResult = new string[2];
            splitResult[0] = firstSplit[0].Split(':')[0];
            splitResult[1] = firstSplit[0].Split(':')[1];

            converted = splitResult[0] + ":" + splitResult[1];
            convertedList.Add(converted);
        }
        return convertedList;
    }

    /*
     * Purpose: Get the end time of each slot of time according to the start time
     * Input: Called by ConvertToRange() and start time is passed in
     * Output: Returns a string of the end time calculated
     */
    private string GetEndTiming(string hr, string min)
    {
        int ihr, imin;
        string endTime;
        ihr = int.Parse(hr);
        imin = int.Parse(min);
        if (imin == 0)
        {
            imin = 30;
        } 
        else if (imin == 30)
        {
            imin = 0;
            ihr++;
        }
        hr = ihr.ToString();
        min = imin.ToString();
        if (hr.Length == 1)
        {
            hr = "0" + hr;
        }
        if (min == "0")
        {
            min = "0" + min;
        }
        endTime = hr + ":" + min;
        return endTime;
    }

    /*
     * Purpose: Getter function of the private GetAddReservationList
     * Input: Called by functions
     * Output: Returns the list of strings GetAddReservationList
     */
    public List<string> GetAddReservationList
    {
        get
        {
            return addReservationList;
        }
    }

}

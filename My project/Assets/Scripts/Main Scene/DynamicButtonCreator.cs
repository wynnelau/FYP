using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using PlayFab;
using PlayFab.ClientModels;


/*
 * Location: MainSceneControls
 * Purpose: Used to create the dynamic buttons for dateDetails UI and timeDetails UI
 * Tutorial used: https://www.youtube.com/watch?v=8bMzz-nSIwg
 */
public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf;
    public GameObject timeDetailsStaff, timeDetailsProf;
    public Text dateDetailsDateStaff, dateDetailsDateProf, timeDetailsDateStaff, timeDetailsDateProf, timeDetailsLocationStaff, timeDetailsLocationProf;
    public GameObject buttonDatePrefabStaff, buttonDatePrefabProf, buttonTimePrefabStaff, buttonTimePrefabProf; // Assign your button prefab in the inspector
    public Transform buttonDateParentStaff, buttonDateParentProf, buttonTimeParentStaff, buttonTimeParentProf; // Assign the parent transform for the buttons in the inspector
    public RealmController RealmController;

    private Color lightBlueColor = new Color(0.678f, 0.847f, 0.902f, 1.0f);
    private Color lightGreenColor = new Color(0.678f, 0.902f, 0.678f, 1.0f);
    private Color lightRedColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);

    private List<GameObject> createdButtons = new List<GameObject>();

    private List<Reserved> addReservationList = new List<Reserved>();
    private List<Reserved> removeReservationList = new List<Reserved>();
    private string userEmail ;

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
            ResourceReservationUI ResourceReservationUI = FindObjectOfType<ResourceReservationUI>();
            userEmail = ResourceReservationUI.GetUserEmail;
            newButton = Instantiate(buttonTimePrefabStaff, buttonTimeParentStaff);

        }
        else if (timeDetailsProf.activeSelf)
        {
            ResourceReservationUI ResourceReservationUI = FindObjectOfType<ResourceReservationUI>();
            userEmail = ResourceReservationUI.GetUserEmail;
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

        // Check whether button is available (white), reservedByMe (lightBlue) or grey (reservedByOthers)
        if (timeDetailsStaff.activeSelf)
        {
            RealmController = FindObjectOfType<RealmController>();
            Reserved reservation = ConvertToReserved(timeDetailsLocationStaff.text, timeDetailsDateStaff.text, buttonTextComponent.text);
            var checkReservation = RealmController.GetReservations(reservation);
            if (checkReservation.Count == 0)
            {
                buttonComponent.GetComponent<Image>().color = Color.white;
            }
            else
            {
                buttonComponent.GetComponent<Image>().color = Color.grey;
            }
        }
        else if (timeDetailsProf.activeSelf)
        {
            
            RealmController = FindObjectOfType<RealmController>();
            Reserved reservation = ConvertToReserved(timeDetailsLocationProf.text, timeDetailsDateProf.text, buttonTextComponent.text);
            var checkReservation = RealmController.GetReservations(reservation);
            if (checkReservation.Count == 0)
            {
                buttonComponent.GetComponent<Image>().color = Color.white;
            }
            else if (checkReservation[0].Name == userEmail)
            {
                Debug.Log(checkReservation[0].Name + "1" + userEmail);
                Debug.Log("1" + userEmail);
                buttonComponent.GetComponent<Image>().color = lightBlueColor;
            }
            else
            {
                Debug.Log(checkReservation[0].Name + "1" + userEmail);
                Debug.Log("1" + userEmail);
                buttonComponent.GetComponent<Image>().color = Color.grey;
            }
        }




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
                timeDetailsLocationProf.text = buttonText;
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
                timeDetailsLocationStaff.text = buttonText;
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
                Reserved reservation = ConvertToReserved(timeDetailsLocationProf.text, timeDetailsDateProf.text, buttonTextComponent.text);
                if (buttonComponent.GetComponent<Image>().color == Color.white)
                {
                    buttonComponent.GetComponent<Image>().color = lightGreenColor;
                    addReservationList.Add(reservation);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightGreenColor)
                {
                    buttonComponent.GetComponent<Image>().color = Color.white;
                    addReservationList.Remove(reservation);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightBlueColor)
                {
                    buttonComponent.GetComponent<Image>().color = lightRedColor;
                    removeReservationList.Add(reservation);
                }
                else if (buttonComponent.GetComponent<Image>().color == lightRedColor)
                {
                    buttonComponent.GetComponent<Image>().color = lightBlueColor;
                    removeReservationList.Remove(reservation);
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

    private Reserved ConvertToReserved(string location, string dateString, string timing)
    {
        string[] dateParts = dateString.Split('/');
        int date = int.Parse(dateParts[0]);
        int month = int.Parse(dateParts[1]);
        int year = int.Parse(dateParts[2]) - 2000;
        string[] timingParts = ConvertToTiming(timing);
        int hr = int.Parse(timingParts[0]);
        int min = int.Parse(timingParts[1]);

        Reserved reserved = new Reserved(location, date, month, year, hr, min, userEmail);
        return reserved;
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
     * Purpose: Convert a slot of time in "hr:min to hr:min" to the timings in "hr:min" 
     * Input: 
     * Output: Returns a string array used to send data to database
     */
    private string[] ConvertToTiming(string timing)
    {
        string[] firstSplit, splitResult;
  
        firstSplit = timing.Split(new string[] { " to " }, StringSplitOptions.None);

        splitResult = new string[2];
        splitResult[0] = firstSplit[0].Split(':')[0];
        splitResult[1] = firstSplit[0].Split(':')[1];

        return splitResult;
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
     * Purpose: Getter function of the private addReservationList
     * Input: Called by RealmController by AddReservation()
     * Output: Returns the list of strings addReservationList
     */
    public List<Reserved> GetAddReservationList
    {
        get
        {
            return addReservationList;
        }
    }

    /*
     * Purpose: Getter function of the private removeReservationList
     * Input: Called by RealmController by RemoveReservation()
     * Output: Returns the list of strings removeReservationList
     */
    public List<Reserved> GetRemoveReservationList
    {
        get
        {
            return removeReservationList;
        }
    }

}

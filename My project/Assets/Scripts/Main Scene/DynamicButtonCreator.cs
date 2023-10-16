using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using System;

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

    // This method creates a dynamic button with the given text and assigns a callback function to it.
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

        // Set the button's text
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

        // Add a click event handler to the button
        buttonComponent.onClick.AddListener(() =>
        {
            /*
             * Purpose: Create dynamic timing buttons according to the location button pressed
             * Outcome: call GetTimings according to location and get a timing list to create the dynamic buttons
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
             * Purpose: If available (white) timing pressed, turn it to selected (lightGreen) and add to addReservationList
             * If selected (lightGreen) timing pressed, turn it to available (white) and remove it from addReservationList
             * If reservedByMe (lightBlue) timing pressed, turn it to removeReservation (lightRed) and add it to removeReservationList
             * If removeReservation (lightRed) timing pressed, turn it to reservedByMe (lightBlue) and remove it from removeReservationList
             * grey (reservedByOthers) cannot be clicked
             * Outcome: call GetTimings according to location and get a timing list to create the dynamic buttons
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

    // used to convert the timings to the period of time eg. "0:0" to "00:00 to 00:30"
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

    public List<string> GetAddReservationList
    {
        get
        {
            return addReservationList;
        }
    }

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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

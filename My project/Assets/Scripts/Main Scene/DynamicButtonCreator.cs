using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;

public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf;
    public GameObject timeDetailsStaff, timeDetailsProf;
    public Text dateDetailsDateStaff, dateDetailsDateProf, timeDetailsDateStaff, timeDetailsDateProf;
    public GameObject buttonDatePrefabStaff, buttonDatePrefabProf, buttonTimePrefabStaff, buttonTimePrefabProf; // Assign your button prefab in the inspector
    public Transform buttonDateParentStaff, buttonDateParentProf, buttonTimeParentStaff, buttonTimeParentProf; // Assign the parent transform for the buttons in the inspector
    public RealmController RealmController;

    private List<GameObject> createdButtons = new List<GameObject>();

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
            Debug.Log("timeDetailsStaff newButton");
            newButton = Instantiate(buttonTimePrefabStaff, buttonTimeParentStaff);
        }
        else if (timeDetailsProf.activeSelf)
        {
            Debug.Log("timeDetailsProf newButton");
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

        // Add a click event handler to the button
        buttonComponent.onClick.AddListener(() =>
        {
            // Handle button click here
            if (dateDetailsProf.activeSelf == true)
            {
                Debug.Log("DynamicButtonCreator buttonOnClick Prof");
                RealmController = FindObjectOfType<RealmController>();
                dateDetailsProf.SetActive(false);
                timeDetailsProf.SetActive(true);
                timeDetailsDateProf.text = dateDetailsDateProf.text;
            }
            else if (dateDetailsStaff.activeSelf == true)
            {
                Debug.Log("DynamicButtonCreator buttonOnClick Staff");
                RealmController = FindObjectOfType<RealmController>();
                dateDetailsStaff.SetActive(false);
                timeDetailsStaff.SetActive(true);
                timeDetailsDateStaff.text = dateDetailsDateStaff.text;
            }
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

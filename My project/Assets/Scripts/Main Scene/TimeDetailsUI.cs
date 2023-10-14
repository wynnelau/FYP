using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDetailsUI : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf, timeDetailsStaff, timeDetailsProf;
    public DynamicButtonCreator buttonCreator;
    public RealmController RealmController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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

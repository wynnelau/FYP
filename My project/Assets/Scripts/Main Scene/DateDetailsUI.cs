using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateDetailsUI : MonoBehaviour
{
    public GameObject dateDetails, resourceReservation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void closeDetails()
    {
        if(dateDetails.activeSelf == true)
        {
            dateDetails.SetActive(false);
            resourceReservation.SetActive(true);
        }

    }
}

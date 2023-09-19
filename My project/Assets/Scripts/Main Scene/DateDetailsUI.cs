using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateDetailsUI : MonoBehaviour
{
    public GameObject dateDetailsStaff, dateDetailsProf, resourceReservationProf, resourceReservationStaff;
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
        if(dateDetailsProf.activeSelf == true)
        {
            dateDetailsProf.SetActive(false);
            resourceReservationProf.SetActive(true);
        }
        else if (dateDetailsStaff.activeSelf == true)
        {
            dateDetailsStaff.SetActive(false);
            resourceReservationStaff.SetActive(true);
        }

    }
}

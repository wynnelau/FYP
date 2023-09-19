using UnityEngine;

/*
 *Location: Main Scene, attached to "Controls"
 *Purpose: manage date details UI
 */
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

    /*
     * Purpose: To close the date details UI, attached to "closeDateDetailsButton"
     * Outcomes: deactivate date details
     */
    public void closeDetails()
    {
        if(dateDetailsProf.activeSelf == true)
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

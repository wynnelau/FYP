using UnityEngine;

/*
 *Location: Main Scene, attached to "Controls"
 *Purpose: manage manageSlots UI
 */
public class ManageSlotsUI : MonoBehaviour
{
    public GameObject resourceReservationStaff, manageSlotsStaff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To open manageSlots UI, attached to "addSlotsButton" in "resourceReservationStaff" UI
     * Outcomes: open the manageSlotsUI and closes the resourceReservationStaffUI
     */
    public void closeManageSlots()
    {
        if (manageSlotsStaff.activeSelf == true)
        {
            manageSlotsStaff.SetActive(false);
            resourceReservationStaff.SetActive(true);
        }
    }

}

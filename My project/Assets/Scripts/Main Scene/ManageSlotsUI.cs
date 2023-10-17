using UnityEngine;

/*
 * Location: MainSceneControls
 * Purpose: Manage manageSlots UI
 */
public class ManageSlotsUI : MonoBehaviour
{
    public GameObject resourceReservationStaff, manageSlotsStaff;

    /*
     * Purpose: Close the "manageSlots" UI when the "closeManageSlotsButton" button is clicked
     * Input: Click on the "closeManageSlotsButton" button
     * Output: Close the "manageSlots" UI and open the "resourceReservationStaff" UI
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

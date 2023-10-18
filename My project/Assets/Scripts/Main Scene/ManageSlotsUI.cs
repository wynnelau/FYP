using UnityEngine;

/*
 * Location: MainSceneControls
 * Purpose: Manage manageSlots UI, which allows staff to create new available slots
 */
public class ManageSlotsUI : MonoBehaviour
{
    public GameObject resourceReservationStaff, manageSlotsStaff;
    public RealmController RealmController;

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

    /*
     * Purpose: Attempt to add available slots
     * Input: Click on the "addSlotsButton" button in "manageSlots" UI
     * Output: Call AddAvailable()
     */
    public void AddAvailableSlots()
    {
        RealmController = FindObjectOfType<RealmController>();
        RealmController.AddAvailable();
    }

    /*
     * Purpose: Attempt to remove available slots
     * Input: Click on the "removeSlotsButton" button in "manageSlots" UI
     * Output: Call RemoveAvailable()
     */
    public void RemoveAvailableSlots()
    {
        RealmController = FindObjectOfType<RealmController>();
        RealmController.RemoveAvailable();
    }

}

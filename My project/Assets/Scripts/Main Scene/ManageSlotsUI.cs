using UnityEngine;
using UnityEngine.UI;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage manageSlots UI, which allows staff to create new available slots
 */
public class ManageSlotsUI : MonoBehaviour
{
    public GameObject resourceReservationStaff, manageSlotsStaff;
    public RealmController RealmController;
    public InputField location, fromDate, fromMonth, fromYear, toDate, toMonth, toYear;
    public Dropdown fromHr, fromMin, fromAm, toHr, toMin, toAm;
    public Text errorText;

    /*
     * Purpose: Close the "manageSlots" UI when the "closeManageSlotsButton" button is clicked
     * Input: Click on the "closeManageSlotsButton" button
     * Output: Close the "manageSlots" UI and open the "resourceReservationStaff" UI
     */
    public void CloseManageSlots()
    {
        Debug.Log("ManageSlotsUI closeManageSlots");
        if (manageSlotsStaff.activeSelf == true)
        {
            Debug.Log("ManageSlotsUI closeManageSlots Staff");
            errorText.text = "";
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
        Debug.Log("ManageSlotsUI AddAvailableSlots");
        RealmController = FindObjectOfType<RealmController>();
        RealmController.AddAvailable(location.text, fromDate.text, fromMonth.text, fromYear.text, toDate.text, toMonth.text, toYear.text, fromHr.value, fromMin.value, fromAm.value, toHr.value, toMin.value, toAm.value);
    }

    /*
     * Purpose: Attempt to remove available slots
     * Input: Click on the "removeSlotsButton" button in "manageSlots" UI
     * Output: Call RemoveAvailable()
     */
    public void RemoveAvailableSlots()
    {
        Debug.Log("ManageSlotsUI RemoveAvailableSlots");
        RealmController = FindObjectOfType<RealmController>();
        RealmController.RemoveAvailable(location.text, fromDate.text, fromMonth.text, fromYear.text, toDate.text, toMonth.text, toYear.text, fromHr.value, fromMin.value, fromAm.value, toHr.value, toMin.value, toAm.value);
    }

}

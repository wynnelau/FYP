using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the EnableClassUI
 */

public class EnableClassUI : MonoBehaviour
{
    public GameObject enableClassUI;

    /*
     * Purpose: Close the EnableClass UI
     * Input: Click on "X" button in EnableClass UI
     * Output: Close the EnableClass UI
     */
    public void CloseEnableClassUI()
    {
        enableClassUI.SetActive(false);
    }

    /*
     * Purpose: Enable the EnableClass UI
     * Input: Click on "enableClasses" button in NetworkManager UI
     * Output: Enable the EnableClass UI
     */
    public void OpenEnableClassUI()
    {
        enableClassUI.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the EnableClassUI
 */

public class EnableClassUI : MonoBehaviour
{
    public GameObject enableClassUI;
    public void CloseEnableClassUI()
    {
        enableClassUI.SetActive(false);
    }

    public void OpenEnableClassUI()
    {
        enableClassUI.SetActive(true);
    }
}

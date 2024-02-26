using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the EnableQuizUI
 */

public class EnableQuizUI : MonoBehaviour
{
    public GameObject enableQuizUI;
    public void CloseEnableQuizUI()
    {
        enableQuizUI.SetActive(false);
    }

    public void OpenEnableQuizUI()
    {
        enableQuizUI.SetActive(true);
    }
}

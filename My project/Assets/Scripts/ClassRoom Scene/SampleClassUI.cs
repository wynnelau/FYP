using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the SampleClassUI
 */

public class SampleClassUI : MonoBehaviour
{
    public GameObject sampleClassUI;
    public void CloseSampleClassUI()
    {
        sampleClassUI.SetActive(false);
    }

    public void OpenSampleClassUI()
    {
        sampleClassUI.SetActive(true);
    }
}

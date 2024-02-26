using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the SampleQuizUI
 */

public class SampleQuizUI : MonoBehaviour
{
    public GameObject sampleQuizUI;
    public void CloseSampleQuizUI()
    {
        sampleQuizUI.SetActive(false);
    }

    public void OpenSampleQuizUI()
    {
        sampleQuizUI.SetActive(true);
    }
}

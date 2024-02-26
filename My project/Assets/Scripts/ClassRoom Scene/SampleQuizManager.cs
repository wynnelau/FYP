using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the instantiation and destruction of the joinQuizBtn in both host and client side
 * Tutorial used: https://www.youtube.com/watch?v=HWPKlpeZUjM
 */

public class SampleQuizManager : MonoBehaviour
{
    public GameObject sampleQuizUI;
    public Canvas syncButtonCanvas;
    public GameObject joinQuizBtnPrefab;
    public Button enableSampleQuiz, disableSampleQuiz;
    private GameObject joinQuizBtn;

    public void EnableSampleQuiz()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            joinQuizBtn = Instantiate(joinQuizBtnPrefab);
            
            joinQuizBtn.GetComponent<NetworkObject>().Spawn();
            joinQuizBtn.GetComponent<Button>().onClick.AddListener(() => { Debug.Log("Add listener"); sampleQuizUI.SetActive(true); });
            joinQuizBtn.transform.SetParent(syncButtonCanvas.transform, false);
            

        }
        enableSampleQuiz.gameObject.SetActive(false);
        disableSampleQuiz.gameObject.SetActive(true);
    }

    public void DisableSampleQuiz()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Destroy(joinQuizBtn);
        }
        enableSampleQuiz.gameObject.SetActive(true);
        disableSampleQuiz.gameObject.SetActive(false);
    }
}

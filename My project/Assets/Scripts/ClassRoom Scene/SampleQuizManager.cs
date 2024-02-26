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
            if (syncButtonCanvas != null)
            {
                Debug.Log("Canvas reference is not null.");

                joinQuizBtn = Instantiate(joinQuizBtnPrefab);
                if (joinQuizBtn != null)
                {
                    Debug.Log("Button instantiated");

                    

                    joinQuizBtn.GetComponent<Button>().onClick.AddListener(() => { sampleQuizUI.SetActive(true); });
                    
                    
                }
                else
                {
                    Debug.LogError("Failed to instantiate button.");
                }
            }
            else
            {
                Debug.LogError("Canvas reference is null!");
                return;
            }
            joinQuizBtn.GetComponent<NetworkObject>().Spawn();
            joinQuizBtn.transform.SetParent(syncButtonCanvas.transform, false);
            if (joinQuizBtn.transform.parent == syncButtonCanvas.transform)
            {
                Debug.Log("Button parent is syncButtonCanvas");
            }
            else
            {
                Debug.LogWarning("Button parent is not syncButtonCanvas");
            }

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

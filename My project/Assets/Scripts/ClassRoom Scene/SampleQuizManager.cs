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

public class SampleQuizManager : NetworkBehaviour
{
    public GameObject sampleQuizUI;
    public Button enableSampleQuiz, disableSampleQuiz, joinQuizBtn;

    public void EnableSampleQuiz()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            ActivateJoinQuizBtnClientRpc();
        }

        enableSampleQuiz.gameObject.SetActive(false);
        disableSampleQuiz.gameObject.SetActive(true);
    }

    [ClientRpc]
    void ActivateJoinQuizBtnClientRpc()
    {
        joinQuizBtn.gameObject.SetActive(true);
    }

    public void DisableSampleQuiz()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            DeactivateJoinQuizBtnClientRpc();
        }
        enableSampleQuiz.gameObject.SetActive(true);
        disableSampleQuiz.gameObject.SetActive(false);
    }

    [ClientRpc]
    void DeactivateJoinQuizBtnClientRpc()
    {
        joinQuizBtn.gameObject.SetActive(false);
    }
}

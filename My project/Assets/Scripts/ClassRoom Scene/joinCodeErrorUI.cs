using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the joinCodeErrorUI
 */

public class JoinCodeErrorUI : MonoBehaviour
{
    private GameObject joinCodeError;

    /*
     * Purpose: Return to Main Scene when the "ReturnToMainSceneBtn" is clicked
     * Input: Click on "ReturnToMainSceneBtn" button
     * Output: Return to Main Scene
     */
    public void returnToMainScene()
    {
        joinCodeError = GameObject.Find("JoinCodeError");
        joinCodeError.SetActive(false);
        Destroy(NetworkManager.Singleton.gameObject);
        SceneManager.LoadScene("Main Scene");
    }
}

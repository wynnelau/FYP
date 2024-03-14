using UnityEngine;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the EnableQuizUI
 */

public class EnableQuizUI : MonoBehaviour
{
    public GameObject enableQuizUI;

    /*
     * Purpose: Close the EnableQuiz UI
     * Input: Click on "X" button in EnableQuiz UI
     * Output: Close the EnableQuiz UI
     */
    public void CloseEnableQuizUI()
    {
        enableQuizUI.SetActive(false);
    }

    /*
     * Purpose: Enable the EnableQuiz UI
     * Input: Click on "enableQuizzes" button in NetworkManager UI
     * Output: Enable the EnableQuiz UI
     */
    public void OpenEnableQuizUI()
    {
        enableQuizUI.SetActive(true);
    }
}

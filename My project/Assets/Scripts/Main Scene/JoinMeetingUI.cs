using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

public class JoinMeetingUI : MonoBehaviour
{
    public GameObject joinMeetingUI;
    public PlayerControls player;
    public InputField joinCodeInput;

    /*
     * Purpose: Close the joinMeetingUI when the "closeJoinMeetingButton" button is clicked
     * Input: Click on the "closeJoinMeetingButton" button
     * Output: Close the joinMeetingUI and enable player controls
     */
    public void closeJoinMeeting()
    {
        Debug.Log("JoinMeetingUI closeJoinMeeting");
        joinMeetingUI.SetActive(false);
        player.enabled = true;
    }

    public void JoinMeeting()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                    {
                        {"JoinCode", joinCodeInput.text}
                    }
        };
        PlayFabClientAPI.UpdateUserData(request, UpdateJoinCodeSuccess, UpdateJoinCodeFail);
    }

    void UpdateJoinCodeSuccess(UpdateUserDataResult result)
    {
        Debug.Log("JoinMeetingUI UpdateJoinCodeSuccess");
        SceneManager.LoadScene("ClassRoom Scene");
    }

    void UpdateJoinCodeFail(PlayFabError error)
    {
        Debug.Log("JoinMeetingUI UpdateJoinCodeFail" + error);
    }





}

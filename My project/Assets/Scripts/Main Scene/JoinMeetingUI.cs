using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the JoinMeetingUI
 */
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

    /*
     * Purpose: Save the join code entered to PlayFab when "joinMeetingButton" button is clicked
     *          Join code saved will be used in the ClassRoom Scene
     * Input: Click on the "closeJoinMeetingButton" button
     * Output: Attempt to save the join code to PlayFab
     */
    public void JoinMeeting()
    {
        Debug.Log("JoinMeetingUI JoinMeeting");
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
                    {
                        {"JoinCode", joinCodeInput.text}
                    }
        };
        PlayFabClientAPI.UpdateUserData(request, UpdateJoinCodeSuccess, UpdateJoinCodeFail);
    }

    /*
     * Purpose: Load the ClassRoom Scene when attempt to save join code is successful
     * Input: Attempt to save join code is successful
     * Output: Load the ClassRoom Scene
     */
    void UpdateJoinCodeSuccess(UpdateUserDataResult result)
    {
        Debug.Log("JoinMeetingUI UpdateJoinCodeSuccess");
        SceneManager.LoadScene("ClassRoom Scene");
    }

    /*
    * Purpose: Log the error when attempt to save join code failed
    * Input: Attempt to save join code failed
    * Output: Log the error
    */
    void UpdateJoinCodeFail(PlayFabError error)
    {
        Debug.Log("JoinMeetingUI UpdateJoinCodeFail" + error);
    }





}

using PlayFab.ClientModels;
using PlayFab;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using System;

// Tutorial used: https://www.youtube.com/watch?v=msPNJ2cxWfw
public class Relay : MonoBehaviour
{
    public Text displayJoinCode;
    private string joinCodeParticipant, joinCodeHost;
    public RealmControllerClassRoom RealmController;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

    /*
     * Purpose: Used to start the meeting and then updates the join code
     * Input: Click on the "RelayBtn"
     * Output: Attempt to start meeting and then update the join code
     */
    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(40);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            displayJoinCode.text = joinCode;
            joinCodeHost = joinCode;
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
            UpdateJoinCode(joinCode);
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
    }

    public void StopRelay()
    {
        
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }

    /*
     * Purpose: Attempt to update the join code of the meeting to MongoDB by getting the meetingId
     * Input: Called by CreateRelay()
     * Output: Attempt to retrieve the meetingID
     */
    void UpdateJoinCode(string joinCode)
    {
        Debug.Log("Relay UpdateJoinCode");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, UpdateJoinCodeSuccess, UpdateJoinCodeFail);
    }

    /*
     * Purpose: Successful attempt to retrieve the meetingId, then tries to update join code of the meeting to MongoDB
     * Input: Called by UpdateJoinCode() when attempt to retrieve the meetingId is successful
     * Output: 
     */
    void UpdateJoinCodeSuccess(GetUserDataResult result)
    {
        Debug.Log("Relay UpdateJoinCodeSuccess " + joinCodeHost);
        string meetingId = result.Data["MeetingID"].Value;
        Debug.Log("Relay UpdateJoinCodeSuccess " + meetingId);
        RealmController = FindObjectOfType<RealmControllerClassRoom>();
        RealmController.UpdateMeetingDetails(meetingId, joinCodeHost);
    }

    /*
    * Purpose: Failed attempt to retrieve meetiingID
    * Input: Called by UpdateJoinCode()
    * Output: Debug.Log("Relay UpdateJoinCodeFail " + error);
    */
    void UpdateJoinCodeFail(PlayFabError error)
    {
        Debug.Log("Relay UpdateJoinCodeFail " + error);

    }

    /*
     * Purpose: Attempt to retrieve the user's join code from the PlayFab database when "EnterRoomBtn" is clicked
     * Input: Click on "EnterRoomBtn"
     * Output: Attempt to retrieve the user's join code from the PlayFab database
     */
    public void JoinMeeting()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, GetJoinCodeSuccess, GetJoinCodeFail);
    }

    /*
     * Purpose: Successful attempt to retrieve the user's join code to join a meeting
     * Input: Called by joinMeeting() when attempt to retrieve the user's join code is successful
     * Output: Call JoinRelay to join a meeting
     */
    void GetJoinCodeSuccess(GetUserDataResult result)
    {
        joinCodeParticipant = result.Data["JoinCode"].Value;
        Debug.Log("Relay GetJoinCodeSuccess " + joinCodeParticipant);
        JoinRelay(joinCodeParticipant);
    }

    /*
    * Purpose: Failed attempt to retrieve user's join code
    * Input: Called by joinMeeting()
    * Output: Debug.Log("Relay GetJoinCodeFail " + error);
    */
    void GetJoinCodeFail(PlayFabError error)
    {
        Debug.Log("Relay GetJoinCodeFail " + error);
    }

    /*
     * Purpose: Attempt to join the meeting
     * Input: Called by GetJoinCodeSuccess
     * Output: Join the meeting
     */
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining Relay with " +  joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        } 
    }
}

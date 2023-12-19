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

// Tutorial used: https://www.youtube.com/watch?v=msPNJ2cxWfw
public class Relay : MonoBehaviour
{
    public Text displayJoinCode;
    private string joinCodeString;
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(40);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("JoinCode " + joinCode);
            displayJoinCode.text = joinCode;
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
        }
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
        joinCodeString = result.Data["JoinCode"].Value;
        Debug.Log("Relay GetJoinCodeSuccess " + joinCodeString);
        JoinRelay(joinCodeString);
    }

    /*
    * Purpose: Failed attempt to retrieve user's join code
    * Input: Called by joinMeeting
    * Output: Debug.Log("Relay GetJoinCodeFail " + error);
    */
    void GetJoinCodeFail(PlayFabError error)
    {
        Debug.Log("Relay GetJoinCodeFail " + error);
    }

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

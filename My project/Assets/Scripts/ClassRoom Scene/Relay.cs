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
using UnityEngine.SceneManagement;
using Unity.Services.Vivox;


/*
 * Location: ClassRoom Scene/ NetworkManager
 * Purpose: Manage the start meeting and join meeting
 * Tutorial used: https://www.youtube.com/watch?v=msPNJ2cxWfw
 */

public class Relay : MonoBehaviour
{
    public Text displayJoinCode;
    public GameObject joinCodeError;
    public RealmControllerClassRoom RealmControllerClassRoom;
    public Button startMeetingButton, endMeetingButton, enableAudio, enableQuizBtn, enableClassesBtn;

    private string email;
    private string channelName;

    /*
     * Purpose: Ensures access to multiplayer functions when user enters "ClassRoom" Scene
     *          Start is called before the first frame update
     * Input: NA
     * Output: Initialise Unity Gaming Services, followed by an anonymous sign-in to the session and an initialization of Vivox
     */
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Relay Start AuthenticationService SignInAnonAsync " + AuthenticationService.Instance.PlayerId);
        }
        catch (AuthenticationException ex)
        {
            Debug.Log(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.Log(ex);
        }

        try
        {
            await VivoxService.Instance.InitializeAsync();
            Debug.Log("Relay Start VivoxService InitializeAsync");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    /*
     * Purpose: Attempt to retrieve the user's meeting status from the PlayFab database when "Start" button is clicked
     * Input: Click on "Start" button in NetworkManager UI
     * Output: Attempt to retrieve the user's meeting status from the PlayFab database
     */
    public void EnterMeetingRoom()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, EnterMeetingRoomSuccess, EnterMeetingRoomFail);
    }

    /*
     * Purpose: Successful attempt to retrieve the user's meeting status
     * Input: Called by EnterMeetingRoom() when attempt to retrieve the user's meeting status is successful
     * Output: Call CreateRelay() when MeetingStatus is Host
     *         Call JoinRelay() to join a meeting when MeetingStatus is Participant
     */
    void EnterMeetingRoomSuccess(GetUserDataResult result)
    {
        if (result.Data["MeetingStatus"].Value == "Host")
        {
            Debug.Log("Relay EnterMeetingRoomSuccess Host");
            string meetingId = result.Data["MeetingID"].Value;
            email = result.Data["Email"].Value;
            CreateRelay(meetingId);
        }
        else if (result.Data["MeetingStatus"].Value == "Participant")
        {
            Debug.Log("Relay EnterMeetingRoomSuccess Participant");
            string joinCodeParticipant = result.Data["JoinCode"].Value;
            email = result.Data["Email"].Value;
            JoinRelay(joinCodeParticipant);
        }
    }

    /*
    * Purpose: Failed attempt to retrieve user's meeting status
    * Input: Called by EnterMeetingRoom()
    * Output: Debug.Log("Relay EnterMeetingRoomFail " + error);
    */
    void EnterMeetingRoomFail(PlayFabError error)
    {
        Debug.Log("Relay EnterMeetingRoomFail " + error);
    }

    /*
     * Purpose: Used to start the meeting, update the join code and update MeetingAttendees
     * Input: Called by EnterMeetingRoomSuccess()
     * Output: Attempt to start meeting and then update the join code and MeetingAttendees
     */
    async void CreateRelay(string meetingId)
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(40);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            displayJoinCode.text = "Join code: \n" + joinCode;
            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartHost();

            RealmControllerClassRoom = FindObjectOfType<RealmControllerClassRoom>();
            RealmControllerClassRoom.UpdateMeetingDetails(meetingId, joinCode);
            RealmControllerClassRoom.UpdateMeetingAttendeesHost(meetingId, email);

            startMeetingButton.gameObject.SetActive(false);
            endMeetingButton.gameObject.SetActive(true);
            enableAudio.gameObject.SetActive(true);
            enableQuizBtn.gameObject.SetActive(true);
            enableClassesBtn.gameObject.SetActive(true);

            channelName = meetingId;
            JoinChannel();
        }
        catch (RelayServiceException e)
        {
            Debug.Log("Relay CreateRelay error:" + e);
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("Main Scene");
        }
    }

    /*
     * Purpose: Used to join the meeting and update MeetingAttendees
     * Input: Called by EnterMeetingRoomSuccess and joinCode is passed in
     * Output: Join the meeting successfully and update MeetingAttendees
     */
    async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
            NetworkManager.Singleton.StartClient();

            RealmControllerClassRoom = FindObjectOfType<RealmControllerClassRoom>();
            string meetingId = RealmControllerClassRoom.GetMeetingDetails(joinCode);
            RealmControllerClassRoom.UpdateMeetingAttendeesParticipant(meetingId, email);

            startMeetingButton.gameObject.SetActive(false);
            endMeetingButton.gameObject.SetActive(true);
            enableAudio.gameObject.SetActive(true);

            channelName = meetingId;
            JoinChannel();
        }
        catch (RelayServiceException e)
        {
            Debug.Log("Relay JoinRelay error: " + e);
            joinCodeError.SetActive(true);
        }
    }

    /*
     * Purpose: Used to join the meeting's audio channel
     * Input: Called by JoinRelay()
     * Output: Join the meeting's audio channel 
     */
    async void JoinChannel()
    {
        try
        {
            await VivoxService.Instance.JoinGroupChannelAsync(channelName, ChatCapability.TextAndAudio);
            VivoxService.Instance.MuteInputDevice();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    /*
    * Purpose: Used to stop or exit the meeting
    * Input: Click on "End" button in NetworkManager UI
    * Output: If user is host, shutdown the server, destroy the NetworkManager and call RemoveJoinCode()
    *         else destroy the NetworkManager and load Main Scene
    */
    public void StopRelay()
    {
        VivoxService.Instance.MuteInputDevice();
        VivoxService.Instance.LeaveChannelAsync(channelName);
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            RemoveJoinCode();
        }
        else
        {
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("Main Scene");
        }
    }

    /*
     * Purpose: Used to retrieve the meetingId from the PlayFab database
     * Input: Called by StopRelay()
     * Output: Attempt to retrieve the meetingId from the PlayFab database
     */
    void RemoveJoinCode()
    {
        Debug.Log("Relay RemoveJoinCode");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, RemoveJoinCodeSuccess, RemoveJoinCodeFail);
    }

    /*
     * Purpose: Successful attempt to retrieve the meetingId, then tries to update join code of the meeting to MongoDB
     * Input: Called by RemoveJoinCode() when attempt to retrieve the meetingId is successful
     * Output: Update "Meeting ended" to the join code of the meeting to MongoDB and load Main Scene
     */
    void RemoveJoinCodeSuccess(GetUserDataResult result)
    {
        string meetingId = result.Data["MeetingID"].Value;
        Debug.Log("Relay RemoveJoinCodeSuccess " + meetingId);
        RealmControllerClassRoom = FindObjectOfType<RealmControllerClassRoom>();
        RealmControllerClassRoom.UpdateMeetingDetails(meetingId, "Meeting ended");
        SceneManager.LoadScene("Main Scene");
    }

    /*
    * Purpose: Failed attempt to retrieve the meetingId
    * Input: Called by RemoveJoinCode()
    * Output: Debug.Log("Relay RemoveJoinCodeFail " + error);
    */
    void RemoveJoinCodeFail(PlayFabError error)
    {
        Debug.Log("Relay RemoveJoinCodeFail " + error);
    }

    

}

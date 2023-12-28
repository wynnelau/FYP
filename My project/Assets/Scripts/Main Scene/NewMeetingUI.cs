using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class NewMeetingUI : MonoBehaviour
{
    public GameObject NewMeeting, MeetingSchedule;
    public InputField participantEmail;

    /*
     * Purpose: To close the NewMeetingUI
     * Input: Click on the "closeNewMeetingButton" button in the NewMeetingUI
     * Output: Close NewMeetingUI and open the MeetingScheduleUI
     */
    public void closeNewMeeting()
    {
        Debug.Log("NewMeetingUI closeNewMeeting");
        NewMeeting.SetActive(true);
        MeetingSchedule.SetActive(false);
    }

    /*
     * Purpose: Check whether the email/ user exists
     * Input: Click on AddUser button
     * Output: Check whether email/ user exists using email
     */
    public void addParticipant()
    {
        Debug.Log("NewMeetingUI addParticipant " + participantEmail.text);
        // Conversion required here as email in PlayFab database contains \xe2\x80\x8b at the end
        string email = StringToHex(participantEmail.text);
        string convertedEmail = HexToString(email + "e2808b");
        var request = new GetAccountInfoRequest { Email = convertedEmail };
        PlayFabClientAPI.GetAccountInfo(request, AddParticipantSuccess, AddParticipantFail);
    }

    void AddParticipantSuccess(GetAccountInfoResult result)
    {
        Debug.Log(result.AccountInfo.PlayFabId);    
    }

    void AddParticipantFail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport()); 
    }

    /*
     * Purpose: Convert email string to hex string
     * Input: Pass the email string in
     * Output: return the hex string out
     */
    string StringToHex(string email)
    {
        StringBuilder hex = new StringBuilder(email.Length * 2);
        foreach (char c in email)
        {
            hex.AppendFormat("{0:X2}", (int)c);
        }

        return hex.ToString();
    }

    /*
     * Purpose: Convert hex string to email string
     * Input: Pass the hex string in
     * Output: return the email string out
     */
    string HexToString(string hex)
    {
        byte[] bytes = new byte[hex.Length / 2];

        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }

        return Encoding.UTF8.GetString(bytes);
    }
}

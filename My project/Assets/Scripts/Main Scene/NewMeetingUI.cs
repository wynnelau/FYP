using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;

public class NewMeetingUI : MonoBehaviour
{
    public GameObject NewMeeting, MeetingSchedule;
    public InputField participantEmail, dDate, dMonth, dYear, duration, description;
    public Text errorText, participantList;
    public Dropdown tHr, tMin, tAM;

    private List<string> emailList = new List<string>();

    /*
     * Purpose: To close the NewMeetingUI
     * Input: Click on the "closeNewMeetingButton" button in the NewMeetingUI
     * Output: Close NewMeetingUI and open the MeetingScheduleUI
     */
    public void closeNewMeeting()
    {
        Debug.Log("NewMeetingUI closeNewMeeting");

        errorText.text = "";
        participantList.text = "";

        participantEmail.text = "";
        dDate.text = "";
        dMonth.text = "";
        dYear.text = "";
        duration.text = "";
        description.text = "";

        tHr.value = 0;
        tMin.value = 0;
        tAM.value = 0;

        emailList.Clear();

        NewMeeting.SetActive(false);
        MeetingSchedule.SetActive(true);
    }

    /*
     * Purpose: Check whether the email/ user exists
     * Input: Click on AddUser button
     * Output: Check whether email/ user exists using email
     */
    public void AddParticipant()
    {
        Debug.Log("NewMeetingUI AddParticipant " + participantEmail.text);
        // Conversion required here as email in PlayFab database contains \xe2\x80\x8b at the end
        string email = StringToHex(participantEmail.text);
        string convertedEmail = HexToString(email + "e2808b");
        var request = new GetAccountInfoRequest { Email = convertedEmail };
        PlayFabClientAPI.GetAccountInfo(request, AddParticipantSuccess, AddParticipantFail);
    }

    /*
     * Purpose: Add the email to a list if the email/ user exists
     * Input: Email/ user exists
     * Output: Add email to list of emails or if already in list, return message
     */
    void AddParticipantSuccess(GetAccountInfoResult result)
    {
        Debug.Log("NewMeetingUI AddParticipantSuccess");
        if (emailList.Contains(participantEmail.text))
        {
            errorText.text = participantEmail.text + " already added";
        } 
        else
        {
            emailList.Add(participantEmail.text);
            errorText.text = participantEmail.text + " successfully added";
            participantList.text += participantEmail.text + "; ";
            participantEmail.text = "";
        }
        
        
    }

    /*
     * Purpose: Return an error message if email/ user does not exist
     * Input: Email/ user does not exists
     * Output: Return an error message
     */
    void AddParticipantFail(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        errorText.text = error.GenerateErrorReport();
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

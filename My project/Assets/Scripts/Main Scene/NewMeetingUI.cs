using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using System.Collections.Generic;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the NewMeetings UI
 */

public class NewMeetingUI : MonoBehaviour
{
    public GameObject NewMeeting, MeetingSchedule;
    public InputField participantEmail, dDate, dMonth, dYear, duration, description, tHr, tMin;
    public Text errorText, participantList;
    public Dropdown tAM;
    public RealmController RealmController;

    private List<string> emailList = new List<string>();

    /*
     * Purpose: To close the NewMeetingUI
     * Input: Click on the "closeNewMeetingButton" button in the NewMeetingUI
     * Output: Close NewMeetingUI and open the MeetingScheduleUI
     */
    public void CloseNewMeeting()
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
        tHr.text = "";
        tMin.text = "";

        tAM.value = 0;

        emailList.Clear();

        NewMeeting.SetActive(false);
        MeetingSchedule.SetActive(true);
    }

    /*
     * Purpose: To attempt to create a new meeting by calling AddMeeting when "createMeeting" button clicked
     * Input: Click on the "createMeeting" button
     * Output: Call AddMeeting() and pass the emailList
     */
    public void CreateMeeting()
    {
        Debug.Log("NewMeetingUI CreateMeeting");
        RealmController = FindObjectOfType<RealmController>();
        RealmController.AddMeeting(emailList);
    }

    /*
     * Purpose: Check whether the email/ user exists
     * Input: Click on AddUser button
     * Output: Check whether email/ user exists using email
     */
    public void AddParticipant()
    {
        Debug.Log("NewMeetingUI AddParticipant " + participantEmail.text);
        
        string email = StringToHex(participantEmail.text);
        var request = new GetAccountInfoRequest { Email = email };
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
            emailList.Add(StringToHex(participantEmail.text));
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
     * Purpose: Convert as email in PlayFab database contains \xe2\x80\x8b at the end
     * Input: Pass the email string in
     * Output: return the converted string out
     */
    public string StringToHex(string email)
    {
        StringBuilder hex = new StringBuilder(email.Length * 2);
        foreach (char c in email)
        {
            hex.AppendFormat("{0:X2}", (int)c);
        }
        string hexString = hex.ToString();
        hexString += "e2808b";

        byte[] bytes = new byte[hexString.Length / 2];

        for (int i = 0; i < hexString.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }

        return Encoding.UTF8.GetString(bytes);
    }

   
    
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.UI;
using MongoDB.Bson;
using System.Text;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage send and retrieve data from MongoDB
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c, https://www.youtube.com/watch?v=q4_997QEQww
 */
public class RealmControllerClassRoom : MonoBehaviour
{
    private Realm realm;
    private readonly string myRealmAppId = "application-0-nlkew";
    private bool isRealmInitialized = false;

    private void Start()
    {
        RealmController RealmController = FindObjectOfType<RealmController>();
        if (RealmController != null)
        {
            realm = RealmController.RealmInstance;
            if (realm == null)
            {
                Debug.Log("RealmControllerClassRoom Start RealmIsNull");
            }
            else
            {
                isRealmInitialized = true;
            }
            
        }
        else
        {
            Debug.LogWarning("RealmControllerClassRoom not found or not initialized in the new scene.");
        }
    }
    /*void Start()
    {
        try
        {
            realm = Realm.GetInstance();
            isRealmInitialized = true;
        }
        catch (Exception e)
        {
            Debug.Log("RealmControllerClassRoom Start" + e);
        }

        if(realm == null)
        {
            Debug.Log("RealmControllerClassRoom Start RealmIsNull");
        }
        else
        {
            Debug.Log("RealmControllerClassRoom Start RealmInstance: " + realm);
        }
        
    }*/

    /*
     * Purpose: Update meeting details according to objectId passed in
     * Input: Called by Relay using UpdateJoinCodeSuccess()
     * Output: Call PerformRealmWriteUpdateMeetingDetails
     *         return if there is an error
     */
    public void UpdateMeetingDetails(string objectID, string joinCode)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmControllerClassRoom UpdateMeetingDetails RealmNotInitialized");
            return;
        }
        /*string meetingId = StringToHex(objectID);*/
        StartCoroutine(PerformRealmWriteUpdateMeetingDetails(objectID, joinCode));

    }

    /*
     * Purpose: Update the Meeting details according to the objectId
     * Input: Called by UpdateMeetingDetails()
     * Output: Update Meeting details
     */
    private IEnumerator PerformRealmWriteUpdateMeetingDetails(string objectID, string joinCode)
    {
        try
        {
            realm.Write(() =>
            {
                Meetings result = realm.Find<Meetings>(ObjectId.Parse(objectID));
                if (result != null)
                {
                    // Update the JoinCode property
                    result.JoinCode = joinCode;
                    Debug.Log("Code updated");
                }
                else
                {
                    // Handle the case where the object with the specified ID is not found
                    Debug.LogError($"Meetings object with ID {objectID} not found.");
                }

            });
            
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails Completed.");
    }

    

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

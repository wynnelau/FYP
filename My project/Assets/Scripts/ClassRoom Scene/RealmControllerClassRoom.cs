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
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage send and retrieve data from MongoDB
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c, https://www.youtube.com/watch?v=q4_997QEQww
 */
public class RealmControllerClassRoom : MonoBehaviour
{
    private Realm realm;
    private bool isRealmInitialized = false;

    /*
     * Purpose: Get the realm instance from the previous scene
     * Input: NA
     * Output: Get the realm instance
     */
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
                Debug.Log("RealmControllerClassRoom Start RealmInitialised");
            }
            
        }
        else
        {
            Debug.LogWarning("RealmControllerClassRoom Start RealmControllerIsNull");
        }
    }
    

    /*
     * Purpose: Update meeting details according to objectId and joinCode passed in
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

        StartCoroutine(PerformRealmWriteUpdateMeetingDetails(objectID, joinCode));

    }

    /*
     * Purpose: Update the Meeting details according to the objectId and joinCode
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
                result.JoinCode = joinCode;
            });
            
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmControllerClassRoom PerformRealmWriteUpdateMeetingDetails Completed.");
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using UnityEngine;
using Realms.Logging;
using Logger = Realms.Logging.Logger;
using Realms.Sync.ErrorHandling;
using Realms.Sync.Exceptions;
using System.Linq;
using System;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;
using PlayFab.Internal;

/*
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c
 */
public class RealmController : MonoBehaviour
{
    private Realm realm;
    private readonly string myRealmAppId = "application-0-nlkew";
    private bool isRealmInitialized = false;
    public InputField location, fromDate, fromMonth, fromYear, toDate, toMonth, toYear;
    public Dropdown fromHr, fromMin, fromAm, toHr, toMin, toAm;
    private async void Start()
    {
        await InitAsync();
    }

    private async Task InitAsync()
    {
        Debug.Log("Init");
        var app = App.Create(myRealmAppId);
        var credential = Credentials.Anonymous();
        try
        {
            await app.LogInAsync(credential); 
            var config = new PartitionSyncConfiguration("FYP", app.CurrentUser);
            realm = await Realm.GetInstanceAsync(config);
            isRealmInitialized = true;
        }
        catch (Exception ex)
        {
            Debug.Log("Login failed: " + ex.Message);
        }
    }

    public void AddAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot addAvailable.");
            return;
        }

        Debug.Log("Adding available to Realm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAdd());
    }

    public void RemoveAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot removeAvailable.");
            return;
        }

        Debug.Log("Remove available to Realm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteRemove());
    }

    private IEnumerator PerformRealmWriteRemove()
    {
        try
        {
            realm.Write(() =>
            {
                realm.RemoveAll();
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("Error writing to Realm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation remove completed.");
    }

    private IEnumerator PerformRealmWriteAdd()
    {
        string loc = location.text;
        int date = int.Parse(fromDate.text); 
        int month = int.Parse(fromMonth.text);
        int year = int.Parse(fromYear.text);
        int hr = GetHr(fromAm.value, fromHr.value, false);
        int min = fromMin.value == 0 ? 0 : 30;
        int noOfDays = DateTime.DaysInMonth(2000+year, month);
        
        while (date != int.Parse(toDate.text) || month != int.Parse(toMonth.text) || year != int.Parse(toYear.text) || hr != GetHr(toAm.value, toHr.value, true) || min != (toMin.value == 0 ? 0 : 30))
        {
            hr = GetHr(fromAm.value, fromHr.value, false);
            min = fromMin.value == 0 ? 0 : 30;

            while (hr != GetHr(toAm.value, toHr.value, true) || min != (toMin.value == 0 ? 0 : 30) )
            {
                // This code block will run on the main/UI thread
                try
                {
                    realm.Write(() =>
                    {
                        realm.Add(new Available(loc, date, month, year, hr, min));
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogError("Error writing to Realm: " + ex.Message);
                }

                if (min == 0)
                {
                    min = 30;
                }
                else if (min == 30)
                {
                    min = 0;
                    hr++;
                }
            }
            
            if (date != int.Parse(toDate.text) || month != int.Parse(toMonth.text) || year != int.Parse(toYear.text))
            {
                if (date < noOfDays)
                {
                    date++;
                }
                else if (date == noOfDays && month < 12)
                {
                    date = 1;
                    month++;
                    noOfDays = DateTime.DaysInMonth(2000 + year, month);
                }
                else if (date == noOfDays && month == 12)
                {
                    date = 1;
                    month = 1;
                    year++;
                    noOfDays = DateTime.DaysInMonth(2000 + year, month);
                }
                hr = GetHr(fromAm.value, fromHr.value, false);
                min = fromMin.value == 0 ? 0 : 30;
            }

        }
        
        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation add completed.");
    }

    private int GetHr(int am, int hr, bool to)
    {
        if (to == true && am == 0 && hr == 11)
        {
            hr = 24;
            return hr;
        }

        if(am == 0 && hr < 11)
        {
            hr++;
        } 
        else if (am == 0 && hr == 11)
        {
            hr = 0;
        }
        else if (am == 1 && hr < 11)
        {
            hr += 13;
        }
        else if (am == 1 && hr == 11)
        {
            hr = 12;
        }
        return hr;
    }


}

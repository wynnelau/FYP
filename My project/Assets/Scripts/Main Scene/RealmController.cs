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

public class RealmController : MonoBehaviour
{
    private Realm realm;
    private readonly string myRealmAppId = "application-0-nlkew";
    private bool isRealmInitialized = false;
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
        StartCoroutine(PerformRealmWrite());
    }

    private IEnumerator PerformRealmWrite()
    {
        // This code block will run on the main/UI thread
        realm.Write(() =>
        {
            realm.Add(new Available());
        });

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation completed.");
    }
}

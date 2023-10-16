using System.Collections.Generic;
using System.Threading.Tasks;
using Realms;
using Realms.Sync;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using UnityEngine.UI;

/*
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c
 * Location: Main Scene, under StudentControls
 * Purpose: Manage send and retrieve data from MongoDB
 */
public class RealmController : MonoBehaviour
{
    private Realm realm;
    private readonly string myRealmAppId = "application-0-nlkew";
    private bool isRealmInitialized = false;
    public InputField location, fromDate, fromMonth, fromYear, toDate, toMonth, toYear;
    public Dropdown fromHr, fromMin, fromAm, toHr, toMin, toAm;
    public Text errorText;

    public Text dateTextProf, dateTextStaff, timeTextProf, timeTextStaff;
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

    /*
     * Purpose: Add available slots by taking in inputs
     * Outcomes: Call PerformRealmWriteAddAvailable if successful
     */
    public void AddAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot addAvailable.");
            return;
        }

        if (location.text == "" || fromDate.text == "" || fromMonth.text == "" || fromYear.text == "" || toDate.text == "" || toMonth.text == "" || toYear.text == "")
        {
            errorText.text = "Unable to add slots. Missing inputs.";
            return;
        }

        if (DatetimeError())
        {
            errorText.text = "Unable to add slots. Please make sure fromdate/time is earlier than todate/time";
            Debug.Log("DateTime error");
            return;
        }

        errorText.text = "Adding slots";
        Debug.Log("Adding available to Realm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAddAvailable());

        errorText.text = "Adding slots complete";
    }

    /*
     * Purpose: Add available reservations by taking in inputs
     * Outcomes: Call PerformRealmWriteAddReservation if successful
     */
    public void AddReservation()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot addAvailable.");
            return;
        }

        Debug.Log("Adding reservation to Realm");

        StartCoroutine(PerformRealmWriteAddReservation());

        errorText.text = "Adding slots complete";

    }

    /*
     * Purpose: Remove available slots by taking in inputs
     * Outcomes: Call PerformRealmWriteRemoveAvailable if successful
     */
    public void RemoveAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot removeAvailable.");
            return;
        }

        if (location.text == "" || fromDate.text == "" || fromMonth.text == "" || fromYear.text == "" || toDate.text == "" || toMonth.text == "" || toYear.text == "")
        {
            errorText.text = "Unable to add slots. Missing inputs.";
            return;
        }

        if (DatetimeError())
        {
            errorText.text = "Unable to remove slots. Please make sure fromdate/time is earlier than todate/time";
            Debug.Log("DateTime error");
            return;
        }

        errorText.text = "Removing slots";
        Debug.Log("Remove available from Realm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteRemoveAvailable());

        errorText.text = "Removing slots complete";
    }

    /*public void GetAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot removeAvailable.");
            return;
        }

        var queryResults = PerformRealmWriteRetrieveAvailable();
        var sortedResults = queryResults.OrderBy(item => item.Location) 
            .ThenBy(item => item.Hour)       
            .ThenBy(item => item.Min)        
            .ToList();
    }*/

    /*
     * Purpose: Get the reservations according to the location
     * Outcomes: Call PerformRealmWriteRetrieveReservation if successful
     */
    public List<string> GetReservations(string location)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot removeAvailable.");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveReservation();
        var timingList = queryResults
            .Where(item => item.Location == location)
            .OrderBy(item => item.Hour)
            .ThenBy(item => item.Min)
            .Select(item => item.Hour.ToString() + ":" + item.Min.ToString())
            .ToList();
        return timingList;
    }

    /*
     * Purpose: Get the list of locations
     * Outcomes: Call PerformRealmWriteRetrieveAvailable if successful, and returns ordered string list of locations
     */
    public List<string> GetLocations()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot GetLocations.");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveAvailable();
        var locationList = queryResults
            .OrderBy(item => item.Location)
            .Select(item => item.Location)
            .Distinct()
            .ToList();

        return locationList;
    }

    /*
     * Purpose: Get the list of timings according to the location
     * Outcomes: Call PerformRealmWriteRetrieveAvailable if successful, and returns ordered string list of timings
     */
    public List<string> GetTimings(string location)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot GetTimings.");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveAvailable();
        var timingList = queryResults
            .Where(item => item.Location == location)
            .OrderBy(item => item.Hour)
            .ThenBy(item => item.Min)
            .Select(item => item.Hour.ToString() + ":" + item.Min.ToString())
            .ToList();
        Debug.Log(timingList);
        return timingList;
    }

    /*private IEnumerator PerformRealmWriteRemove2()
    {
        realm.Write(() =>
        {
            realm.RemoveAll<Available>(); // Remove all objects of the specified type
        });

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation remove completed.");
    }*/

    /*
     * Purpose: Called by AddAvailable and RemoveAvailable, to make sure from < to
     * Outcomes: return true if there is an error
     */
    private bool DatetimeError()
    {
        DateTime from = new DateTime(2000 + int.Parse(fromYear.text), int.Parse(fromMonth.text), int.Parse(fromDate.text));
        DateTime to = new DateTime(2000 + int.Parse(toYear.text), int.Parse(toMonth.text), int.Parse(toDate.text));
        Debug.Log(from.ToString());
        Debug.Log(to.ToString());
        if (DateTime.Compare(from, to) > 0)
        {
            Debug.Log("from is later");
            return true;
        } 
        else if (DateTime.Compare(from, to) == 0)
        {
            Debug.Log("equal");
        } 
        else
        {
            Debug.Log("from is earlier");
        }
        int fhr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
        int fmin = fromMin.value == 0 ? 0 : 30;
        int thr = GetHr(toAm.value, toHr.value, toMin.value, true);
        int tmin = toMin.value == 0 ? 0 : 30;
        if (fhr > thr)
        {
            Debug.Log("fromHr is later" + fhr + " " + thr);
            return true;
        }
        else if (fhr == thr)
        {
            if (fmin == tmin)
            {
                Debug.Log("equal" + fhr + " " + fmin + " " + thr + " " + tmin);
                return true;
            } 
            else if (fmin < tmin)
            {
                Debug.Log("tmin later" + fhr + " " + fmin + " " + thr + " " + tmin);
                return false;
            }
        }
        else if (fhr < thr) 
        {
            Debug.Log("fromHr is earlier" + fhr + " " + thr);
            return false;
        }
        return true;
    }

    /*
     * Purpose: Separate the slots and write them to db
     * Outcomes: add slots to db
     */
    private IEnumerator PerformRealmWriteAddAvailable()
    {
        string loc = location.text;
        int date = int.Parse(fromDate.text); 
        int month = int.Parse(fromMonth.text);
        int year = int.Parse(fromYear.text);
        int hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
        int min = fromMin.value == 0 ? 0 : 30;
        int noOfDays = DateTime.DaysInMonth(2000+year, month);
        
        while (date != int.Parse(toDate.text) || month != int.Parse(toMonth.text) || year != int.Parse(toYear.text) || hr != GetHr(toAm.value, toHr.value, toMin.value, true) || min != (toMin.value == 0 ? 0 : 30))
        {
            hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
            min = fromMin.value == 0 ? 0 : 30;

            while (hr != GetHr(toAm.value, toHr.value, toMin.value, true) || min != (toMin.value == 0 ? 0 : 30) )
            {
                // This code block will run on the main/UI thread
                try
                {
                    realm.Write(() =>
                    {
                        var results = realm.All<Available>().FirstOrDefault(item => item.Location == loc && item.Date == date && item.Month == month && item.Year == year && item.Hour == hr && item.Min == min);
                        if (results == null) 
                        {
                            realm.Add(new Available(loc, date, month, year, hr, min));
                            Debug.Log("Added new");
                        }
                        
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
                hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
                min = fromMin.value == 0 ? 0 : 30;
            }

        }
        
        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation addAvailable completed.");
    }

    private IEnumerator PerformRealmWriteAddReservation()
    {


        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation addResrvation completed.");
    }
    /*
     * Purpose: Separate the slots and write them to db
     * Outcomes: remove slots from db
     */
    private IEnumerator PerformRealmWriteRemoveAvailable()
    {
        string loc = location.text;
        int date = int.Parse(fromDate.text);
        int month = int.Parse(fromMonth.text);
        int year = int.Parse(fromYear.text);
        int hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
        int min = fromMin.value == 0 ? 0 : 30;
        int noOfDays = DateTime.DaysInMonth(2000 + year, month);

        while (date != int.Parse(toDate.text) || month != int.Parse(toMonth.text) || year != int.Parse(toYear.text) || hr != GetHr(toAm.value, toHr.value, toMin.value, true) || min != (toMin.value == 0 ? 0 : 30))
        {
            hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
            min = fromMin.value == 0 ? 0 : 30;

            while (hr != GetHr(toAm.value, toHr.value, toMin.value, true) || min != (toMin.value == 0 ? 0 : 30))
            {
                // This code block will run on the main/UI thread
                try
                {
                    realm.Write(() =>
                    {
                        var results = realm.All<Available>().Where(item => item.Location == loc && item.Date == date && item.Month == month && item.Year == year && item.Hour == hr && item.Min == min).ToList();
                        foreach (var item in results)
                        {
                            realm.Remove(item);
                        }
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
                hr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
                min = fromMin.value == 0 ? 0 : 30;
            }

        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation remove completed.");
    }

    /*
     * Purpose: Get the available list according to the date, called by GetLocations and GetTimings
     * Outcomes: returns ordered available list according to the date
     */
    private List<Available> PerformRealmWriteRetrieveAvailable()
    {
        string dateText;
        if (dateTextProf.IsActive())
        {
            dateText = dateTextProf.text;
            Debug.Log("ProfText" + dateText);
        }
        else if (dateTextStaff.IsActive())
        {
            dateText = dateTextStaff.text;
            Debug.Log("StaffText" + dateText);
        }
        else if (timeTextProf.IsActive())
        {
            dateText = timeTextProf.text;
            Debug.Log("ProfText" + dateText);
        }
        else if (timeTextStaff.IsActive())
        {
            dateText = timeTextStaff.text;
            Debug.Log("StaffText" + dateText);
        }
        else return null;
        string[] parts = dateText.Split('/');
        int date = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int year = int.Parse(parts[2]) - 2000;

        try
        {
            List<Available> results = realm.All<Available>()
                .Where(item => item.Date == date && item.Month == month && item.Year == year)
                .ToList();
            Debug.Log(results);
            return results;

        }
        catch (Exception ex)
        {
            Debug.LogError("Error querying Realm: " + ex.Message);
            return null;
        }

        
    }

    /*
     * Purpose: Get the reserved list according to the date, called by GetReservations
     * Outcomes: returns ordered reserved list according to the date
     */
    private List<Reserved> PerformRealmWriteRetrieveReservation()
    {
        string dateText;
        if (timeTextProf.IsActive())
        {
            dateText = timeTextProf.text;
            Debug.Log("ProfText" + dateText);
        }
        else if (timeTextStaff.IsActive())
        {
            dateText = timeTextStaff.text;
            Debug.Log("StaffText" + dateText);
        }
        else return null;
        string[] parts = dateText.Split('/');
        int date = int.Parse(parts[0]);
        int month = int.Parse(parts[1]);
        int year = int.Parse(parts[2]) - 2000;
        try
        {
            List<Reserved> results = realm.All<Reserved>()
                .Where(item => item.Date == date && item.Month == month && item.Year == year)
                .ToList();
            Debug.Log(results);
            return results;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error querying Realm: " + ex.Message);
            return null;
        }
    }

    /*
     * Purpose: Called by PerfromRealmWriteAdd/Remove, to calculate the hr in 24hr
     * Outcomes: return hr in 24hr format
     */
    private int GetHr(int am, int hr, int min, bool to)
    {
        if (to == true && am == 0 && hr == 11 && min == 0)
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

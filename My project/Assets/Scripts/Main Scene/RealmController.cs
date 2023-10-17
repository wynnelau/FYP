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
 * Location: MainSceneControls
 * Purpose: Manage send and retrieve data from MongoDB
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c
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
        Debug.Log("RealmController InitAsync");
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
            Debug.Log("RealmController InitAsync Login failed: " + ex.Message);
        }
    }

    /*
     * Purpose: Check whether we are able to add available slots, then attempt to write to database
     * Input: Click on the "addSlotsButton" button in "manageSlots" UI
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to add Available
     */
    public void AddAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController AddAvailable RealmNotInitialised");
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
            Debug.Log("RealmController AddAvailable DateTimeError");
            return;
        }

        errorText.text = "Adding slots";
        Debug.Log("RealmController AddAvailable AddingAvailableToRealm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAddAvailable());

        errorText.text = "Adding slots complete";
    }

    /*
     * Purpose: Add available reservations by taking in inputs
     * Outcomes: Call PerformRealmWriteAddReservation if successful
     */
    /*public void AddReservation(List<Reserved> addReservationList)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("Realm initialization is not complete, cannot addAvailable.");
            return;
        }

        Debug.Log("Adding reservation to Realm");

        StartCoroutine(PerformRealmWriteAddReservation(addReservationList));

        errorText.text = "Adding slots complete";

    }*/

    /*
     * Purpose: Check whether we are able to remove available slots, then attempt to write to database
     * Input: Click on the "removeSlotsButton" button in "manageSlots" UI
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to remove Available
     */
    public void RemoveAvailable()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController RemoveAvailable RealmNotInitialised");
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
            Debug.Log("RealmController RemoveAvailable DateTimeError");
            return;
        }

        errorText.text = "Removing slots";
        Debug.Log("RealmController RemoveAvailable RemovingAvailableFromRealm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteRemoveAvailable());

        errorText.text = "Removing slots complete";
    }

    /*
     * Purpose: Check whether the date time input in "manageSlots" UI is correct
     * Input: Called by AddAvailable() and RemoveAvailable()
     * Output: return true if there is an error
     *         else return false
     */
    private bool DatetimeError()
    {
        DateTime from = new DateTime(2000 + int.Parse(fromYear.text), int.Parse(fromMonth.text), int.Parse(fromDate.text));
        DateTime to = new DateTime(2000 + int.Parse(toYear.text), int.Parse(toMonth.text), int.Parse(toDate.text));
        Debug.Log(from.ToString());
        Debug.Log(to.ToString());
        if (DateTime.Compare(from, to) > 0)
        {
            Debug.Log("RealmController DateTimeError FromDateIsLaterThanToDate");
            return true;
        }
        else if (DateTime.Compare(from, to) == 0)
        {
            Debug.Log("RealmController DateTimeError FromDateAndToDateIsSame");
        }
        else
        {
            Debug.Log("RealmController DateTimeError FromDateIsEarlierThanToDate");
        }
        int fhr = GetHr(fromAm.value, fromHr.value, fromMin.value, false);
        int fmin = fromMin.value == 0 ? 0 : 30;
        int thr = GetHr(toAm.value, toHr.value, toMin.value, true);
        int tmin = toMin.value == 0 ? 0 : 30;
        if (fhr > thr)
        {
            Debug.Log("RealmController DateTimeError FromHrIsLater" + fhr + " " + thr);
            return true;
        }
        else if (fhr == thr)
        {
            if (fmin == tmin)
            {
                Debug.Log("RealmController DateTimeError FromTimeAndToTimeIsSame" + fhr + " " + fmin + " " + thr + " " + tmin);
                return true;
            }
            else if (fmin < tmin)
            {
                Debug.Log("RealmController DateTimeError FromHrIsEarlierButFromMinIsLater" + fhr + " " + fmin + " " + thr + " " + tmin);
                return false;
            }
        }
        else if (fhr < thr)
        {
            Debug.Log("RealmController DateTimeError FromHrIsEarlier" + fhr + " " + thr);
            return false;
        }
        return true;
    }

    /*
     * Purpose: Get the list of locations according to the date 
     * Input: Called by ResourceReservationUI using openDetails() when date button is clicked
     *        Called by TimeDetailsUI using closeTimeDetails() when "closeTimeDetails" button is clicked
     * Output: Call PerformRealmWriteRetrieveAvailable and returns ordered string list of locations
     */
    public List<string> GetLocations()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetLocations RealmNotInitialized");
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
     * Purpose: Get the list of timings according to the date and location string
     * Input: Called by RealmController when buttons in dateDetails UI are clicked
     * Output: Call PerformRealmWriteRetrieveAvailable and returns ordered string list of timings
     */
    public List<string> GetTimings(string location)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetReservations RealmNotInitialized");
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
     * Purpose: Get the reservations according to the location string passed in
     * Input: Click on the "removeSlotsButton" button in "manageSlots" UI
     * Output: return null if there is an error
     *         else return a list of strings that contains the reservations
     */
    public List<string> GetReservations(string location)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetReservations RealmNotInitialized");
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
     * Purpose: Create the available slots to add from user input and write them to the database
     * Input: Called by AddAvailable()
     * Output: Add the available slots to the database
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
                        }
                        
                    });
                }
                catch (Exception ex)
                {
                    Debug.LogError("RealmController PerformRealmWriteAddAvailable ErrorWritingToRealm: " + ex.Message);
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

        Debug.Log("RealmController PerformRealmWriteAddAvailable Completed.");
    }

    private IEnumerator PerformRealmWriteAddReservation(List<Reserved> addReservationList)
    {


        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("Realm write operation addResrvation completed.");
    }
    
    /*
     * Purpose: Create the available slots to remove from user input and write them to the database
     * Input: Called by RemoveAvailable()
     * Output: Remove the available slots to the database
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
                    Debug.LogError("RealmController PerformRealmWriteRemoveAvailable ErrorWritingToRealm: " + ex.Message);
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

        Debug.Log("RealmController PerformRealmWriteRemoveAvailable Completed.");
    }

    /*
     * Purpose: Get the available list according to the date
     * Input: Called by GetLocations() and GetTimings()
     * Output: return the list of Available according the the dateText or timeText
     */
    private List<Available> PerformRealmWriteRetrieveAvailable()
    {
        string dateText;
        if (dateTextProf.IsActive())
        {
            dateText = dateTextProf.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveAvailable ProfText" + dateText);
        }
        else if (dateTextStaff.IsActive())
        {
            dateText = dateTextStaff.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveAvailable StaffText" + dateText);
        }
        else if (timeTextProf.IsActive())
        {
            dateText = timeTextProf.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveAvailable ProfText" + dateText);
        }
        else if (timeTextStaff.IsActive())
        {
            dateText = timeTextStaff.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveAvailable StaffText" + dateText);
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
            Debug.LogError("RealmController PerformRealmWriteRetrieveAvailable ErrorQueryingRealm: " + ex.Message);
            return null;
        }

        
    }

    /*
     * Purpose: Get the reserved list according to the date
     * Input: 
     * Output: return the list of reserved according the the dateText or timeText
     */
    private List<Reserved> PerformRealmWriteRetrieveReservation()
    {
        string dateText;
        if (timeTextProf.IsActive())
        {
            dateText = timeTextProf.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveReservation ProfText" + dateText);
        }
        else if (timeTextStaff.IsActive())
        {
            dateText = timeTextStaff.text;
            Debug.Log("RealmController PerformRealmWriteRetrieveReservation StaffText" + dateText);
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
            Debug.LogError("RealmController PerformRealmWriteRetrieveReservation ErrorQueryingRealm: " + ex.Message);
            return null;
        }
    }

    /*
     * Purpose: Used to calculate the time from 12hr format to 24hr format
     * Input: Called by PerfromRealmWriteAdd/Remove(), passing in am, hr, min, and to
     * Output: return int hr in 24hr format
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

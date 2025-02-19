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

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage send and retrieve data from MongoDB
 * Tutorial used: https://www.youtube.com/watch?v=f-IQwVReQ-c, https://www.youtube.com/watch?v=q4_997QEQww
 */
public class RealmController : MonoBehaviour
{
    private static RealmController instance;
    private Realm realm;
    private readonly string myRealmAppId = "application-0-nlkew";
    private bool isRealmInitialized = false;
    public DynamicButtonCreator buttonCreator;

    // ErrorText from "manageSlots" UI
    public Text manageSlotsErrorText;

    // ErrorText from "newMeeting" UI
    public Text newMeetingErrorText;

    /*
     * Purpose: Initialise an instance of RealmController and make sure it persists across scene changes
     * Input: NA
     * Output: Call InitializeRealm()
     */
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeRealm();
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }

    /*
     * Purpose: To initialise the realm
     * Input: Called by Start()
     * Output: Initialise realm using anonymous login
     */
    private async void InitializeRealm()
    {
        Debug.Log("RealmController InitializeRealm");
        var app = App.Create(myRealmAppId);
        var credential = Credentials.Anonymous();
        try
        {
            await app.LogInAsync(credential);
            var config = new PartitionSyncConfiguration("FYP", app.CurrentUser);
            realm = await Realm.GetInstanceAsync(config);
            isRealmInitialized = true;
            Debug.Log("RealmController InitializeRealm Login success");
        }
        catch (Exception ex)
        {
            Debug.Log("RealmController InitializeRealm Login failed: " + ex.Message);
            Debug.Log("RealmController InitializeRealm Login failed: " + ex.InnerException);
        }
    }

    /*
     * Purpose: Getter function of realm
     * Input: Called by RealmControllerClassRoom by Start()
     * Output: return realm
     */
    public Realm RealmInstance
    {
        get
        {
            return realm;
        }
    }

    /*
     * Purpose: Check whether we are able to add available slots, then attempt to write to database
     * Input: Called by ManageSlotsUI by AddAvailableSlots()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to add Available
     */
    public void AddAvailable(string location, string fromDate, string fromMonth, string fromYear, string toDate, string toMonth, string toYear, int fromHr, int fromMin, int fromAm, int toHr, int toMin, int toAm)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController AddAvailable RealmNotInitialised");
            return;
        }

        if (location == "" || fromDate == "" || fromMonth == "" || fromYear == "" || toDate == "" || toMonth == "" || toYear == "")
        {
            manageSlotsErrorText.text = "Unable to add slots. Missing input(s).";
            return;
        }

        if (DatetimeError(fromDate, fromMonth, fromYear, toDate, toMonth, toYear, fromHr, fromMin, fromAm, toHr, toMin, toAm))
        {
            manageSlotsErrorText.text = "Unable to add slots. Please make sure fromdate/time is earlier than todate/time and that they are a valid date.";
            Debug.Log("RealmController AddAvailable DateTimeError");
            return;
        }

        Debug.Log("RealmController AddAvailable AddingAvailableToRealm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAddAvailable(location, fromDate,fromMonth, fromYear, toDate, toMonth, toYear, fromHr, fromMin, fromAm, toHr, toMin, toAm));

        manageSlotsErrorText.text = "Adding slots complete";
    }

    /*
     * Purpose: Attempt to add reservation dates, no need to check as list is created by clicking available buttons
     * Input: Called by TimeDetailsUI by AddReservationSlots()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to add Available
     */
    public void AddReservation()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController AddReservation RealmNotInitialised");
            return;
        }

        Debug.Log("RealmController AddReservation AddingReservationsToRealm");

        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        List<Reserved> addReservationList = new List<Reserved>();
        addReservationList = buttonCreator.GetAddReservationList;

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAddReservation(addReservationList));
    }

    /*
     * Purpose: Attempt to add meeting schedule, called by NewMeetingUI
     * Input: Called by NewMeetingUI by CreateMeeting()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to add Meetings
     */
    public void AddMeeting(List<string> emailList, string meetingDate, string meetingMonth, string meetingYear, string meetingDuration, string meetingDescription, string meetingHr, string meetingMin, int meetingAm)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController AddMeeting RealmNotInitialised");
            return;
        }
        if (meetingDate== "" || meetingMonth == "" || meetingYear == "" || meetingDuration == "" || meetingDescription == "" || meetingHr == "" || meetingMin == "")
        {
            newMeetingErrorText.text = "Unable to create meeting. Missing input(s).";
            return;
        }
        if (MeetingError(meetingDate, meetingMonth, meetingYear, meetingHr, meetingMin))
        {
            newMeetingErrorText.text = "Unable to create meeting. Please make sure date and start time is valid.";
            Debug.Log("RealmController AddMeeting MeetingError");
            return;
        }

        Debug.Log("RealmController AddMeeting AddingAvailableToRealm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteAddMeeting(emailList, meetingDate, meetingMonth, meetingYear, meetingDuration, meetingDescription, meetingHr, meetingMin, meetingAm));

        newMeetingErrorText.text = "Meeting created";
    }

    /*
     * Purpose: Check whether we are able to remove available slots, then attempt to write to database
     * Input: Called by ManageSlotsUI by RemoveAvailableSlots()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to remove Available
     */
    public void RemoveAvailable(string location, string fromDate, string fromMonth, string fromYear, string toDate, string toMonth, string toYear, int fromHr, int fromMin, int fromAm, int toHr, int toMin, int toAm)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController RemoveAvailable RealmNotInitialised");
            return;
        }

        if (location == "" || fromDate == "" || fromMonth == "" || fromYear == "" || toDate == "" || toMonth == "" || toYear == "")
        {
            manageSlotsErrorText.text = "Unable to remove slots. Missing inputs.";
            return;
        }

        if (DatetimeError(fromDate, fromMonth, fromYear, toDate, toMonth, toYear, fromHr, fromMin, fromAm, toHr, toMin, toAm))
        {
            manageSlotsErrorText.text = "Unable to remove slots. Please make sure fromdate/time is earlier than todate/time";
            Debug.Log("RealmController RemoveAvailable DateTimeError");
            return;
        }

        Debug.Log("RealmController RemoveAvailable RemovingAvailableFromRealm");

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteRemoveAvailable(location, fromDate, fromMonth, fromYear, toDate, toMonth, toYear, fromHr, fromMin, fromAm, toHr, toMin, toAm));

        manageSlotsErrorText.text = "Removing slots complete";
    }

    /*
     * Purpose: Attempt to remove reservation dates, no need to check as list is created by clicking available buttons
     * Input: Called by TimeDetailsUI by RemoveReservationSlots()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to remove Reservation
     */
    public void RemoveReservation()
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController RemoveReservation RealmNotInitialised");
            return;
        }

        Debug.Log("RealmController RemoveReservation RemovingReservationsFromRealm");

        buttonCreator = FindObjectOfType<DynamicButtonCreator>();
        List<Reserved> removeReservationList = new List<Reserved>();
        removeReservationList = buttonCreator.GetRemoveReservationList;

        // Schedule a coroutine to execute Realm write operation on the main thread
        StartCoroutine(PerformRealmWriteRemoveReservation(removeReservationList));
    }

    /*
     * Purpose: Attempt to remove meeting schedule, called by MeetingDetailsUI
     * Input: Called by MeetingDetailsUI by DeleteMeeting()
     * Output: return if there is an error
     *         else schedule a coroutine to execute Realm write operation on the main thread to remove Meetings
     */
    public void RemoveMeeting(string meetingId)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController RemoveMeeting RealmNotInitialised");
            return;
        }

        Debug.Log("RealmController RemoveMeeting RemovingMeetingFromRealm");

        StartCoroutine(PerformRealmWriteRemoveMeeting(meetingId));
    }

    /*
     * Purpose: Get the list of locations according to the date 
     * Input: Called by ResourceReservationUI using openDetails() when date button is clicked
     *        Called by TimeDetailsUI using closeTimeDetails() when "closeTimeDetails" button is clicked
     * Output: Call PerformRealmWriteRetrieveAvailable and returns ordered string list of locations
     */
    public List<string> GetLocations(string date)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetLocations RealmNotInitialized");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveAvailable(date);
        var locationList = queryResults
            .OrderBy(item => item.Location)
            .Select(item => item.Location)
            .Distinct()
            .ToList();

        return locationList;
    }

    /*
     * Purpose: Get the list of timings according to the date and location string passed in
     * Input: Called by DynamicButtonCreator when buttons in dateDetails UI are clicked, location string passed in
     *        Called by TimeDetailsUI using RefreshTimeDetails()
     * Output: Call PerformRealmWriteRetrieveAvailable and returns ordered string list of timings
     */
    public List<string> GetTimings(string location, string date)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetReservations RealmNotInitialized");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveAvailable(date);
        var timingList = queryResults
            .Where(item => item.Location == location)
            .OrderBy(item => item.Hour)
            .ThenBy(item => item.Min)
            .Select(item => item.Hour.ToString() + ":" + item.Min.ToString())
            .ToList();
        Debug.Log(timingList);
        return timingList;
    }

    /*
     * Purpose: Get the reservations according to the reservation passed in
     * Input: Called by DynamicButtonCreator when buttons in dateDetails UI are clicked
     * Output: Call PerformRealmWriteRetrieveReservation
     *         return null if there is an error
     *         else return Reserved
     */
    public Reserved GetReservations(Reserved reservation)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetReservations RealmNotInitialized");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveReservation(reservation);
        return queryResults;
    }

    /*
     * Purpose: Get a list of meetings according to the date passed in
     * Input: Called when buttons in MeetingSchedule UI are clicked
     * Output: Call PerformRealmWriteRetrieveMeetings
     *         return null if there is an error
     *         else return an ordered list of Meetings
     */
    public List<Meetings> GetMeetings(string date)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetMeetings RealmNotInitialized");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveMeetings(date);
        var meetingList = queryResults
            .OrderBy(item => item.StartTimeHr)
            .ThenBy(item => item.StartTimeMin)
            .ToList();
        Debug.Log(meetingList);
        return meetingList;
    }

    /*
     * Purpose: Get meeting details according to objectId passed in
     * Input: Called by DynamicButtonCreator when buttons in MeetingSchedule UI are clicked
     * Output: Call PerformRealmWriteRetrieveMeetingDetails
     *         return null if there is an error
     *         else return Meetings
     */
    public Meetings GetMeetingDetails(string objectID)
    {
        if (!isRealmInitialized)
        {
            Debug.Log("RealmController GetMeetingDetails RealmNotInitialized");
            return null;
        }
        var queryResults = PerformRealmWriteRetrieveMeetingDetails(objectID);
        return queryResults;
    }

    /*
     * Purpose: Create the available slots to add from user input and write them to the database
     * Input: Called by AddAvailable()
     * Output: Add the available slots to the database
     */
    private IEnumerator PerformRealmWriteAddAvailable(string location, string fromDate, string fromMonth, string fromYear, string toDate, string toMonth, string toYear, int fromHr, int fromMin, int fromAm, int toHr, int toMin, int toAm)
    {
        int date = int.Parse(fromDate); 
        int month = int.Parse(fromMonth);
        int year = int.Parse(fromYear);
        int hr = GetHr(fromAm, fromHr, fromMin, false);
        int min = fromMin == 0 ? 0 : 30;
        int noOfDays = DateTime.DaysInMonth(2000+year, month);
        
        while (date != int.Parse(toDate) || month != int.Parse(toMonth) || year != int.Parse(toYear) || hr != GetHr(toAm, toHr, toMin, true) || min != (toMin == 0 ? 0 : 30))
        {
            hr = GetHr(fromAm, fromHr, fromMin, false);
            min = fromMin == 0 ? 0 : 30;

            while (hr != GetHr(toAm, toHr, toMin, true) || min != (toMin == 0 ? 0 : 30) )
            {
                // This code block will run on the main/UI thread
                try
                {
                    realm.Write(() =>
                    {
                        var results = realm.All<Available>().FirstOrDefault(item => item.Location == location && item.Date == date && item.Month == month && item.Year == year && item.Hour == hr && item.Min == min);
                        if (results == null) 
                        {
                            realm.Add(new Available(location, date, month, year, hr, min));
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
            
            if (date != int.Parse(toDate) || month != int.Parse(toMonth) || year != int.Parse(toYear))
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
                hr = GetHr(fromAm, fromHr, fromMin, false);
                min = fromMin == 0 ? 0 : 30;
            }

        }
        
        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteAddAvailable Completed.");
    }

    /*
     * Purpose: Add the reservation slots into the database
     * Input: Called by AddReservation()
     * Output: Add the reservation slots to the database
     */
    private IEnumerator PerformRealmWriteAddReservation(List<Reserved> addReservationList)
    {
        try
        {
            realm.Write(() =>
            {
                foreach (Reserved reservation in addReservationList)
                {
                    var results = realm.All<Reserved>().FirstOrDefault(item => item.Location == reservation.Location && item.Date == reservation.Date && item.Month == reservation.Month && item.Year == reservation.Year && item.Hour == reservation.Hour && item.Min == reservation.Min);
                    if (results == null)
                    {
                        realm.Add(reservation);
                    }
                }

            });
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteAddReservation ErrorWritingToRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteAddReservation Completed.");
    }

    /*
     * Purpose: Add the meeting scheduled into the database
     * Input: Called by AddMeeting()
     * Output: Add the meeting scheduled to the database
     */
    private IEnumerator PerformRealmWriteAddMeeting(List<string> emailList, string meetingDate, string meetingMonth, string meetingYear, string meetingDuration, string meetingDescription, string meetingHr, string meetingMin, int meetingAm)
    {
        string date = meetingDate + "/" + meetingMonth + "/20" + meetingYear;
        string joinCode = "";
        int timeHr = meetingAm == 1 ? (int.Parse(meetingHr) == 12 ? int.Parse(meetingHr) : int.Parse(meetingHr) + 12) : (int.Parse(meetingHr) == 12 ? int.Parse(meetingHr) - 12 : int.Parse(meetingHr));
        int timeMin = int.Parse(meetingMin);
        MeetingScheduleUI MeetingScheduleUI = FindObjectOfType<MeetingScheduleUI>();
        string hostEmail = MeetingScheduleUI.GetUserEmail;

        try
        {
            realm.Write(() =>
            {
                realm.Add(new Meetings(date, timeHr, timeMin, meetingDuration, meetingDescription, hostEmail, emailList, joinCode));
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteAddMeeting ErrorWritingToRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteAddMeeting Completed.");
    }

    /*
     * Purpose: Create the available slots to remove from user input and write them to the database
     * Input: Called by RemoveAvailable()
     * Output: Remove the available slots to the database
     */
    private IEnumerator PerformRealmWriteRemoveAvailable(string location, string fromDate, string fromMonth, string fromYear, string toDate, string toMonth, string toYear, int fromHr, int fromMin, int fromAm, int toHr, int toMin, int toAm)
    {
        int date = int.Parse(fromDate);
        int month = int.Parse(fromMonth);
        int year = int.Parse(fromYear);
        int hr = GetHr(fromAm, fromHr, fromMin, false);
        int min = fromMin == 0 ? 0 : 30;
        int noOfDays = DateTime.DaysInMonth(2000 + year, month);

        while (date != int.Parse(toDate) || month != int.Parse(toMonth) || year != int.Parse(toYear) || hr != GetHr(toAm, toHr, toMin, true) || min != (toMin == 0 ? 0 : 30))
        {
            hr = GetHr(fromAm, fromHr, fromMin, false);
            min = fromMin == 0 ? 0 : 30;

            while (hr != GetHr(toAm, toHr, toMin, true) || min != (toMin == 0 ? 0 : 30))
            {
                try
                {

                    var reservation = realm.All<Reserved>().FirstOrDefault(item => item.Location == location && item.Date == date && item.Month == month && item.Year == year && item.Hour == hr && item.Min == min);
                    
                    if (reservation == null)
                    {
                        realm.Write(() =>
                        {
                            var results = realm.All<Available>().Where(item => item.Location == location && item.Date == date && item.Month == month && item.Year == year && item.Hour == hr && item.Min == min).ToList();
                            foreach (var item in results)
                            {
                                Debug.Log("Remove Available: " + item.Hour + ":" + item.Min);
                                realm.Remove(item);
                            }
                        });
                    }
                    else
                    {
                        Debug.Log("ReservationNotNull: " + reservation.Hour + ":" + reservation.Min);
                    }
                    
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

            if (date != int.Parse(toDate) || month != int.Parse(toMonth) || year != int.Parse(toYear))
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
                hr = GetHr(fromAm, fromHr, fromMin, false);
                min = fromMin == 0 ? 0 : 30;
            }

        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteRemoveAvailable Completed.");
    }

    /*private IEnumerator PerformRealmWriteRemoveAll()
{
    realm.Write(() =>
    {
        realm.RemoveAll<Available>(); // Remove all objects of the specified type
    });

    yield return null; // Yielding once to ensure the write operation is executed

    Debug.Log("Realm write operation remove completed.");
}*/

    /*
     * Purpose: Remove the reservation slots from the database
     * Input: Called by RemoveReservation()
     * Output: Remove the reservation slots from the database
     */
    private IEnumerator PerformRealmWriteRemoveReservation(List<Reserved> removeReservationList)
    {
        try
        {
            foreach (Reserved reservation in removeReservationList)
            {
                realm.Write(() =>
                {
                    var results = realm.All<Reserved>().Where(item => item.Location == reservation.Location && item.Date == reservation.Date && item.Month == reservation.Month && item.Year == reservation.Year && item.Hour == reservation.Hour && item.Min == reservation.Min).ToList();
                    foreach (var item in results)
                    {
                        realm.Remove(item);
                    }

                });
            }
            
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRemoveReservation ErrorWritingToRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteRemoveReservation Completed.");
    }

    /*
     * Purpose: Remove the meeting scheduled from the database
     * Input: Called by RemoveMeeting()
     * Output: Remove the meeting slots from the database
     */
    private IEnumerator PerformRealmWriteRemoveMeeting(string meetingId)
    {
        try
        {
            realm.Write(() =>
            {
                var results = realm.All<Meetings>().FirstOrDefault(item => item.Id == ObjectId.Parse(meetingId));
                realm.Remove(results);
            });
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRemoveMeeting ErrorQueryingRealm: " + ex.Message);
        }

        yield return null; // Yielding once to ensure the write operation is executed

        Debug.Log("RealmController PerformRealmWriteRemoveMeeting Completed.");
    }

    /*
     * Purpose: Get the available list according to the date
     * Input: Called by GetLocations() and GetTimings()
     * Output: return the list of Available according the the dateText or timeText
     */
    private List<Available> PerformRealmWriteRetrieveAvailable(string dateText)
    {
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
     * Purpose: Get the reserved slot according to Reserved passed
     * Input: Called by GetReservation()
     * Output: return the reserved slot according
     */
    private Reserved PerformRealmWriteRetrieveReservation(Reserved reservation)
    {
        try
        {
            Reserved results = realm.All<Reserved>()
                .FirstOrDefault(item => item.Location == reservation.Location && item.Date == reservation.Date && item.Month == reservation.Month && item.Year == reservation.Year && item.Hour == reservation.Hour && item.Min == reservation.Min);
            return results;
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRetrieveReservation ErrorQueryingRealm: " + ex.Message);
            return null;
        }
    }

    /*
     * Purpose: Get the Meetings slot according to the date
     * Input: Called by GetMeetings()
     * Output: return the list of Meetings according to the dateText
     */
    private List<Meetings> PerformRealmWriteRetrieveMeetings(string dateText)
    {
        string[] parts = dateText.Split('/');
        MeetingScheduleUI MeetingScheduleUI = FindObjectOfType<MeetingScheduleUI>();
        string userEmail = MeetingScheduleUI.GetUserEmail;
        Meetings_participant_emails userMeetingEmail = new Meetings_participant_emails(userEmail);

        try
        {
            /*List<Meetings> results = realm.All<Meetings>()
                .Where(item => item.Date == dateText)
                .ToList()
                .Where(item => item.ParticipantEmails.Any(participant => participant.ParticipantEmail == userEmail) || item.HostEmail == userEmail)
                .ToList();*/

            List<Meetings> hostResult = realm.All<Meetings>()
                .Filter($"Date == '{dateText}'")
                .Filter($"ANY ParticipantEmails.ParticipantEmail == '{userEmail}'")
                .ToList();
            List<Meetings> participantResult = realm.All<Meetings>()
                .Filter($"Date == '{dateText}'")
                .Filter($"HostEmail == '{userEmail}'")
                .ToList();
            List<Meetings> results = hostResult.Union(participantResult).ToList();

            foreach (Meetings result in results)
            {
                Debug.Log(result);
            }
            return results;
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRetrieveMeetings ErrorQueryingRealm: " + ex.Message);
            return null;
        }

    }

    /*
     * Purpose: Get the Meeting details according to the objectId
     * Input: Called by GetMeetingDetails()
     * Output: return the Meeting details according to the objectId
     */
    private Meetings PerformRealmWriteRetrieveMeetingDetails(string objectID)
    {
        try
        {
            Meetings results = realm.All<Meetings>()
                .FirstOrDefault(item => item.Id == ObjectId.Parse(objectID));
            return results;
        }
        catch (Exception ex)
        {
            Debug.LogError("RealmController PerformRealmWriteRetrieveMeetingDetails ErrorQueryingRealm: " + ex.Message);
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

    /*
     * Purpose: Check whether the date time input in "manageSlots" UI is correct
     * Input: Called by AddAvailable() and RemoveAvailable()
     * Output: return true if there is an error
     *         else return false
     */
    private bool DatetimeError(string fromDate, string fromMonth, string fromYear, string toDate, string toMonth, string toYear, int fromHr, int fromMin, int fromAm, int toHr, int toMin, int toAm)
    {
        try
        {
            DateTime from = new DateTime(2000 + int.Parse(fromYear), int.Parse(fromMonth), int.Parse(fromDate));
            DateTime to = new DateTime(2000 + int.Parse(toYear), int.Parse(toMonth), int.Parse(toDate));
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
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return true;
        }
        
        int fhr = GetHr(fromAm, fromHr, fromMin, false);
        int fmin = fromMin == 0 ? 0 : 30;
        int thr = GetHr(toAm, toHr, toMin, true);
        int tmin = toMin == 0 ? 0 : 30;
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
     * Purpose: Check whether the date time input in "newMeeting" UI is correct
     * Input: Called by AddMeeting()
     * Output: return true if there is an error
     *         else return false
     */
    private bool MeetingError(string meetingDate, string meetingMonth, string meetingYear, string meetingHr, string meetingMin)
    {
        int hr = int.Parse(meetingHr);
        int min = int.Parse(meetingMin);
        try
        {
            DateTime date = new DateTime(2000 + int.Parse(meetingYear), int.Parse(meetingMonth), int.Parse(meetingDate));
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return true;
        }
        if (hr <= 0 || hr >= 13)
        {
            return true;
        } 
        if (min < 0 || min >= 60)
        {
            return true;
        }
        return false;
    }

    
}

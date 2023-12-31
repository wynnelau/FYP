using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PlayFab.ClientModels;
using PlayFab;
using Realms.Sync;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the MeetingSchedule calendar, which contains the dates
 * Tutorial used: https://www.youtube.com/watch?v=cMwCZhZnE4k
 */
public class MeetingScheduleUI : MonoBehaviour
{
    public class Day
    {
        public int dayInt;
        public Color availability;
        public GameObject dayButton;

        public Day (int dayInt, Color availability, GameObject dayButton)
        {
            this.dayInt = dayInt;
            this.dayButton = dayButton;
            UpdateColor(availability);
            UpdateDay(dayInt);
        }

        public void GetDayInt()
        {
            Debug.Log(dayInt);
        }

        public void UpdateColor(Color newColor)
        {
            dayButton.GetComponent<UnityEngine.UI.Image>().color = newColor;
            availability = newColor;
        }

        public void UpdateDay(int newDayInt)
        {
            this.dayInt = newDayInt;
            if (availability == Color.white || availability == Color.green || availability == Color.gray)
            {
                dayButton.GetComponentInChildren<Text>().text = (dayInt+1).ToString();
            }
            else
            {
                dayButton.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public GameObject mainMenuStaff, mainMenuProf, mainMenuStudent, MeetingSchedule, MeetingDetails, NewMeeting;
    public Text MeetingDetailsDate;
    public PlayerControls player;
    public RealmController RealmController;
    public DynamicButtonCreator buttonCreator;

    private List<Day> days = new List<Day>();
    public Transform[] weeks;
    public Text MonthAndYear;
    public DateTime currMonthYear = DateTime.Now;

    private string userEmail;
    private string userIdentity;

    /*
     * Purpose: Instantiate all Day gameObjects and addListener to all buttons in the calendar
     *          Retrieve user's email for reservation of slots
     *          Start is called before the first frame update
     * Input: NA
     * Output: addListener will be used when user clicks on the buttons in the calendar, calling openDetails
     */
    void Start()
    {
        RetrieveUserEmail();
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
        for (int i = 0; i < 42; i++)
        {
            var dateSelected = days[i];
            dateSelected.dayButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { OpenDateDetails(dateSelected.dayButton); });
        }
    }

    /*
     * Purpose: To close the MeetingScheduleUI
     * Input: Click on the "closeMeetingScheduleButton" button in the MeetingScheduleUI
     * Output: Close MeetingScheduleUI and open the respective mainMenuUI
     */
    public void CloseMeetingSchedule()
    {
        Debug.Log("MeetingScheduleUI closeMeetingSchedule");
        MainMenuTable MainMenuTable = FindObjectOfType<MainMenuTable>();
        userIdentity = MainMenuTable.GetUserIdentity;
        if (userIdentity == "Student")
        {
            Debug.Log("MeetingScheduleUI closeMeetingSchedule Student");
            mainMenuStudent.SetActive(true);
        }
        else if (userIdentity == "Staff")
        {
            Debug.Log("MeetingScheduleUI closeMeetingSchedule Staff");
            mainMenuStaff.SetActive(true);
        }
        else if (userIdentity == "Professor/TA")
        {
            Debug.Log("MeetingScheduleUI closeMeetingSchedule Prof");
            mainMenuProf.SetActive(true);
        }
        MeetingSchedule.SetActive(false);
    }

    /*
     * Purpose: To open the dateDetails UI with buttons
     * Input: Click on the date buttons in the resourceReservationUI, used by the listeners 
     * Output: Open the respective dateDetails UI
     */
    void OpenDateDetails(GameObject dateSelected)
    {
        Debug.Log("MeetingScheduleUI openDateDetails");
        if (dateSelected.GetComponentInChildren<Text>().text != "")
        {
            Debug.Log("MeetingScheduleUI openDateDetails " + dateSelected.GetComponentInChildren<Text>().text + currMonthYear.Month + currMonthYear.Year);

            RealmController = FindObjectOfType<RealmController>();
            buttonCreator = FindObjectOfType<DynamicButtonCreator>();

            if (MeetingSchedule.activeSelf == true)
            {
                Debug.Log("MeetingScheduleUI openDateDetails MeetingScheduleActive");
                MeetingSchedule.SetActive(false);
                MeetingDetails.SetActive(true);
                MeetingDetailsDate.text = dateSelected.GetComponentInChildren<Text>().text + "/" + currMonthYear.Month + "/" + currMonthYear.Year;
            }

            if (RealmController != null)
            {
                var meetingList = RealmController.GetMeetings();
                if (meetingList != null && meetingList.Count > 0)
                {
                    foreach (var meeting in meetingList)
                    {
                        buttonCreator.CreateButton(meeting.Id.ToString() + "\nStart time:     " + TimeConvert(meeting.StartTimeHr) + ":" + TimeConvert(meeting.StartTimeMin));
                    }

                }
            }

        }

    }

    /*
     * Purpose: To open the "NewMeeting" UI
     * Input: Click on the "createMeetingButton" button in the "MeetingSchedule" UI 
     * Output: Open the "NewMeeting" UI
     */
    public void OpenCreateMeeting()
    {
        MeetingSchedule.SetActive(false);
        NewMeeting.SetActive(true);
    }

    /*
     * Purpose: To update the calendar of MeetingScheduleUI
     * Input: Called by Start(), and when we toggle between the months using PrevMonth() and NextMonth() 
     * Output: Updates the calendar dates in the calendar
     */
    void UpdateCalendar(int year, int month)
    {
        DateTime date = new DateTime(year, month, 1);
        currMonthYear = date;
        MonthAndYear.text = date.ToString("MMMM") + " " + date.Year.ToString();
        int startDay = GetMonthStartDay(year, month);
        int endDay = GetTotalNumberOfDays(year, month);

        /* Used to create Day objects when there are no Day objects */
        if (days.Count == 0)
        {
            for (int week = 0; week < 6; week++)
            {
                for (int i = 0; i < 7; i++)
                {
                    Day newDay;
                    int currDay = (week * 7) + i;
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new Day(currDay - startDay, Color.black, weeks[week].GetChild(i).gameObject);
                    }
                    else if (DateTime.Now.Day > currDay - startDay)
                    {
                        newDay = new Day(currDay - startDay, Color.gray, weeks[week].GetChild(i).gameObject);
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, Color.white, weeks[week].GetChild(i).gameObject);
                    }
                    days.Add(newDay);
                }
            }
        }
        /* Used to update the Day objects when they are already present */
        else
        {
            for (int i = 0; i < 42; i++)
            {
                if (i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateColor(Color.black);
                }
                else if (DateTime.Now.Year >= year)
                {
                    if (DateTime.Now.Year > year)
                    {
                        days[i].UpdateColor(Color.gray);
                    }
                    else if (DateTime.Now.Year == year)
                    {
                        if (DateTime.Now.Month > month)
                        {
                            days[i].UpdateColor(Color.gray);
                        }
                        else if (DateTime.Now.Month == month)
                        {
                            if (DateTime.Now.Day > i - startDay)
                            {
                                days[i].UpdateColor(Color.gray);
                            } 
                            else
                            {
                                days[i].UpdateColor(Color.white);
                            }
                        } 
                        else
                        {
                            days[i].UpdateColor(Color.white);
                        }
                    }
                }
                else
                {
                    days[i].UpdateColor(Color.white);
                }

                days[i].UpdateDay(i - startDay);
            }
        }

        /* Highlight the current day */
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateColor(Color.green);
        }

    }

    /*
     * Purpose: To get the first day of the month. DayOfWeek: Sun == 0, Saturday == 6
     * Input: Called by UpdateCalendar() 
     * Output: returns the first day of the month
     */
    int GetMonthStartDay(int dateYear, int dateMonth)
    {
        DateTime day = new DateTime(dateYear, dateMonth, 1);
        return (int)day.DayOfWeek;
    }

    /*
     * Purpose: To get the total number of days of the month
     * Input: Called by UpdateCalendar() 
     * Output: returns the number of days of the month
     */
    int GetTotalNumberOfDays(int dateYear, int dateMonth)
    {
        return DateTime.DaysInMonth(dateYear, dateMonth);
    }

    /*
     * Purpose: To navigate between the different months when the "left" button is clicked in the MeetingScheduleUI
     * Input: Click on the "left" button
     * Output: Calls UpdateCalendar() with updated dates
     */
    public void PrevMonth()
    {
        currMonthYear = currMonthYear.AddMonths(-1);
        UpdateCalendar(currMonthYear.Year, currMonthYear.Month);
    }

    /*
     * Purpose: To navigate between the different months when the "right" button is clicked in the MeetingScheduleUI
     * Input: Click on the "right" button
     * Output: Calls UpdateCalendar() with updated dates
     */
    public void NextMonth()
    {
        currMonthYear = currMonthYear.AddMonths(1);
        UpdateCalendar(currMonthYear.Year, currMonthYear.Month);
    }

    /*
     * Purpose: Getter function of the private userEmail
     * Input: Called by RealmController to get Host Email
     * Output: Returns the user's email
     */
    public string GetUserEmail
    {
        get
        {
            return userEmail;
        }
    }

    /*
     * Purpose: Attempt to get the user's email for reservation purposes
     * Input: Called by Start() in MeetingScheduleUI
     * Output: Call RetrieveUserEmailSuccess() if successful, RetrieveUserEmailFail() if unsuccessful
     */
    void RetrieveUserEmail()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null
        }, RetrieveUserEmailSuccess, RetrieveUserEmailFail);
    }

    /*
     * Purpose: Successful attempt to get user's email and assign userEmail with the user's email
     * Input: Called by the RetrieveUserEmail() when attempt to get user's email is successful
     * Output: Assign userEmail with the retrieved data
     */
    void RetrieveUserEmailSuccess(GetUserDataResult result)
    {
        userEmail = result.Data["Email"].Value;
    }

    /*
     * Purpose: Failed attempt to get user's email
     * Input: Called by the RetrieveUserEmail() when attempt to get user's email failed
     * Output: Debug.Log("MeetingScheduleUI RetrieveUserEmailFail " + error);
     */
    void RetrieveUserEmailFail(PlayFabError error)
    {
        Debug.Log("MeetingScheduleUI RetrieveUserEmailFail " + error);
    }

    /*
     * Purpose: To convert int to string for time display
     * Input: Called when trying to create dynamic button for MeetingDetailsUI and int time is passed in
     * Output: Returns a string for the time display
     */
    string TimeConvert(int? time)
    {
        string timeString;
        if (time < 10)
        {
            timeString = "0" + time.ToString();
        }
        else
        {
            timeString = time.ToString();
        }

        return timeString;
    }

}

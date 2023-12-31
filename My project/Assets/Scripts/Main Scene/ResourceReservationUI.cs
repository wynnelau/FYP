using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using PlayFab.ClientModels;
using PlayFab;

/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Manage the ResourceReservation calendar, which contains the dates
 * Tutorial used: https://www.youtube.com/watch?v=cMwCZhZnE4k
 */
public class ResourceReservationUI : MonoBehaviour
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

    public GameObject resourceReservationStaff, mainMenuStaff, dateDetailsStaff, resourceReservationProf, mainMenuProf, dateDetailsProf, reservationSlots;
    public Text dateDetailsDateStaff, dateDetailsDateProf;
    public PlayerControls player;
    public RealmController RealmController;
    public DynamicButtonCreator buttonCreator;

    private List<Day> days = new List<Day>();
    public Transform[] weeks;
    public Text MonthAndYear;
    public DateTime currMonthYear = DateTime.Now;

    private string userEmail;

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
     * Purpose: To close the ResourceReservationUI
     * Input: Click on the "closeResourceReservationButton" button in the respective ResourceReservation UIs
     * Output: Set the resourceReservationUI as inactive and set the respective mainMenuUI as active
     */
    public void CloseResourceReservation()
    {
        Debug.Log("ResourceReservationUI closeResourceReservation");
        if (resourceReservationStaff.activeSelf == true)
        {
            Debug.Log("ResourceReservationUI closeResourceReservation Staff");
            resourceReservationStaff.SetActive(false);
            mainMenuStaff.SetActive(true);
        }
        else if (resourceReservationProf.activeSelf == true)
        {
            Debug.Log("ResourceReservationUI closeResourceReservation Prof");
            resourceReservationProf.SetActive(false);
            mainMenuProf.SetActive(true);
        }
    }

    /*
     * Purpose: To open the dateDetails UI with buttons
     * Input: Click on the date buttons in the resourceReservationUI, used by the listeners 
     * Output: Open the respective dateDetails UI
     */
    void OpenDateDetails(GameObject dateSelected)
    {
        Debug.Log("ResourceReservationUI openDetails");
        if (dateSelected.GetComponentInChildren<Text>().text != "")
        {
            Debug.Log("ResourceReservationUI openDetails " + dateSelected.GetComponentInChildren<Text>().text + currMonthYear.Month + currMonthYear.Year);

            RealmController = FindObjectOfType<RealmController>();
            buttonCreator = FindObjectOfType<DynamicButtonCreator>();

            if (resourceReservationStaff.activeSelf == true)
            {
                Debug.Log("ResourceReservationUI openDetails Staff");
                resourceReservationStaff.SetActive(false);
                dateDetailsStaff.SetActive(true);
                dateDetailsDateStaff.text = dateSelected.GetComponentInChildren<Text>().text + "/" + currMonthYear.Month + "/" + currMonthYear.Year;
            }
            else if (resourceReservationProf.activeSelf == true)
            {
                Debug.Log("ResourceReservationUI openDetails Prof");
                resourceReservationProf.SetActive(false);
                dateDetailsProf.SetActive(true);
                dateDetailsDateProf.text = dateSelected.GetComponentInChildren<Text>().text + "/" + currMonthYear.Month + "/" + currMonthYear.Year; 
            }

            if (RealmController != null)
            {
                var locationList = RealmController.GetLocations();
                if (locationList != null && locationList.Count > 0)
                {
                    foreach (var location in locationList)
                    {
                        buttonCreator.CreateButton(location);
                    }

                }
            }
 
        }

    }

    /*
     * Purpose: To open the "manageSlots" UI
     * Input: Click on the "manageSlotsButton" button in the "resourceReservationStaff" UI 
     * Output: Open the "manageSlots" UI
     */
    public void OpenReservationSlots()
    {
        if (resourceReservationStaff.activeSelf == true)
        {
            Debug.Log("Calendar openReservationSlots Staff");
            resourceReservationStaff.SetActive(false);
            reservationSlots.SetActive(true);
        }

    }

    /*
     * Purpose: To update the calendar of resourceReservation UIs
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
     * Purpose: To navigate between the different months when the "left" button is clicked in the resourceReservation UI
     * Input: Click on the "left" button
     * Output: Calls UpdateCalendar() with updated dates
     */
    public void PrevMonth()
    {
        currMonthYear = currMonthYear.AddMonths(-1);
        UpdateCalendar(currMonthYear.Year, currMonthYear.Month);
    }

    /*
     * Purpose: To navigate between the different months when the "right" button is clicked in the resourceReservation UI
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
     * Input: Called by DynamicButtonCreator by when creating timing buttons
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
     * Purpose: Attempt to get the user's (Prof) email for reservation purposes
     * Input: Called by Start() in ResourceReservationUI
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
     * Output: Debug.Log("ResourceReservationUI RetrieveUserEmailFail " + error);
     */
    void RetrieveUserEmailFail(PlayFabError error)
    {
        Debug.Log("ResourceReservationUI RetrieveUserEmailFail " + error);
    }

}

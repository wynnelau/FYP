using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;
using TMPro;

/*
 *Location: Main Scene, attached to "StudentControls"
 *Purpose: Manage the calendar
 *Tutorial used: https://www.youtube.com/watch?v=cMwCZhZnE4k
 */
public class Calendar : MonoBehaviour
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

    public GameObject resourceReservation, mainMenuOthers;
    public PlayerControls player;

    private List<Day> days = new List<Day>();
    public Transform[] weeks;
    public Text MonthAndYear;
    public DateTime currMonthYear = DateTime.Now;

    // Start is called before the first frame update
    /*
     * Purpose: Instantiate all Day gameObjects and addListener to all buttons in the calendar
     * Outcomes: addListener will be used when user clicks on the buttons
     */
    void Start()
    {
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
        for (int i = 0; i < 42; i++)
        {
            var dateSelected = days[i];
            dateSelected.dayButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { openDetails(dateSelected.dayButton); });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To close the calendar UI, attached to "closeResourceReservation" button
     * Outcomes: deactivate calendar UI
     */
    public void closeResourceReservation()
    {
        if (resourceReservation.activeSelf == true)
        {
            resourceReservation.SetActive(false);
        }
        mainMenuOthers.SetActive(true);
        player.enabled = true;
    }

    /*
     * Purpose: To open the respective details of the chosen date, 
     * Outcomes: open details of chosen date
     */
    public void openDetails(GameObject dateSelected)
    {
        if (resourceReservation.activeSelf == true)
        {
            resourceReservation.SetActive(false);
        }
        Debug.Log(dateSelected.GetComponentInChildren<Text>().text + currMonthYear.Month + currMonthYear.Year);

    }

    /*
     * Purpose: To update the calendar
     * Outcomes: updates calendar
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
     * Outcomes: return the first day of the month
     */
    int GetMonthStartDay(int dateYear, int dateMonth)
    {
        DateTime day = new DateTime(dateYear, dateMonth, 1);
        return (int)day.DayOfWeek;
    }

    /*
     * Purpose: To get the total number of days of the month
     * Outcomes: return number of days in the month
     */
    int GetTotalNumberOfDays(int dateYear, int dateMonth)
    {
        return DateTime.DaysInMonth(dateYear, dateMonth);
    }

    /*
     * Purpose: To navigate between the different months, attached to "left" button in resourceReservation
     * Outcomes: call UpdateCalendar with updated dates
     */
    public void PrevMonth()
    {
        currMonthYear = currMonthYear.AddMonths(-1);
        UpdateCalendar(currMonthYear.Year, currMonthYear.Month);
    }

    /*
     * Purpose: To navigate between the different months, attached to "right" button in resourceReservation
     * Outcomes: call UpdateCalendar with updated dates
     */
    public void NextMonth()
    {
        currMonthYear = currMonthYear.AddMonths(1);
        UpdateCalendar(currMonthYear.Year, currMonthYear.Month);
    }


}

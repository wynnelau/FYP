using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


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

        public void UpdateColor(Color newColor)
        {
            dayButton.GetComponent<Image>().color = newColor;
            availability = newColor;
        }

        public void UpdateDay(int newDayInt)
        {
            this.dayInt = newDayInt;
            if (availability == Color.white || availability == Color.green)
            {
                dayButton.GetComponentInChildren<Text>().text = (dayInt+1).ToString();
            }
            else
            {
                dayButton.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    private List<Day> days = new List<Day>();
    public Transform[] weeks;
    public Text MonthAndYear;
    public DateTime currDate = DateTime.Now;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(DateTime.Now.Year);
        Debug.Log(DateTime.Now.Month);
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateCalendar(int year, int month)
    {

        DateTime temp = new DateTime(year, month, 1);
        currDate = temp;
        Debug.Log(temp);
        MonthAndYear.text = temp.ToString("MMMM") + " " + temp.Year.ToString();
        int startDay = GetMonthStartDay(year, month);
        int endDay = GetTotalNumberOfDays(year, month);


        ///Create the days
        ///This only happens for our first Update Calendar when we have no Day objects therefore we must create them
        if (days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)
                {
                    Day newDay;
                    int currDay = (w * 7) + i;
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new Day(currDay - startDay, Color.grey, weeks[w].GetChild(i).gameObject);
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, Color.white, weeks[w].GetChild(i).gameObject);
                    }
                    days.Add(newDay);
                }
            }
        }
        ///loop through days
        ///Since we already have the days objects, we can just update them rather than creating new ones
        else
        {
            for (int i = 0; i < 42; i++)
            {
                if (i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateColor(Color.grey);
                }
                else
                {
                    days[i].UpdateColor(Color.white);
                }

                days[i].UpdateDay(i - startDay);
            }
        }

        ///This just checks if today is on our calendar. If so, we highlight it in green
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateColor(Color.green);
        }

    }

    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        //DayOfWeek Sunday == 0, Saturday == 6 etc.
        return (int)temp.DayOfWeek;
    }

    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    public void SwitchMonth(int direction)
    {
        if (direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }

        UpdateCalendar(currDate.Year, currDate.Month);
    }


}

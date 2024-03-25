﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EduCal {
    public partial class frmMain : Form 
    {
        public List<EventModel> Events { get; set; }
        public List<UserControlDays> UserDays { get; set; }
        public DateTime NowDate { get; set; }
        public EventForm CalEventForm { get; set; }
        public frmSettings settingMenu { get; set; }
        public frmSettings frmMainBackColor { get; set; }


        int month, year;
        public Color dayFore, dayBack, mainColor;


        public frmMain() 
        { 
            InitializeComponent();
            Events = new List<EventModel>();
            NowDate = DateTime.Now;
            dayFore = Color.Black;
            dayBack = Color.White;
            displaymonths();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void displaydays(int _dayoftheweek, int _days)
        {
            UserDays = new List<UserControlDays>();

            for (int i = 1; i < _dayoftheweek; i++)
            {
                UserControlBlank ucblank = new UserControlBlank();
                daycontainer.Controls.Add(ucblank);
            }

            for (int i = 1; i <= _days; i++)
            {
                UserControlDays newDay = new UserControlDays();
                DateTime uniqToday = DateTime.Parse($"{NowDate.Month}/{i}/{NowDate.Year}");

                if (uniqToday.DayOfWeek == DayOfWeek.Sunday || uniqToday.DayOfWeek == DayOfWeek.Saturday)
                {
                    newDay.weekEnd = true;
                }
                newDay.days(i);

                foreach (EventModel em in Events)
                {
                    if (em.isMutliDay)
                    {
                        if (em.eventStartDay.Date <= uniqToday.Date) 
                        {
                            if (em.eventEndDay.Date >= uniqToday.Date) 
                            {
                                newDay.ucTodaytxt = em.Name;
                            }
                        }
                    }
                    else 
                    {
                        if (em.eventStartDay.ToShortDateString() == uniqToday.ToShortDateString())
                        {
                            newDay.ucTodaytxt = em.Name;
                        }
                    }
                }
                UserDays.Add(newDay);
            }

            foreach (UserControlDays item in UserDays)
            {
                if (item.weekEnd)
                {
                    item.BackColor = Color.DarkGray;
                    item.ForeColor = Color.Gray;
                }
                else
                {
                    item.BackColor = dayBack;
                    item.ForeColor = dayFore;
                }

                item.popAdd += mnuFileEvent_Click;
                daycontainer.Controls.Add(item);
            }
        }

        private void displaymonths() 
        {
            daycontainer.Controls.Clear();

            month = NowDate.Month;
            year = NowDate.Year;

            String monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lblMonthYear.Text = monthname + " " + year;

            DateTime startofthemonth = new DateTime(year, month, 1);

            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            
            displaydays(dayoftheweek, days);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            NowDate = NowDate.AddMonths(-1);
            displaymonths();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            NowDate = NowDate.AddMonths(1);
            displaymonths();
        }

        private void about_Click(object sender, EventArgs e)
        {
            frmAbout TeamTwoNames = new frmAbout();
            TeamTwoNames.ShowDialog();
        }    

        private void mnuSettings_Click(object sender, EventArgs e)
        {
            settingMenu = new frmSettings();
            settingMenu.settingsChanged += mnuSetting_AddNew;
            settingMenu.frmMainBackground += mainBackgroundColor;
            settingMenu.Show();
        }

        private void mnuSetting_AddNew(object sender, ColorOfDayEventArgs e)
        {
            dayFore = e.foreColor;
            dayBack = e.backGroundColor;           
            displaymonths();
        }

        private void mainBackgroundColor(object sender, frmMainColorEventArgs e) 
        {
            this.BackColor = e.mainBackground;
        }

        private void mnuFileEvent_Click(object sender, EventArgs e)
        {
            CalEventForm = new EventForm();
            CalEventForm.eventfrmAdd += eventform_AddNew;
            CalEventForm.Show();
        }

        private void eventform_AddNew(object sender, AddEventArgs e)
        { 
            Events.Add(e.Model);
            displaymonths();
        }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
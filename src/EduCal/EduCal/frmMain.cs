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
        //ask why putting keyword(new) on line 17 removed the squiggly line
        public new List<EventModel> Events { get; set; }
        public List<UserControlDays> UserDays { get; set; }
        public DateTime NowDate { get; set; }
        public EventForm CalEventForm { get; set; }
        public frmSettings SettingMenu { get; set; }
        public frmSettings FrmMainBackColor { get; set; }


        int month, year;
        public Color dayFore, dayBack, mainColor;


        public frmMain() 
        { 
            InitializeComponent();
            Events = new List<EventModel>();
            NowDate = DateTime.Now;
            dayFore = Color.Black;
            dayBack = Color.White;
            Displaymonths();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void Displaydays(int _dayoftheweek, int _days)
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
                    newDay.WeekEnd = true;
                }
                newDay.Days(i);

                foreach (EventModel em in Events)
                {
                    if (em.isMutliDay)
                    {
                        if (em.EventStartDay.Date <= uniqToday.Date) 
                        {
                            if (em.EventEndDay.Date >= uniqToday.Date) 
                            {
                                newDay.UcTodaytxt = em.Name;
                            }
                        }
                    }
                    else 
                    {
                        if (em.EventStartDay.ToShortDateString() == uniqToday.ToShortDateString())
                        {
                            newDay.UcTodaytxt = em.Name;
                        }
                    }
                }
                UserDays.Add(newDay);
            }

            foreach (UserControlDays item in UserDays)
            {
                if (item.WeekEnd)
                {
                    item.BackColor = Color.DarkGray;
                    item.ForeColor = Color.Gray;
                }
                else
                {
                    item.BackColor = dayBack;
                    item.ForeColor = dayFore;
                }

                item.PopAdd += MnuFileEvent_Click;
                daycontainer.Controls.Add(item);
            }
        }

        private void Displaymonths() 
        {
            daycontainer.Controls.Clear();

            month = NowDate.Month;
            year = NowDate.Year;

            String monthname = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);
            lblMonthYear.Text = monthname + " " + year;

            DateTime startofthemonth = new DateTime(year, month, 1);

            int days = DateTime.DaysInMonth(year, month);
            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;
            
            Displaydays(dayoftheweek, days);
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            NowDate = NowDate.AddMonths(-1);
            Displaymonths();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            daycontainer.Controls.Clear();
            NowDate = NowDate.AddMonths(1);
            Displaymonths();
        }

        private void About_Click(object sender, EventArgs e)
        {
            frmAbout TeamTwoNames = new frmAbout();
            TeamTwoNames.ShowDialog();
        }    

        private void MnuSettings_Click(object sender, EventArgs e)
        {
            SettingMenu = new frmSettings();
            SettingMenu.SettingsChanged += MnuSetting_AddNew;
            SettingMenu.FrmMainBackground += MainBackgroundColor;
            SettingMenu.Show();
        }

        private void MnuSetting_AddNew(object sender, ColorOfDayEventArgs e)
        {
            dayFore = e.ForeColor;
            dayBack = e.BackGroundColor;           
            Displaymonths();
        }

        private void MainBackgroundColor(object sender, frmMainColorEventArgs e) 
        {
            this.BackColor = e.mainBackground;
        }

        private void MnuFileEvent_Click(object sender, EventArgs e)
        {
            CalEventForm = new EventForm();
            CalEventForm.EventfrmAdd += Eventform_AddNew;
            CalEventForm.Show();
        }

        private void Eventform_AddNew(object sender, AddEventArgs e)
        { 
            Events.Add(e.Model);
            Displaymonths();
        }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    }
}
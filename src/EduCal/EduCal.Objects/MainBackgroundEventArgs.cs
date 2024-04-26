﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EduCal
{
    public delegate void MainBackgroundEventHandler(object sender, MainBackgroundEventArgs e);
    public class MainBackgroundEventArgs:EventArgs
    {
        public Color MainBackground { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Embassy_Project
{
    class Constance
    {
        public static Thickness MoveMobileUp = new Thickness(0, -150, 0, 0);
        public static Thickness MoveMobileDown = new Thickness(0, 0, 0, 0);

        public static Double MoveMobileLeft = -200;

        private const Int16 TabletScreen = 1280;
        private const Int16 MainScreen = 3840;
        public const Int16 Screenratio = (Int16)(MainScreen / TabletScreen);
     
        public const double delayScrollerUpdate = 5.0;
        public const double phone_Width = 639;

       

       


        
    }


}

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Embassy_Project
{
	/// <summary>
	/// Interaction logic for IntrolPage.xaml
	/// </summary>
	public partial class IntrolPage : UserControl
	{
        public Storyboard introl_start;

		public IntrolPage()
		{
			this.InitializeComponent();
            introl_start = (Storyboard)TryFindResource("Introl_Start");

            ChangeIntrolPage(new Point(0,0));
           
		}
        public void ChangeIntrolPage(Point mobileLocation)
        {
            if (Global.lastMobileSelected != null)
            {
                phoneModel.Source = Global.LoadImage(new Uri(Global.lastMobileSelected.MobileSpecification.imagePath, UriKind.Relative));
                IntroNameText.Source = Global.LoadImage(new Uri(Global.lastMobileSelected.MobileSpecification.nametextPath, UriKind.Relative));
                phoneModel.Margin = new Thickness(mobileLocation.X + 210 ,mobileLocation.Y - 70,0,0);  //370 , -250
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] testfunction = {"SelectPhone","12"};
            Global.mainWindow.processFunction(testfunction);
            /*MobileManager.fliterMobileFromClient();
            Global.lastMobileSelected.blurRect.Visibility = System.Windows.Visibility.Collapsed;
         
            Global.scaleAnimation(Global.lastMobileSelected.glow2, 1.3, 1, 0.1, 0);
            Global.FadeinoutBtn(1, 0, Global.lastMobileSelected.glow2, 0.1, 0);*/
        }
	}
}
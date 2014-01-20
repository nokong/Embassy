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

            ChangeIntrolPage();
           
		}
        public void ChangeIntrolPage()
        {
            if (Global.lastMobileSelected != null)
            {
                phoneModel.Source = Global.LoadImage(new Uri(Global.lastMobileSelected.MobileSpecification.imagePath, UriKind.Relative));
                IntroNameText.Source = Global.LoadImage(new Uri(Global.lastMobileSelected.MobileSpecification.WHITEPATH, UriKind.Relative));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MobileManager.fliterMobileFromClient();
        }
	}
}
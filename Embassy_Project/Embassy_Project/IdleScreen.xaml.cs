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
using System.Threading;

namespace Embassy_Project
{
	/// <summary>
	/// Interaction logic for IdleScreen.xaml
	/// </summary>
	public partial class IdleScreen : UserControl
	{
        Storyboard TVC_Openning, TVC_Endding, TVC_Howto;
        Storyboard sb,sb2;


        private bool tvc_Appear;
        public bool TVC_Appear 
        {
            get { return tvc_Appear; }
            set
            {
                tvc_Appear = value;
                if (tvc_Appear)
                {
                    this.Visibility = Visibility.Visible;
                 
                    TVC_Openning.Begin();
                }
                else 
                {
                    TVC_Endding.Begin();
                }
            }
        }

        void TVC_Openning_Completed(object sender, EventArgs e)
        {
            string StoryBoardName = ((ClockGroup)sender).Timeline.Name;

            Console.WriteLine("Name :"+StoryBoardName);

            switch (StoryBoardName)
            {
                case "TVC_Openning":
                                     
                                      Video_Grid.Margin = new Thickness(268,0,0,0);
                                      tablet_Grid.Margin = new Thickness(0, 0, 0 - 176, -62);                
                                      this.vdo_BG_mp4.Play();
                                      this.TVC_Howto.Begin();
                                      ScrollToPosition(sb,639*3,0,0.7,1);
                                      sb.Begin();
                                     break;
                case "TVC_Howto":    //Thread.Sleep(7000);
                                     //Global.FadeinoutBtn(1, 1, tablet_Grid, 7, 0); //Animation for delay loop only
                                     if (this.tvc_Appear)
                                     {
                                         this.TVC_Howto.Begin();
                                         ScrollToPosition(sb, 639 * 3, 0, 0.7, 1);
                                         sb.Begin();
                                     }
                                     break; 

                case "TVC_Close"   : this.vdo_BG_mp4.Stop();
                                     
                                     TVC_Howto.Stop();
                                     Video_Grid.Margin = new Thickness(268,-1050,0,0);
                                     tablet_Grid.Margin = new Thickness(0, 0, -176, -866);
                                     sb.Stop();
                                     sb2.Stop();
                                     this.Visibility = Visibility.Collapsed;
                                     break;
                case "HandStep1":    sb.Stop();
                                     Global.mainWindow.phoneScroller.CurrentHorizontalOffset = 639 * 3;
                                     ScrollToPosition(sb2, 0, 0, 1.3, 1);
                                     sb2.Begin();
                                     break;
                case "HandStep2":    sb2.Stop();
                                     break;
            }
          
           
        }

        private void ScrollToPosition(Storyboard sb, double x, double y,double begintime, double duration)
        {
            DoubleAnimation vertAnim = new DoubleAnimation();
            vertAnim.From = Global.mainWindow.phoneScroller.VerticalOffset;
            vertAnim.To = y;
            vertAnim.DecelerationRatio = .2;
            vertAnim.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            DoubleAnimation horzAnim = new DoubleAnimation()
            {
                From = Global.mainWindow.phoneScroller.HorizontalOffset,
                To = x,
                DecelerationRatio = .2,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(duration)
            };

            sb.Children.Add(vertAnim);
            sb.Children.Add(horzAnim);
            Storyboard.SetTarget(vertAnim, Global.mainWindow.phoneScroller);
            Storyboard.SetTargetProperty(vertAnim, new PropertyPath(AniScrollViewer.CurrentVerticalOffsetProperty));
            Storyboard.SetTarget(horzAnim, Global.mainWindow.phoneScroller);
            Storyboard.SetTargetProperty(horzAnim, new PropertyPath(AniScrollViewer.CurrentHorizontalOffsetProperty));
            sb.Begin();
        }

		public IdleScreen()
		{
			this.InitializeComponent();
            this.Visibility = Visibility.Collapsed;
          

            TVC_Openning = (Storyboard)TryFindResource("TVC_Openning");
            TVC_Openning.Completed += new EventHandler(TVC_Openning_Completed);
            TVC_Openning.FillBehavior = FillBehavior.Stop;

            TVC_Endding = (Storyboard)TryFindResource("TVC_Close");
            TVC_Endding.Completed += new EventHandler(TVC_Openning_Completed);
            TVC_Endding.FillBehavior = FillBehavior.Stop;

            TVC_Howto = (Storyboard)TryFindResource("TVC_Howto");
            TVC_Howto.Completed += new EventHandler(TVC_Openning_Completed);
            TVC_Howto.FillBehavior = FillBehavior.Stop;

            sb2 = new Storyboard();
            sb2.Completed += new EventHandler(TVC_Openning_Completed);
            sb2.Name = "HandSetp2";

            sb = new Storyboard();
            sb.Completed += new EventHandler(TVC_Openning_Completed);
            sb.Name = "HandStep1";
            
            //this.vdo_BG_mp4.Play();
		}
	}
}
﻿using System;
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
using System.ComponentModel;
using System.Threading;

namespace Embassy_Project
{
	/// <summary>
	/// Interaction logic for PhoneDetail.xaml
	/// </summary>
	public partial class PhoneDetail : UserControl
	{
        List<PackageItem> PackageList = new List<PackageItem>();
        MobileItem currentMobile = null;

        Storyboard loop_animation;
        public Storyboard Detailin;
        public Storyboard Detail_Out;
        public Storyboard unjai_animation1, unjai_animation2;
        public Storyboard intro_start, intro_end;
        public Storyboard DetailChange;
        private DateTime newClickedTime = DateTime.Now;
        private DateTime oldClickedTime = DateTime.Now;
        private Uri BGPath;
        int defaultAnitmationIn = 0;
        int defaultAnimationOut = 0;
        public BackgroundWorker animationBackground;
        //Storyboard Detailin = new Storyboard(), ResetState, movepackage = new Storyboard(), Detail_Out = new Storyboard();
        public Boolean FirstState = false;

        private double packageStartTime = 0.5;
        private double packageAdditionStartTime = 0.10;

		public PhoneDetail()
		{
			this.InitializeComponent();
            Detailin = (Storyboard)TryFindResource("Detail_in");
            Detail_Out = (Storyboard)TryFindResource("Detail_Out");
            unjai_animation1 = (Storyboard)TryFindResource("unjai animation");
            unjai_animation2 = (Storyboard)TryFindResource("unjai2_animation");
            loop_animation = (Storyboard)TryFindResource("offer_animation");
            intro_start = (Storyboard)TryFindResource("Intro_Start");
            intro_end = (Storyboard)TryFindResource("Intro_Reverse");
            DetailChange = (Storyboard)TryFindResource("Detail_change");

            animationBackground = new BackgroundWorker();
            animationBackground.DoWork += new DoWorkEventHandler(animationBackground_DoWork);
            animationBackground.ProgressChanged += new ProgressChangedEventHandler(serverRunner_ProgressChanged);
            animationBackground.WorkerReportsProgress = true;
            animationBackground.WorkerSupportsCancellation = true;

            defaultAnitmationIn = Detailin.Children.Count;
            defaultAnimationOut = Detail_Out.Children.Count;

            this.Opacity = 0;
		}
        private void RandomBG()
        {
            Random rd = new Random();
            int randomvalue = rd.Next(0, 5);
            ////Console.WriteLine("Random Value = "+randomvalue);
            switch (randomvalue)
            {
                case 0: BGPath = new Uri("Resources\\Detailpage_BG.png", UriKind.Relative); break;
                case 1: BGPath = new Uri("Resources\\background_2.png", UriKind.Relative); break;
            }
        }
        private bool screenAppear;
        public bool ScreenAppear
        {
            get { return screenAppear; }
            set
            {
                screenAppear = value;

                if (screenAppear)
                {
                    if(!this.animationBackground.IsBusy)this.animationBackground.RunWorkerAsync();

                    //this.Opacity = 1;
                    Global.FadeinoutBtn(0, 1, this, 1, 0);
                    this.settingContentAnimationIN(Detailin);

                    Detailin.Begin();
                }
                else
                {
                    this.settingContentAnimationOUT(Detail_Out);
                    EventHandler handler = null;
                    handler = delegate
                    {
                        Detail_Out.Completed -= handler;
                        Detail_Out.Stop();
                        //Global.FadeinoutBtn(0, 1, currentMobile, 0.3, 0);
                        Global.FadeinoutBtn(1, 0, this, 0.3, 0);
                        MobileManager.returnAllMobile(currentMobile);
                        //MobileManager.fliterMobileFromClient();
                    };
                    Detail_Out.Completed += handler;
                    Detail_Out.Begin();
                    ////Console.WriteLine("Detail Out Animation State " + Detail_Out.GetCurrentState());
                    
                }
            }
        }

      

        void animationBackground_DoWork(object sender, DoWorkEventArgs e)
        {
           
            while (true)
            {
                ////Console.WriteLine("------------------------------------------------ start Thread");
                Thread.Sleep(12000);
                Random rd = new Random();
                int randomvalue = rd.Next(0, 5);

                animationBackground.ReportProgress(0, randomvalue);
                
            }
            
        }
        void serverRunner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int randomvalue = (int)e.UserState;
            ////Console.WriteLine("------------------------------ randomvalue {0}"+randomvalue);
            Storyboard animationRun = new Storyboard();
              switch (randomvalue)
                {
                    case 0: animationRun = unjai_animation1; break;
                    case 1: animationRun = unjai_animation2; break;
                    default: animationRun = unjai_animation2; break;
                }

                EventHandler handler = null;
                handler = delegate
                {
                    animationRun.Completed -= handler;
                  
                    animationRun.Stop();
                    //Thread.Sleep(5000);
                };
                animationRun.Completed += handler;
                animationRun.Begin();

        }



        public void PrePareForAnimation() 
        {
            Detail_Out.Stop();
            Detailin.Stop();

            setContentOpacity(0);
            CameraiTime.camera_image.Opacity = 1;

            if (PackageList.Count > 0)
            {
                for (int i = 0; i < PackageList.Count; i++)
                {
                    if (Detail_Scene.Children.Contains(PackageList[i]))
                    {
                        Detail_Scene.Children.Remove(PackageList[i]);
                    }
                }
                PackageList.Clear();
            }

            if (Detail_Out.Children.Count > defaultAnimationOut)
            {
                for (int i = Detail_Out.Children.Count; i > defaultAnimationOut; i--)
                {
                    Detail_Out.Children.RemoveAt(i - 1);
                }
            }

            if (Detailin.Children.Count > defaultAnitmationIn) 
            {
                for (int i = Detailin.Children.Count; i > defaultAnitmationIn ; i--)
                {
                    Detailin.Children.RemoveAt(i-1);
                }

            }
        }
        public void ChangeContentData(MobileItem _mobilteitem) 
        {
            currentMobile = _mobilteitem;

            newClickedTime = DateTime.Now;
            TimeSpan ClicktimeDuration = newClickedTime - oldClickedTime;
            int clickTimeSec = ClicktimeDuration.Seconds;

            Detailin.Stop();
            Detail_Out.Stop();

            RandomBG();

            if(screenAppear)
            {
                ////Console.WriteLine("Detail Out Animation Before State " + Detail_Out.GetCurrentProgress());
                //this.settingContentAnimationOUT(Detail_Out);

                EventHandler handler = null;
                handler = delegate
                {
                    DetailChange.Completed -= handler;

                    DetailChange.Stop();
                    //Thread.Sleep(5000);
                };
                DetailChange.Completed += handler;
                DetailChange.Begin();

                //Detail_Out.Begin();
                
                ////Console.WriteLine("Detail Out Animation After State " + Detail_Out.GetCurrentProgress());

            }
          
           
            ////Console.WriteLine("Detail Count" + Detail_Scene.Children.Count);
            oldClickedTime = newClickedTime;
               
            }
     
        private void setContent()
        {
            phoneModel.phoneModel.Source = Global.LoadImage(new Uri(currentMobile.MobileSpecification.imagePath, UriKind.Relative));
            WhiteNAmeText.Source = Global.LoadImage(new Uri(currentMobile.MobileSpecification.WHITEPATH, UriKind.Relative));
            priceText.Text = currentMobile.MobileSpecification.PRICETEXT;
            OSText.Text = currentMobile.MobileSpecification.OS + " " + currentMobile.MobileSpecification.OSVERSION;
            CPUText.Text = currentMobile.MobileSpecification.CPU + " GHz " + currentMobile.MobileSpecification.CPUCORE;
            StroageText.Text = currentMobile.MobileSpecification.STORAGE + " GB";
            DimensionText.Text = currentMobile.MobileSpecification.WIDTHMM + " x " + currentMobile.MobileSpecification.HEIGHTMM + " x " + currentMobile.MobileSpecification.THICK + " มม.";
            //BateryText.Text
            BatteryLifeText.Text = "คุยต่อเนื่องนาน " + currentMobile.MobileSpecification.BATTERYLIFE + " h(3G)";
            VideoText.Text = "FULL HD " + currentMobile.MobileSpecification.VIDEO;

            CameraiTime.camera_image.Source =Global.LoadImage(currentMobile.MobileSpecification.rearimage);
            CameraiTime.camera_image2.Source = Global.LoadImage(currentMobile.MobileSpecification.frontimage);
        }


        private void settingContentAnimationOUT(Storyboard _storyboardOUT) 
        {
            int count = 0;
            
            double startTime = 0.3;
            double additionStart = 0.10;

            ////Console.WriteLine("Before Add Animation Page Out Count : " + _storyboardOUT.Children.Count);
            foreach (UIElement Child in Detail_Scene.Children)
            {
                if (Child is PackageItem)
                {
                    PackageItem PI = (PackageItem)Child;
                    Global.FadeinoutBtn(1, 0, PI, startTime, 0.2);
                    AddTransitionAnimation(PI.Margin, new Thickness(430 + (535 * count), 250, 0, 0), startTime, 0.2, PI, _storyboardOUT);
                    startTime += additionStart;
                    count++;
                }
            }
           // //Console.WriteLine("After Add Animation Page Out Count : " + _storyboardOUT.Children.Count);

            /*AddMoveAnimation(phoneModel.screen1.Margin, new Thickness(22, 781, 0, 0), 0, 0.1, phoneModel.screen1, _storyboardOUT);
            AddMoveAnimation(phoneModel.screen2.Margin, new Thickness(22, -781, 0, 0), 0, 0.1, phoneModel.screen2, _storyboardOUT);
            AddMoveAnimation(phoneModel.screen3.Margin, new Thickness(442, 771, 0, 0), 0, 0.1, phoneModel.screen3, _storyboardOUT);*/

            EventHandler handler = null;
            handler = delegate
            {
                _storyboardOUT.Completed -= handler;
                _storyboardOUT.Stop();
                setContentOpacity(0);
                setContentMargin(storybordState.stateOut);
                loop_animation.Stop();
                CameraiTime.panPicture.Stop();

                //Console.WriteLine("Detail Out Animation State " + Detail_Out.GetCurrentProgress());
                //intro_start.Begin();
                settingContentAnimationIN(Detailin);
                Detailin.Begin();
                //PerPareForAnimation();
            };
            _storyboardOUT.Completed += handler;
        }
        private void settingContentAnimationIN(Storyboard _storyboardIn) 
        {
            int count = 0;
            double startTime = 0.3;
            double additionStart = 0.10;

            TimeSpan ClicktimeDuration = newClickedTime - oldClickedTime;
            int clickTimeSec = ClicktimeDuration.Seconds;
            
           
            PrePareForAnimation();
            //phoneModel.screen1.Margin = new Thickness(22, 771, 0, 0);
            //phoneModel.screen2.Margin = new Thickness(22, -691, 0, 0);
            //phoneModel.screen3.Margin = new Thickness(442, 771, 0, 0);
            ////Console.WriteLine("Mobile name : ");
            
            this.setContent();
            
            ////Console.WriteLine("Before Add Animation Page In Count : " + _storyboardIn.Children.Count);
            
            foreach (PackageItem packageItem in currentMobile.ListOfPackage)
            {

                packageItem.Margin = new Thickness(430 + (535 * count), 250, 0, 0);    //(430+(535*i),380,0,0);
                packageItem.Opacity = 0;
                PackageList.Add(packageItem);
                Detail_Scene.Children.Add(packageItem);
                Global.FadeinoutBtn(0, 1, packageItem, startTime, 0.2);
                AddTransitionAnimation(packageItem.Margin, new Thickness(430 + (535 * count), 380, 0, 0), startTime, 0.2, packageItem, _storyboardIn);

                startTime += additionStart;
                count++;

            }
            
            ////Console.WriteLine("After Add Animation Page In Count : " + _storyboardIn.Children.Count);

            /*AddMoveAnimation(phoneModel.screen1.Margin, new Thickness(22, 78, 0, 0), 2.3, 0.7, phoneModel.screen1, _storyboardIn);
            AddMoveAnimation(phoneModel.screen2.Margin, new Thickness(22, 78, 0, 0), 5, 0.7, phoneModel.screen2, _storyboardIn);
            AddMoveAnimation(phoneModel.screen3.Margin, new Thickness(22, 78, 0, 0), 8, 0.7, phoneModel.screen3, _storyboardIn);*/

            EventHandler handler = null;
            handler = delegate
            {
                _storyboardIn.Completed -= handler;
                _storyboardIn.Stop();

                setContentOpacity(1);
                setContentMargin(storybordState.stateIn);

                loop_animation.Begin();
                CameraiTime.panPicture.Begin();
                
            };
            _storyboardIn.Completed += handler;
           
        }
        private void setContentOpacity(double _value) 
        {
            ////Console.WriteLine("SetOpacity");
            img_Table.Opacity = _value;
            img_RectLogo.Opacity = _value;
            Decition_Text.Opacity = _value;
            priceText.Opacity = _value;
            Decition_Text_Copy1.Opacity = _value;
            path.Opacity = _value;
            image_OS.Opacity = _value;
            text_OSHeadText.Opacity = _value;
            OSText.Opacity = _value;
            image_CPU.Opacity = _value;
            text_CPUHeadText.Opacity = _value;
            CPUText.Opacity = _value;
            image_Store.Opacity = _value;
            text_StroageHeadtext.Opacity = _value;
            StroageText.Opacity = _value;
            WhiteNAmeText.Opacity = _value;
            phoneModel.Opacity = _value;
            DimensionText.Opacity = _value;
            BateryText.Opacity = _value;
            BatteryLifeText.Opacity = _value;
            VideoText.Opacity = _value;
            image_video.Opacity = _value;
            image_phone.Opacity = _value;
            image_battery.Opacity = _value;
            image_dimension.Opacity = _value;
            CameraiTime.Opacity = _value;
            image_special1.Opacity = _value;
            image_speical2.Opacity = _value;
            PromotionPic.Opacity = _value;

            int count = 0;
            foreach (UIElement Child in Detail_Scene.Children)
            {
                if (Child is PackageItem)
                {
                    PackageItem PI = (PackageItem)Child;
                    //PI.Margin = new Thickness(430 + (535 * count), 380, 0, 0);
                    PI.Opacity = _value;
                    count++;
                    ////Console.WriteLine("Opacity"+PI.Opacity);
                }
            }
            ////Console.WriteLine("package Item :"+count);
        }

        private void setContentMargin(storybordState _state) 
        {
            int packageSetTop = 0;
            if (_state == storybordState.stateIn) 
            {
                WhiteNAmeText.Margin = new Thickness(WhiteNAmeText.Margin.Left, 81, 0, 0);
                img_RectLogo.Margin = new Thickness(img_RectLogo.Margin.Left, 103, 0, 0);
                priceText.Margin = new Thickness(126, priceText.Margin.Top, 0, 0);
                image_CPU.Margin = new Thickness(123.5, image_CPU.Margin.Top, 0, 0);
                text_CPUHeadText.Margin = new Thickness(252, text_CPUHeadText.Margin.Top, 0, 0);
                CPUText.Margin = new Thickness(252, CPUText.Margin.Top, 0, 0);
                phoneModel.Margin = new Thickness(phoneModel.Margin.Left + 4, 174.5, 0, 0);
                CameraiTime.Margin = new Thickness(CameraiTime.Margin.Left, 513, 0, 0);
                packageSetTop = 380;
            }
            else 
            {
                WhiteNAmeText.Margin = new Thickness(124.331, 121.169, 0, 0);
                img_RectLogo.Margin = new Thickness(114.5, 99.5, 0, 0);
                priceText.Margin = new Thickness(123.583, 362.5, 0, 0);
                image_CPU.Margin = new Thickness(120.5, 700, 0, 0);
                text_CPUHeadText.Margin = new Thickness(249, 695, 0, 0);
                CPUText.Margin = new Thickness(249, 730, 0, 0);
                phoneModel.Margin = new Thickness(833.497, 185.8, 0, 0);
                CameraiTime.Margin = new Thickness(1337.999, 531.01, 0, 0);
                packageSetTop = 250;
            }

            int count = 0;
            foreach (UIElement Child in Detail_Scene.Children)
            {
                if (Child is PackageItem)
                {
                    PackageItem PI = (PackageItem)Child;
                    PI.Margin = new Thickness(430 + (535 * count), packageSetTop, 0, 0);
                    count++;
                }
            }
        }
      
        private void setAnimationParameter(storybordState _state) 
        {
            packageStartTime = 0.5;
            packageAdditionStartTime = 0.10;

            if (_state == storybordState.stateIn) 
            {
            
            }
            else 
            {
            
            }
        
        }


        private void AddTransitionAnimation(Thickness movefrom, Thickness moveTarget,double begintime,double duration, UIElement MoveItem,Storyboard _Storyboard)
        {
            ThicknessAnimation movegrid = new ThicknessAnimation()
            {
                From = movefrom,
                To = moveTarget,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(duration)
            };

            Storyboard.SetTarget(movegrid, MoveItem);
            Storyboard.SetTargetProperty(movegrid, new PropertyPath(Grid.MarginProperty));
           
            _Storyboard.Children.Add(movegrid);

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            screenAppear = false;
        	// TODO: Add event handler implementation here.
        }

        public enum storybordState 
        {
            stateIn,
            stateOut
        }
    
	}
}
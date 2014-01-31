using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Embassy_Project
{
    class Global
    {
        #region  Global property

        public static int maxPhoneItem;
        public static MainWindow mainWindow;

        //public static Boolean inDetail = false;
        public static PhoneDetail detailScene;
        public static IntrolPage  introlScene;

        public static int lastMobileSelectedID;
        public static int lastMobileSelectIndex;
        public static MobileItem lastMobileSelected;

        public static Dictionary<int,MobileItem> listOfMobileFillter = new Dictionary<int,MobileItem>();
        public static Dictionary<int, MobileItem> listOfMobileItem = new Dictionary<int, MobileItem>();

        public static List<MobileItemSpecification> listOfSpecification = new List<MobileItemSpecification>();
       

        public static List<string> listOfBrandFilter = new List<string>();
        public static void setlistOfBrandFilter(string[] _brandList)
        {
            listOfBrandFilter.Clear();

            for (int i = 1; i < _brandList.Length; i++)
            {
                listOfBrandFilter.Add(_brandList[i]);
            }
            listOfBrandFilter.RemoveAt(listOfBrandFilter.Count - 1);
        }

        public static List<string> listOfOSFilter = new List<string>();
        public static void setlistOfOSFilter(string[] _osList)
        {
            listOfOSFilter.Clear();
            for (int i = 1; i < _osList.Length; i++)
            {
                //Console.WriteLine(_osList[i]);
                listOfOSFilter.Add(_osList[i]);
            }
            listOfOSFilter.RemoveAt(listOfOSFilter.Count - 1);
        }
        #endregion

        //------------------------------------------------------------------------------------------------------- End of Porperty

        
        public static void ShowSearchResult(Dictionary<int,MobileItem> NewResultList, Dictionary<int,MobileItem> OldResultList)
        {
            //mainWindow.phoneStack.Children.Clear();

            Thickness OldMobileMargin, NewMobileMargin;
            Boolean FindContent = false;

            //Storyboard sb = new Storyboard();

            /*foreach (MobileItem NewMobile in NewResultList.Values) 
            {
                foreach (MobileItem OldMobile in OldResultList.Values) 
                {
                    if (NewMobile.IDPHONE == OldMobile.IDPHONE)
                    {
                        NewMobileMargin = new Thickness(NewMobile.Width * j, 200, 0, 0);
                        Global.TransitionAnimation(OldMobile.Margin, NewMobileMargin, OldMobile, null, 0);
                    }
                   
                }
            }*/

            for (int i = 0; i < OldResultList.Count; i++)
			{
                 MobileItem OldMobile = OldResultList.ElementAt(i).Value;
                 FindContent = false;
			    for (int j = 0; j < NewResultList.Count; j++)
			    {
                   
			        MobileItem NewMobile = NewResultList.ElementAt(j).Value;

                    if (OldMobile.IDPHONE == NewMobile.IDPHONE)
                    {
                        NewMobileMargin = new Thickness(NewMobile.Width * j, 200, 0, 0);
                        Global.TransitionAnimation(OldMobile.Margin, NewMobileMargin, OldMobile, null, 0);
                        mainWindow.phoneStack.Children[i].Visibility = Visibility.Visible;
                        FindContent = true;
                        //NewMobile.Margin = new Thickness(NewMobile.Width * j, 200, 0, 0);
                    }
			    }
                if (!FindContent) mainWindow.phoneStack.Children[i].Visibility = Visibility.Hidden;
			}


            /*foreach (MobileItem OldMobile in OldResultList.Values) 
            {
                foreach (MobileItem NewMobile in NewResultList.Values) 
                {
                    if (OldMobile.IDPHONE != NewMobile.IDPHONE) 
                    {
                        mainWindow.phoneStack.Children[]
                    }
                }
            }*/

            /*foreach (MobileItem mobile in _resultList.Values) 
            {
               // Console.WriteLine(mobile.MobileSpecification.NAME + "Befor Change Margin " +mobile.Margin);
                Global.scaleAnimation(mobile, 1, 1, 0.1);
                mobile.Margin = new Thickness(mobile.Width * count, 200, 0, 0);
                mainWindow.phoneStack.Children.Add(mobile);
              //  Console.WriteLine(mobile.MobileSpecification.NAME + "After Change Margin " + mobile.Margin);
              //  Console.WriteLine("-----------------------------------------------------------------------");
                count++;
            }*/
           /* for (int i = 0; i < _resultList.Count; i++)
            {
                MobileItem MBItem = (MobileItem)_resultList[i];
                mainWindow.phoneStack.Children.Add(MBItem);
            }*/
            mainWindow.phoneStack.Width = (Constance.phone_Width * maxPhoneItem);
        }


        public static BitmapImage LoadImage(Uri uri)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.UriSource = uri;
            bitmapImage.EndInit();

            return bitmapImage;
        }

        public static void TransitionAnimation(Thickness movefrom, Thickness moveTarget, Image MoveItem, double begintime = 0, double duration = 0.4)
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

            Storyboard st = new Storyboard();
            EventHandler handler = null;
            handler = delegate
            {
                st.Completed -= handler;
                st.Stop();
                MoveItem.Margin = moveTarget;


            };
            st.Children.Add(movegrid);
            st.Completed += handler;
            st.Begin();
        }

        public static void TransitionAnimation(Thickness movefrom, Thickness moveTarget, StackPanel MoveItem, Storyboard transitionStoryboard, double begintime = 0, double duration = 0.4)
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

            EventHandler handler = null;
            handler = delegate
            {
                transitionStoryboard.Completed -= handler;
                transitionStoryboard.Stop();
                MoveItem.Margin = moveTarget;
                

            };
            transitionStoryboard.Children.Add(movegrid);
            transitionStoryboard.Completed += handler;
        }

        public static void scaleAnimation(UIElement mobile, double from, double to, double duration = 0.75,double begintime = 0) 
        {

            ScaleTransform sc = new ScaleTransform();
            mobile.RenderTransformOrigin = new Point(0.5, 0.5);
            mobile.RenderTransform = sc;

            DoubleAnimation scaleanime = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(duration)

            };
           
            sc.BeginAnimation(ScaleTransform.ScaleXProperty,scaleanime);
            sc.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanime);

        }

        public static void TransitionAnimation(Thickness movefrom, Thickness moveTarget, MobileItem MoveItem, Storyboard transitionStoryboard = null, double begintime = 0, double duration = 1)
        {
            bool storyboardnull = false;
            ThicknessAnimation movegrid = new ThicknessAnimation()
            {
                From = movefrom,
                To = moveTarget,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(duration)
            };
            QuarticEase be = new QuarticEase();
            movegrid.EasingFunction = be;
            
            Storyboard.SetTarget(movegrid, MoveItem);
            Storyboard.SetTargetProperty(movegrid, new PropertyPath(Grid.MarginProperty));

            if (transitionStoryboard == null) { transitionStoryboard = new Storyboard(); storyboardnull= true; }
          
            EventHandler handler = null;
            handler = delegate
            {
                transitionStoryboard.Completed -= handler;
                transitionStoryboard.Stop();
                MoveItem.Margin = moveTarget;
                //Console.WriteLine(MoveItem.MobileSpecification.NAME + " Margin : " + MoveItem.Margin);
           
            };
            transitionStoryboard.Children.Add(movegrid);
            transitionStoryboard.Completed += handler;
            if (storyboardnull) { transitionStoryboard.Begin(); }
            //sb.Begin();
        }

        public static void FadeinoutBtn(double from, double to, UIElement target,Storyboard fadeStoryboard, double speed, double begintime)
        {
            target.IsHitTestVisible = false;

            //Console.WriteLine(fadeStoryboard.Name);

            DoubleAnimation fadeanimation = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(speed)

            };

            Storyboard.SetTarget(fadeanimation, target);
            Storyboard.SetTargetProperty(fadeanimation, new PropertyPath(UIElement.OpacityProperty));

            EventHandler handler = null;
            handler = delegate
            {
                fadeStoryboard.Completed -= handler;

                //Console.WriteLine("Complete storyboard");

            };

            fadeStoryboard.Completed += handler;
            fadeStoryboard.Children.Add(fadeanimation);
            //Console.WriteLine("Add complete");
        }

        public static void FadeinoutBtn(double from, double to, UIElement target, double speed, double begintime)
        {
            target.IsHitTestVisible = false;

            DoubleAnimation fadeanimation = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(speed)

            };

            Storyboard.SetTarget(fadeanimation, target);
            Storyboard.SetTargetProperty(fadeanimation, new PropertyPath(UIElement.OpacityProperty));

            Storyboard sb = new Storyboard();

            EventHandler handler = null;
            handler = delegate
            {
                sb.Completed -= handler;



            };

            sb.Completed += handler;
            sb.Children.Add(fadeanimation);
            sb.Begin();
        }


        public static void ChangeRadiusAnimation(double from, double to, UIElement target, double speed, double begintime = 0)
        {
        
            DoubleAnimation radius = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(speed)
            };

            target.BeginAnimation(EllipseGeometry.RadiusXProperty, radius);
            target.BeginAnimation(EllipseGeometry.RadiusYProperty, radius);

        }

        public static void BlurEffectAnimation(double from, double to, UIElement target,Storyboard targetStoryBoard,double speed,double begintime = 0 ) 
        {
            DropShadowEffect dropshadowEffect = new DropShadowEffect();
            dropshadowEffect.Color = Color.FromRgb(255, 255, 255);
            dropshadowEffect.Opacity = 1;
            dropshadowEffect.ShadowDepth = 0;

            //BlurEffect blurEffect = new BlurEffect();
            object blurObject = mainWindow.FindName("dropshadowEffect");
            if (blurObject != null) 
            {
                mainWindow.UnregisterName("dropshadowEffect");
            }


            mainWindow.RegisterName("dropshadowEffect", dropshadowEffect);

            target.Effect = dropshadowEffect;

            
            DoubleAnimation blurAnimation = new DoubleAnimation
            {
                From = from,
                To = to,
                BeginTime = TimeSpan.FromSeconds(begintime),
                Duration = TimeSpan.FromSeconds(speed)
            };

            Storyboard.SetTargetName(blurAnimation, "dropshadowEffect");
            //Storyboard.SetTarget(blurAnimation, target);
            Storyboard.SetTargetProperty(blurAnimation, new PropertyPath(DropShadowEffect.BlurRadiusProperty));

            targetStoryBoard.Children.Add(blurAnimation);
               
        }
    }
}

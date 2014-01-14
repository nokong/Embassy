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

namespace Embassy_Project
{
    class Global
    {
        #region  Global property

        public static int maxPhoneItem;
        public static MainWindow mainWindow;

        //public static Boolean inDetail = false;
        public static PhoneDetail Scene2;


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
                Console.WriteLine(_osList[i]);
                listOfOSFilter.Add(_osList[i]);
            }
            listOfOSFilter.RemoveAt(listOfOSFilter.Count - 1);
        }
        #endregion

        //------------------------------------------------------------------------------------------------------- End of Porperty

        
        public static void ShowSearchResult(Dictionary<int,MobileItem> _resultList)
        {
            mainWindow.phoneStack.Children.Clear();

            int count = 0;
            foreach (MobileItem mobile in _resultList.Values) 
            {
                //Console.WriteLine(mobile.MobileSpecification.NAME);
                mobile.Margin = new Thickness(mobile.Width * count, 0, 0, 0);
                mainWindow.phoneStack.Children.Add(mobile);

                count++;
            }

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

            DoubleAnimation scaleanime = new DoubleAnimation(from, to, TimeSpan.FromSeconds(duration));
            sc.BeginAnimation(ScaleTransform.ScaleXProperty,scaleanime);
            sc.BeginAnimation(ScaleTransform.ScaleYProperty, scaleanime);

          
        }

        public static void TransitionAnimation(Thickness movefrom, Thickness moveTarget, MobileItem MoveItem,Storyboard transitionStoryboard = null, double begintime = 0, double duration = 1)
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

            //Storyboard sb = new Storyboard();

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
            //sb.Begin();
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


                if (to == 1.0)
                { target.IsHitTestVisible = true; }

                else
                {
                    target.IsHitTestVisible = false;

                }

            };

            sb.Completed += handler;
            sb.Children.Add(fadeanimation);
            sb.Begin();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for CameraItem.xaml
    /// </summary>
    public partial class CameraItem : UserControl
    {
        public Storyboard panPicture;
        public Storyboard panPicture2;
        public CameraItem()
        {
            InitializeComponent();
            panPicture = (Storyboard)TryFindResource("scaleandmove");

            EventHandler handler2 = null;
            handler2 = delegate
            {
                panPicture.Completed -= handler2;
                panPicture.Stop();
                camera_image.Opacity = 0;
                EventHandler handler3 = null;
                handler3 = delegate
                {
                    panPicture2.Completed -= handler3;
                    panPicture2.Stop();

                    TransformGroup trGrp = new TransformGroup();

                    TranslateTransform trTns = new TranslateTransform(-80.5, -3.389); ;
                    ScaleTransform trScl = new ScaleTransform(1.293, 1.293);

                    trGrp.Children.Add(trTns);
                    trGrp.Children.Add(trScl);

                    camera_image2.RenderTransform = trGrp;
                    //CameraiTime.camera_image2.Opacity = 1;
                };
                panPicture2.Completed += handler3;
                panPicture2.Begin();


            };
            panPicture.Completed += handler2;
            panPicture2 = (Storyboard)TryFindResource("scale2");

        }
    }
}

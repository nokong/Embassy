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
using System.Xml;
using System.Windows.Media.Animation;

namespace Embassy_Project
{
    /// <summary>
    /// Interaction logic for MobileItem.xaml
    /// </summary>
    public partial class MobileItem : UserControl
    {
        public int IDPHONE = 0, MAXPACKAGE = 0;
        public MobileItemSpecification MobileSpecification;
        public List<PackageItem> ListOfPackage;
        public Storyboard ClickedAnimation;
        public Storyboard aurastart;
        int comparemonth;

        public MobileItem() { }

        public MobileItem(XmlElement phoneNode = null)
        {
            InitializeComponent();
            MobileSpecification = new MobileItemSpecification();
            ListOfPackage = new List<PackageItem>();
            ClickedAnimation = (Storyboard)TryFindResource("phoneClick");
            aurastart = (Storyboard)TryFindResource("aurastart");

            MobileSpecification = new MobileItemSpecification(phoneNode);


            this.InitPhoneContent(MobileSpecification);
        }

        public void InitPhoneContent(MobileItemSpecification _specification)
        {
  
            this.MobileSpecification = _specification;

            comparemonth = DateTime.Now.Month - MobileSpecification.DATESALE.Month;
            //Console.WriteLine("Month Now : {0} Release Month : {1}", DateTime.Now.Month, mobileSpecification.DATESALE);

            this.frontPhone.Source = Global.LoadImage(new Uri(MobileSpecification.imagePath, UriKind.Relative));
            //this.shadow_frontPhone.Source = Global.LoadImage(new Uri(mobileSpecification.imagePath, UriKind.Relative));
            this.headerName.Source = Global.LoadImage(new Uri(MobileSpecification.WHITEPATH, UriKind.Relative));
            this.priceTextBlock.Text = MobileSpecification.PRICETEXT + " บาท";

            if (comparemonth == 0) { _new.Visibility = Visibility.Visible; }
            else _new.Visibility = Visibility.Collapsed;

            //Console.WriteLine("Mobilename {0} : CompareDate : {1} ",MobileSpecification.NAME,comparemonth);

            this.initSpecailItem(MobileSpecification.PROMOTION);

            this.initPackageItem(MobileSpecification.PACKAGENAME);

            //this.Tag = _phoneTag;
        }
        public void initSpecailItem(bool _haveSpecail) 
        {
            if (_haveSpecail && comparemonth > 0) 
            {
                specail_offer.Visibility = Visibility.Visible;
            }
            else specail_offer.Visibility = Visibility.Collapsed;
        }

        public void initPackageItem(String _Packagename) 
        {
            XmlDocument document = new XmlDocument();
            document.Load("Resources\\PackageSpecification.xml");

            XmlElement rootNode = document.DocumentElement;

            int maxPackageitem = rootNode.GetElementsByTagName(_Packagename).Count;

            XmlNodeList itemList = rootNode.SelectNodes(_Packagename + "Package/" + _Packagename);
            //_mainWindow.phoneStack.Width = (phone_Width * max);
            foreach (XmlNode currNode in itemList)
            {
                PackageItem PI = new PackageItem();

                PI.packageName = _Packagename;

                PI.PRICE = int.Parse(currNode.Attributes["PRICE"].Value);


                PI.haveSMS = bool.Parse(currNode.ChildNodes[1].Attributes["VALID"].Value);
                PI.haveMMS = bool.Parse(currNode.ChildNodes[2].Attributes["VALID"].Value);
                PI.boolwifiUnlimit = bool.Parse(currNode.ChildNodes[4].Attributes["VALID"].Value);
                PI.haveEBook = bool.Parse(currNode.ChildNodes[5].Attributes["VALID"].Value);
                PI.haveMoviestore = bool.Parse(currNode.ChildNodes[6].Attributes["VALID"].Value);
                PI.boolfreeTalk = bool.Parse(currNode.ChildNodes[7].Attributes["VALID"].Value);
                PI.haveTalktime = bool.Parse(currNode.ChildNodes[0].Attributes["VALID"].Value);

                PI.INTERNETUNLIMIT = currNode.ChildNodes[3].Attributes["UNLIMITTYPE"].Value;
                PI.INTERNETUSAGE = double.Parse(currNode.ChildNodes[3].Attributes["MAXUSAGE"].Value);
                PI.TALKTIME = int.Parse(currNode.ChildNodes[0].Attributes["AMOUNT"].Value);
                PI.SMSAMOUNT = int.Parse(currNode.ChildNodes[1].Attributes["AMOUNT"].Value);
                PI.MMSAMOUNT = int.Parse(currNode.ChildNodes[2].Attributes["AMOUNT"].Value);
                PI.EBOOKAMOUNT = int.Parse(currNode.ChildNodes[5].Attributes["AMOUNT"].Value);
                PI.MOVIEAMOUNT = int.Parse(currNode.ChildNodes[6].Attributes["AMOUNT"].Value);
                // XmlNode title = currNode.SelectSingleNode("Title");
                // XmlNode body = currNode.SelectSingleNode("Body");
                PI.settingContent();
                ListOfPackage.Add(PI);
            }

        }

   
    }
}

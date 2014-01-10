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
using System.Xml;

namespace Embassy_Project
{
	/// <summary>
	/// Interaction logic for PackageItem.xaml
	/// </summary>
	public partial class PackageItem : UserControl
	{

        public XmlElement rootNode;
        public XmlDocument document;
        public int maxPackageitem;
        public string packageName = "";

      

        public bool  haveSMS = false,
                     haveMMS = false,
                     haveEBook = false,
                     haveTalktime = false,
                     haveMoviestore = false,
                     boolwifiUnlimit = false,
                     boolfreeTalk = false;

		public PackageItem()
		{
			this.InitializeComponent();
		}
        public void setDefultItem() 
        {
            talkfree.Visibility = Visibility.Collapsed;
            grid_TalkTime.Visibility = Visibility.Collapsed;
            grid_3G.Visibility = Visibility.Collapsed;
            grid_msm.Visibility = Visibility.Collapsed;
            grid_semi_3G.Visibility = Visibility.Collapsed;
            grid_wifi.Visibility = Visibility.Collapsed;
            grid_ebook.Visibility = Visibility.Collapsed;
            grid_movie.Visibility = Visibility.Collapsed;
            limitusage.Visibility = Visibility.Collapsed;
            bill.Visibility = Visibility.Collapsed;
        }
        public void settingContent() 
        {
            int countColumn = 0;
            double columnheight = 170.733;
            int maxcolumn = 4;
            double columnspace = 0;
            setDefultItem();
            
            this.TXT_PackageName.Text = this.packageName;
            this.PackagePrice.Text = "" + this.PRICE;


            if (boolfreeTalk) talkfree.Visibility = Visibility.Visible;

            if (haveTalktime) 
            {
                grid_TalkTime.Visibility = Visibility.Visible;
                this.TalkTime.Text = "" + this.TALKTIME;
                countColumn++;
            }
            if (haveSMS) 
            {
                grid_sms.Visibility = Visibility.Visible;
                this.SmsText.Text =""+this.SMSAMOUNT;
                countColumn++;
            }
            if (haveMMS) 
            {
                grid_msm.Visibility = Visibility.Visible;
                this.MMSText.Text = "" + this.MMSAMOUNT;
                countColumn++;
            }
            switch (INTERNETUNLIMIT) 
            {
                case "Unlimit":
                    grid_3G.Visibility = Visibility.Visible;
                    grid_3G.Height = 45.135;
                    limitusage.Visibility = Visibility.Visible;
                    _3GText.Text = "ไม่จำกัด*";
                    if (this.INTERNETUSAGE < 1)
                    {
                        usage.Text = "(ใช้ความเร็วสูงสุดรวม " + (this.INTERNETUSAGE * 1000) + " MB)";
                    }
                    else { usage.Text = "(ใช้ความเร็วสูงสุดรวม " + this.INTERNETUSAGE + " GB)"; }
                    countColumn++;
                    break;
                case "Semilimit":
                    grid_3G.Visibility = Visibility.Visible;
                    grid_3G.Height = 59.135;
                    grid_semi_3G.Visibility = Visibility.Visible;
                    limitusage.Visibility = Visibility.Visible;
                    bill.Visibility = Visibility.Visible;

                    _3GText.Text = "ไม่จำกัด*";
                    if (this.INTERNETUSAGE < 500)
                    {
                        usage.Text = "(ใช้ความเร็วสูงสุดรวม " + (this.INTERNETUSAGE * 100) + " MB)";
                        semi_3GText1.Text =(this.INTERNETUSAGE * 100) + " MB" ;
                    }
                    else 
                    {
                     usage.Text = "(ใช้ความเร็วสูงสุดรวม " + this.INTERNETUSAGE + " GB)";
                     semi_3GText1.Text = (this.INTERNETUSAGE * 100)+ " MB";
                    }
                    countColumn += 2;
                    break;
                case "None":
                    grid_3G.Visibility = Visibility.Visible;
                    grid_3G.Height = 32.135;
                    if (this.INTERNETUSAGE < 500)
                    {
                        _3GText.Text = "(ใช้ความเร็วสูงสุดรวม " + (this.INTERNETUSAGE * 100) + " MB)";
                    }
                    else { _3GText.Text = "(ใช้ความเร็วสูงสุดรวม " + this.INTERNETUSAGE + " GB)"; }
                    countColumn++;
                    break;
            }
            if (boolwifiUnlimit) { grid_wifi.Visibility = Visibility.Visible; countColumn++; }
            if (haveEBook) 
            {
                grid_ebook.Visibility = Visibility.Visible;
                eBook_amount.Text =""+ this.EBOOKAMOUNT;
                if (EBOOKAMOUNT < 10) ebooktype.Margin = new Thickness(68, 14, 0, 0);
                else if (EBOOKAMOUNT > 10 && EBOOKAMOUNT < 100) ebooktype.Margin = new Thickness(65, 14, 0, 0);
                else { ebooktype.Margin = new Thickness(82, 14, 0, 0); }
                countColumn++;
            }
            if (haveMoviestore) 
            {
                grid_movie.Visibility = Visibility.Visible;
                movie_amount.Text = "" + this.MOVIEAMOUNT;
                countColumn++;
                
            }
            //Console.WriteLine("Count Item : "+countColumn);
           /* if (countColumn < 4)
            {
                columnspace = ( (columnheight * maxcolumn) - (columnheight * countColumn)) / 5;
                Console.WriteLine("column space : "+columnspace);
            }
            else if (countColumn == 4)
            {
                columnspace = 10;
            }
            else { countColumn = 0; }
          

               foreach(UIElement child in itemPackage.Children)
               {
                   if (child is Grid) 
                   {
                       Grid gridChild = (Grid)child;
                       if (!gridChild.Name.Equals("grid_TalkTime"))gridChild.Margin = new Thickness(0, columnspace, 0, 0);
                   }
                   else if (child is StackPanel) 
                   {
                       StackPanel stackChild = (StackPanel)child;
                       stackChild.Margin = new Thickness(0, columnspace, 0, 0);
                   }
               }*/
            
            
        }

  

        

        public int PRICE {get; set;}
        public int TALKTIME {get; set;}
        public int SMSAMOUNT { get; set; }
        public int MMSAMOUNT { get; set; }

        public string INTERNETUNLIMIT { get; set; }
        public double INTERNETUSAGE {get; set;}
        //public bool WIFI { get; set; }
        public int EBOOKAMOUNT { get; set; }
        public int MOVIEAMOUNT { get; set; }
        public bool FREETALK { get; set; }
        

	}
}
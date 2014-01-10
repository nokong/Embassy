using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Embassy_Project
{
    public class MobileItemSpecification
    {
        public MobileItemSpecification(XmlElement mobileData = null) 
        {
            this.SetMobileData(mobileData);
               
        }

        private void SetMobileData(XmlElement mobileData) 
        {
            if (mobileData != null)
            {
                this.imagePath = mobileData.GetElementsByTagName("ModelPath")[0].InnerText;
                this.nametextPath = mobileData.GetElementsByTagName("LogoPath")[0].InnerText;
                this.WHITEPATH = mobileData.GetElementsByTagName("WhiteLogoPath")[0].InnerText;

                this.NAME = mobileData.GetElementsByTagName("name")[0].InnerText;
                this.PRICE = Int32.Parse(mobileData.GetElementsByTagName("price")[0].InnerText);
                this.PRICETEXT = mobileData.GetElementsByTagName("pricetext")[0].InnerText;
                this.BRAND = mobileData.GetElementsByTagName("brand")[0].InnerText;
                this.OS = mobileData.GetElementsByTagName("os")[0].InnerText;
                this.RECOMMEND = Int32.Parse(mobileData.GetElementsByTagName("recommend")[0].InnerText);
                this.BESTSALE = Int32.Parse(mobileData.GetElementsByTagName("bestsale")[0].InnerText);
                this.OSVERSION = mobileData.GetElementsByTagName("OSVersion")[0].InnerText;
                this.CPU = mobileData.GetElementsByTagName("CPU")[0].InnerText;
                this.CPUCORE = mobileData.GetElementsByTagName("Core")[0].InnerText;
                this.RAM = Int16.Parse(mobileData.GetElementsByTagName("RAMM")[0].InnerText);
                this.CAMERAFRONT = mobileData.GetElementsByTagName("CameraFront")[0].InnerText;
                this.CAMERABACK = mobileData.GetElementsByTagName("CameraBack")[0].InnerText;
                this.VIDEO = Int16.Parse(mobileData.GetElementsByTagName("VideoFps")[0].InnerText);
                this.WIDTHMM = Double.Parse(mobileData.GetElementsByTagName("Widthmm")[0].InnerText);
                this.HEIGHTMM = Double.Parse(mobileData.GetElementsByTagName("Heightmm")[0].InnerText);
                this.WEIGHTG = Double.Parse(mobileData.GetElementsByTagName("WeightG")[0].InnerText);
                this.THICK = Double.Parse(mobileData.GetElementsByTagName("Thickmm")[0].InnerText);
                this.SCREENWIDTH = Int16.Parse(mobileData.GetElementsByTagName("ScreenWidth")[0].InnerText);
                this.SCREENHEIGHT = Int16.Parse(mobileData.GetElementsByTagName("ScreenHeight")[0].InnerText);
                this.SCREENDIAMETER = Double.Parse(mobileData.GetElementsByTagName("CameraBack")[0].InnerText);
                //Batterthis.BATTERYTYPE = xmlfile.GetElementsByTagName("CameraBack")[0].InnerText;
                this.BATTERYLIFE = mobileData.GetElementsByTagName("BatteryLife")[0].InnerText;
                this.STORAGE = mobileData.GetElementsByTagName("StorageG")[0].InnerText;
                this.PACKAGENAME = mobileData.GetElementsByTagName("PackageName")[0].InnerText;
                this.PROMOTION = Boolean.Parse(mobileData.GetElementsByTagName("Promotion")[0].InnerText);
                this.DATESALE = DateTime.ParseExact(mobileData.GetElementsByTagName("datesale")[0].InnerText, "d/M/yyyy", null);

                this.rearimage = new Uri("Resources\\Phone\\camera\\rear_camera\\rear_" + this.NAME + ".png", UriKind.Relative);
                this.lowlightimage = new Uri("Resources\\Phone\\camera\\low_light_camera\\lowlight_" + this.NAME + ".png", UriKind.Relative);
                this.frontimage = new Uri("Resources\\Phone\\camera\\front_camera\\front_" + this.NAME + ".png", UriKind.Relative);
            }
        }

        public string imagePath = "";
        public string nametextPath = "";
        public Uri rearimage;
        public Uri lowlightimage;
        public Uri frontimage;
        public string WHITEPATH { get;  set; }
        public string NAME { get;  set; }
        public string BRAND { get;  set; }
        public int PRICE { get;  set; }
        public string PRICETEXT { get;  set; }
        public string OS { get;  set; }
        public string OSVERSION { get;  set; }
        public string CPU { get;  set; }
        public int RAM { get;  set; }
        public String CAMERAFRONT { get;  set; }
        public String CPUCORE { get;  set; }
        public String CAMERABACK { get;  set; }
        public int VIDEO { get;  set; }
        public Double WIDTHMM { get;  set; }
        public Double HEIGHTMM { get;  set; }
        public Double THICK { get;  set; }
        public Double WEIGHTG { get;  set; }
        public int SCREENWIDTH { get;  set; }
        public int SCREENHEIGHT { get;  set; }
        public Double SCREENDIAMETER { get;  set; }
        public string BATTERYTYPE { get;  set; }
        public String BATTERYLIFE { get;  set; }
        public string STORAGE { get;  set; }
        public int RECOMMEND { get;  set; }
        public int BESTSALE { get;  set; }
        public DateTime DATESALE { get;  set; }
        public string PACKAGENAME { get;  set; }
        public bool PROMOTION { get;  set; }
    }
}

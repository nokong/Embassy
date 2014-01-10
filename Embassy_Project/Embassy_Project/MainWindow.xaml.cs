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
using System.ComponentModel;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Windows.Media.Animation;


namespace Embassy_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Global.mainWindow = this;
            this.initPhoneDataFromXMLFile("Resources\\Phone\\specification.xml");

            ServerRunner = new BackgroundWorker();
            ServerRunner.DoWork += new DoWorkEventHandler(serverRunner_DoWork);
            ServerRunner.ProgressChanged += new ProgressChangedEventHandler(serverRunner_ProgressChanged);
            ServerRunner.WorkerReportsProgress = true;

            UpdateScroller = new BackgroundWorker();
            UpdateScroller.DoWork += new DoWorkEventHandler(UpdateScoroler_DoWork);
            UpdateScroller.ProgressChanged += new ProgressChangedEventHandler(UpdateScoroler_ProgressChanged);
            UpdateScroller.WorkerReportsProgress = true;

    
            /*CheckIdle = new BackgroundWorker();
            CheckIdle.DoWork += new DoWorkEventHandler(C);
            CheckIdle.ProgressChanged += new ProgressChangedEventHandler(UpdateScoroler_ProgressChanged);
            CheckIdle.WorkerReportsProgress = true;*/
            

        }

        void initPhoneDataFromXMLFile(string dir)
        {

            XmlElement rootNode;
            XmlDocument document = new XmlDocument();

            document.Load(dir);
            rootNode = document.DocumentElement;

            Global.maxPhoneItem = rootNode.GetElementsByTagName("Phone").Count;

            for (int i = 0; i < Global.maxPhoneItem; i++)
            {
                XmlElement childNode = (XmlElement)rootNode.ChildNodes[i];
                MobileItemSpecification specification = new MobileItemSpecification(childNode);
                MobileItem MB = new MobileItem(childNode);
                MB.IDPHONE = i;
                //Console.WriteLine("Name : " + MB.MobileSpecification.NAME + " ID : "+MB.IDPHONE);
                listOfSpecification.Add(specification);
                Global.listOfMobileItem.Add(i,MB);
            }
            Global.listOfMobileFillter = Global.listOfMobileItem;

         
        }

        void UpdateScoroler_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (dataSendDuration > 0)
            {   //Console.WriteLine("+++      >  " + duration + " Current :" + phoneScroller.CurrentHorizontalOffset + " To " + scrollTo + " diff " + diffSClient);
                if (phoneScroller.CurrentHorizontalOffset < scrollTo)
                {
                    double newPoint = phoneScroller.CurrentHorizontalOffset + diffSClient;

                    if (newPoint > scrollTo) this.phoneScroller.CurrentHorizontalOffset = scrollTo;

                    else this.phoneScroller.CurrentHorizontalOffset = newPoint;

                    //Console.WriteLine("------------------- > UIThread less current : " + phoneScroller.CurrentHorizontalOffset + " to " + scrollTo + " diff " + diffSClient);
                }
                else if (phoneScroller.CurrentHorizontalOffset > scrollTo)
                {

                    double newPoint = phoneScroller.CurrentHorizontalOffset - diffSClient;

                    if (newPoint < scrollTo) this.phoneScroller.CurrentHorizontalOffset = scrollTo;

                    else this.phoneScroller.CurrentHorizontalOffset = newPoint;

                    //Console.WriteLine("------------------- > UIThread more  current : " + scrollTo + " to " + phoneScroller.CurrentHorizontalOffset + " diff " + diffSClient);
                }
            }
        }
        void UpdateScoroler_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                UpdateScroller.ReportProgress(0, 0);
                Thread.Sleep((int)Constance.delayScrollerUpdate);
            }
        }


        void serverRunner_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TimeSpan dataSendtime = newDataSendTime - oldDataSendTime;

            Int32 dataSendMilli = dataSendtime.Milliseconds;
            
            Byte[] content = (Byte[])e.UserState;
            char[] delimiterChars = { ',', '|' };
            Int32 newscrollToTablet = 0;

            string[] FunctionList;

            if (content.Length == 2)
            {
                int newx = (content[0] * 100) + content[1];
                if (newx == newscrollToTablet) return;
                newscrollToTablet = (content[0] * 100) + content[1];

                //Console.WriteLine("update duration Old X : " + OldData + " New X : " + scrollTo + " now : " + now.Millisecond + " sar " + SARTime.Millisecond + " duration: " + duration);
                //Console.WriteLine("recieve : " + newTime.Minute + ":" + newTime.Second + ":" + newTime.Millisecond + " --> " + newscrollTo);
                if (dataSendMilli > 0 && OldscrollToData != newscrollToTablet)
                {
                    dataSendDuration = dataSendMilli;

                    newScrollToTabletData = newscrollToTablet;


                    scrollTo = newScrollToTabletData * Constance.Screenratio;

                    if (scrollTo > phoneScroller.CurrentHorizontalOffset)
                    {
                        diffSClient = (Int32)((scrollTo - phoneScroller.CurrentHorizontalOffset) / Constance.delayScrollerUpdate);

                    }
                    else if (scrollTo < phoneScroller.CurrentHorizontalOffset)
                    {
                        diffSClient = (Int32)((phoneScroller.CurrentHorizontalOffset - scrollTo) / Constance.delayScrollerUpdate);
                    }

                    if (diffSClient < 1) diffSClient = 1;

                    //Console.WriteLine(" duration: " + duration);

                    OldscrollToData = newScrollToTabletData;
                    oldDataSendTime = newDataSendTime;
                }
            }
            else
            {
                //List<MobileItem> currentList = Global.listOfMobileFillter;
                FunctionList = System.Text.Encoding.UTF8.GetString(content).Split(delimiterChars);
                processFunction(FunctionList);
            }
        }
        void serverRunner_DoWork(object sender, DoWorkEventArgs doWorke)
        {

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress send_to_address = ipHostInfo.AddressList[0];
                Console.WriteLine(send_to_address);
                
                Socket sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                UdpClient listener = new UdpClient(37777);
                IPEndPoint sending_end_point = new IPEndPoint(send_to_address, 37777);
             

                string received_data;
                byte[] receive_byte_array;
                try
                {
                    while (true)
                    {
                        TimeSpan onActionTime = DateTime.Now - newTouchScreenTime;
                        //Console.WriteLine("NoActionTime {0}:{1}", onActionTime.Minutes, onActionTime.Seconds);

                        newDataSendTime = DateTime.Now;
                       
                        receive_byte_array = listener.Receive(ref sending_end_point);
                        client_IP = sending_end_point.Address;
                        if (FirstTimeStartProgram)
                        {
                            //Console.WriteLine(listener.);
                            

                            IPEndPoint RemoteEndPoint = new IPEndPoint(client_IP, 7777);
                            Socket server = new Socket(AddressFamily.InterNetwork,
                                                       SocketType.Dgram, ProtocolType.Udp);
                            string start_srever = "true";
                            byte[] data = Encoding.ASCII.GetBytes(start_srever);
                            server.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);

                           
                            FirstTimeStartProgram = false;

                           
                        }
                        ServerRunner.ReportProgress(0, receive_byte_array);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error in receive Sockets : " + e.Message);
            }
             
        }

        void processFunction(string[] functionList)
        {
            String Function = functionList[0];
            switch (Function)
            {
                case "Brand": Global.setlistOfBrandFilter(functionList); break;
                case "OS": Global.setlistOfOSFilter(functionList); break;
                case "Price": MobileManager.minPrice = Int32.Parse(functionList[1]);
                    MobileManager.maxPrice = Int32.Parse(functionList[2]);
                    break;
                case "SelectPhone":

                    if (Global.Scene2 == null) { Global.Scene2 = new PhoneDetail(); Scene1.Children.Add(Global.Scene2);  }

                    /*if (Global.lastMobileSelected.IDPHONE == Int32.Parse(functionList[1]) && Global.Scene2.ScreenAppear)
                    {
                        mobileReturn = true;
                        Constance.Scene2.ScreenAppear = false;
                       
                    }
                    else { mobileReturn = false; }*/

                    Global.lastMobileSelectedID = Int32.Parse(functionList[1]);

                   for (int i = 0; i < Global.listOfMobileFillter.Count; i++)
			       {
                       if(Global.listOfMobileFillter.ElementAt(i).Key == Global.lastMobileSelectedID)
                       {
                           Global.lastMobileSelected = Global.listOfMobileFillter.ElementAt(i).Value;
                           Global.lastMobileSelectIndex = i;
                       }
			       }

               
            
                    #region CustomAnimation For Selected Phone
                    /*foreach (MobileItem mobileItem in Global.listOfMobileFillter)
                    {
                        if (mobileItem.IDPHONE == Global.lastMobileSelected && !mobileReturn)
                        {
                            Constance.Scene2.ChangeContentData(mobileItem);
                            if (!Constance.Scene2.ScreenAppear)
                            {
                                //Global.FadeinoutBtn(1, 0, mobileItem, 0.3, 0);
                                Global.TransitionAnimation(mobileItem.Margin, Constance.MoveMobileUp, mobileItem, MobileTransitionChangeScene);

                                if (!Constance.Scene2.ScreenAppear && !mobileReturn)
                                {

                                    Constance.Scene2.ScreenAppear = true;
                                    //Global.FadeinoutBtn(0, 1, mainWindow.ShowDetail, 0.3, 0);
                                    //inDetail = true;
                                }
                                //ShowDetail.FirstState = true;
                               
                                
                            }

                        }
                        else 
                        {
                            Global.TransitionAnimation(mobileItem.Margin, Constance.MoveMobileLeft, mobileItem, MobileTransitionChangeScene);
                        }
                    }*/
#endregion

                    #region All Content Side to Left
                    /*double AdditionMarginLeft = Global.lastMobileSelected.Width * 5;
                    Console.WriteLine("Last Touch  "+Global.lastMobileSelected.MobileSpecification.NAME + " Margin : "+Global.lastMobileSelected.Margin );

                    //Canvas.SetLeft(Global.listOfMobileFillter.ElementAt(0).Value, -1000);
                    Global.FadeinoutBtn(1, 0, phoneStack, 0.5, 0.25);
                    Global.TransitionAnimation(phoneStack.Margin, new Thickness(-AdditionMarginLeft, 0, 0, 0),phoneStack, MobileTransitionChangeScene,0,0.5);
                    
                      EventHandler handler = null;
                    handler = delegate
                    {
                    MobileTransitionChangeScene.Completed -= handler;
                    MobileTransitionChangeScene.Stop();

                    if (Global.Scene2 != null && !Global.Scene2.ScreenAppear) 
                        {
                            Global.Scene2.ChangeContentData(Global.lastMobileSelected);
                            Global.Scene2.ScreenAppear = true;
                        }

                    };

                    MobileTransitionChangeScene.Completed += handler;
                    MobileTransitionChangeScene.Begin();

                    if (Global.Scene2.ScreenAppear) { Global.Scene2.ChangeContentData(Global.lastMobileSelected); }*/
                    #endregion

                    #region Slide Left and Right
                   for (int i = 0; i < Global.listOfMobileFillter.Count; i++)
                   {
                       if (i < Global.lastMobileSelectIndex)
                       {
                           Global.TransitionAnimation(Global.listOfMobileFillter.ElementAt(i).Value.Margin, new Thickness(-2556, 0, 0, 0), Global.listOfMobileFillter.ElementAt(i).Value);
                       }
                       else if (i == Global.lastMobileSelectIndex) { }
                       else 
                       {
                           Global.TransitionAnimation(Global.listOfMobileFillter.ElementAt(i).Value.Margin, new Thickness(639, 0, 0, 0), Global.listOfMobileFillter.ElementAt(i).Value);
                       }
                   }
                    MobileTransitionChangeScene.Begin();
                    #endregion

                    break;
                case "reccomnd":
                    switch (functionList[1])
                    {
                        case "NewRelease": MobileManager.sortAndShowMobile(MobileManager.SortBy.NewRelease); break;
                        case "BestSell": MobileManager.sortAndShowMobile(MobileManager.SortBy.BestSell); break;
                        case "Recommend": MobileManager.sortAndShowMobile(MobileManager.SortBy.Recommend); break;
                    }
                    break;
                case "Time":

                    newTouchScreenTime = DateTime.Parse(functionList[1]);
                    TimeSpan touchTimePeriod = newTouchScreenTime - oldTouchScreenTime;
                   
                    Console.WriteLine("New Time" + newTouchScreenTime + " - Old Time" + oldTouchScreenTime + " = " + touchTimePeriod);

                    oldTouchScreenTime = newTouchScreenTime;
                    break;
            }
            if (!Function.Equals("SelectPhone")&&!Function.Equals("Time"))
            {
               /* if (Global.Scene2 != null && Global.Scene2.ScreenAppear)
                {
                    Global.Scene2.ScreenAppear = false;
                    mobileReturn = true;
                    //Global.inDetail = false;
                }*/
                MobileManager.fliterMobileFromClient();
                
            }
        }
      
        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
         
            ServerRunner.RunWorkerAsync();
            UpdateScroller.RunWorkerAsync();
        	// TODO: Add event handler implementation here.
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
          /*  Global.FadeinoutBtn(1, 0, Global.listOfMobileItem[0], 0.3, 0);
            ShowDetail.ChangeContentData(Global.listOfMobileItem[0]);
            Global.MobileTransitionAnimation(Global.listOfMobileItem[0].Margin, Constance.MoveMobileUp, Global.listOfMobileItem[0], 0, 0.4);*/
        	// TODO: Add event handler implementation here.
        }

        #region property

        #region ServerRunner 

        IPAddress client_IP;
        BackgroundWorker ServerRunner;
        Boolean FirstTimeStartProgram = true;

        private DateTime newDataSendTime = DateTime.Now;
        private DateTime oldDataSendTime = DateTime.Now;
        private Int32 dataSendDuration;

       
        #endregion

        #region UpdateScroller 

        BackgroundWorker UpdateScroller;

        private Int32 diffSClient;
        private Int32 OldscrollToData = 0;
        private Int32 newScrollToTabletData;
        private Int32 scrollTo;
        #endregion

        #region CheckTime For Idle

        BackgroundWorker CheckIdle;

        public DateTime newTouchScreenTime = DateTime.Now;
        public DateTime oldTouchScreenTime = DateTime.Now;

        #endregion

        #region List of Mobile Content Data
        public  List<MobileItemSpecification> listOfSpecification = new List<MobileItemSpecification>();
        public bool mobileReturn = false;

        public Storyboard MobileTransitionChangeScene = new Storyboard();

        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (MobileItem mb in Global.listOfMobileFillter.Values)
            {
                mb.Margin = new Thickness(0, 0, 0, 0);
            }
        }






        #endregion
    }
}

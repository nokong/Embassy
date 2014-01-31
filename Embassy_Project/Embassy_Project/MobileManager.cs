using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Embassy_Project
{
    class MobileManager
    {
        public static SortBy sortValue;

        public static int minPrice = 1000, maxPrice = 24000;
        public static Dictionary<int,MobileItem> PerparForFilter = new Dictionary<int,MobileItem>();

        public static void sortAndShowMobile(SortBy _sortType)
        {
            sortValue = _sortType;
            PerparForFilter = sortMobile(PerparForFilter, _sortType);
            Global.ShowSearchResult(PerparForFilter,Global.listOfMobileFillter);
        }
        public static void fliterMobileFromClient()
        {
            if (Global.listOfBrandFilter.Count == 0 && Global.listOfOSFilter.Count == 0)
            {
                PerparForFilter = FliterMobile(Global.listOfMobileItem);

            }
            else if (Global.listOfBrandFilter.Count == 0 && Global.listOfOSFilter.Count > 0)
            {
                PerparForFilter = FliterMobile(Global.listOfMobileItem, Global.listOfOSFilter, SortBy.OS);

            }
            else if (Global.listOfBrandFilter.Count > 0 && Global.listOfOSFilter.Count == 0)
            {
                PerparForFilter = FliterMobile(Global.listOfMobileItem, Global.listOfBrandFilter, SortBy.Brand);
            }
            else
            {
                Dictionary<int, MobileItem> sortByBrandlist = new Dictionary<int, MobileItem>();
                sortByBrandlist = FliterMobile(Global.listOfMobileItem, Global.listOfBrandFilter,SortBy.Brand);
                PerparForFilter = FliterMobile(sortByBrandlist, Global.listOfOSFilter, SortBy.OS);
            }
            sortAndShowMobile(sortValue);
        }

        private static Dictionary<int,MobileItem> sortMobile(Dictionary<int,MobileItem> _sortList, SortBy sortState)
        {
            List<KeyValuePair<int, MobileItem>> SortList = _sortList.ToList();

            SortList.Sort
            (
               delegate(KeyValuePair<int, MobileItem> firstPair,
    KeyValuePair<int, MobileItem> nextPair)
                {
                    //return firstObj.DATESALE.CompareTo(secondObj.DATESALE);
                    object firstObjSort = null, secondObjSort = null;
                    int value = 0;
                    switch (sortState)
                    {  
                        case SortBy.NewRelease: firstObjSort = firstPair.Value.MobileSpecification.DATESALE;
                            secondObjSort = nextPair.Value.MobileSpecification.DATESALE;
                            break;
                        case SortBy.Recommend: firstObjSort = firstPair.Value.MobileSpecification.RECOMMEND;
                            secondObjSort = nextPair.Value.MobileSpecification.RECOMMEND;
                            break;
                        case SortBy.BestSell: firstObjSort = firstPair.Value.MobileSpecification.BESTSALE;
                            secondObjSort = nextPair.Value.MobileSpecification.BESTSALE;
                            break;
                        case SortBy.Price: firstObjSort = firstPair.Value.MobileSpecification.PRICE;
                            secondObjSort = nextPair.Value.MobileSpecification.PRICE;
                            break;
                    }
                    if (firstObjSort is int && secondObjSort is int)
                    {
                        if ((int)firstObjSort == (int)secondObjSort) value = 0;
                        else if ((int)firstObjSort < (int)secondObjSort) value = 1;
                        else value = -1;
                    }
                    else if (firstObjSort is DateTime && secondObjSort is DateTime)
                    {
                        if ((DateTime)firstObjSort == (DateTime)secondObjSort) value = 0;
                        else if ((DateTime)firstObjSort < (DateTime)secondObjSort) value = 1;
                        else value = -1;
                    }

                    return value;

                }
            );
            _sortList = SortList.Distinct().ToDictionary(x => x.Key, x => x.Value);
            return _sortList;
        }

        private static Dictionary<int,MobileItem> FliterMobile(Dictionary<int,MobileItem> _mobileList)
        {
            Dictionary<int,MobileItem> resultList = new Dictionary<int,MobileItem>();
            for (int j = 0; j < _mobileList.Count(); j++)
            {
                MobileItem MB = (MobileItem)_mobileList[j];
                if (MB.MobileSpecification.PRICE > minPrice &&
                     MB.MobileSpecification.PRICE < maxPrice)
                {
                    resultList.Add(MB.IDPHONE,MB);
                }
            }
            return resultList;
        }
        private static Dictionary<int,MobileItem> FliterMobile(Dictionary<int,MobileItem> _mobileList, List<String> _listOfFilter,SortBy _sortType)
        {
            Dictionary<int, MobileItem> resultList = new Dictionary<int, MobileItem>();
            for (int i = 0; i < _listOfFilter.Count(); i++)
            {
                //Console.WriteLine(_listOfFilter);
                for (int j = 0; j < _mobileList.Count(); j++)
                {
                    MobileItem MB = (MobileItem)_mobileList.ElementAt(j).Value;
                    switch (_sortType) 
                    {
                        case SortBy.OS:
                            if (MB.MobileSpecification.OS.Contains(_listOfFilter[i]) &&
                                MB.MobileSpecification.PRICE > minPrice &&
                                MB.MobileSpecification.PRICE < maxPrice
                                )
                            {
                                resultList.Add(MB.IDPHONE,MB);
                                //Console.WriteLine(MB.mobileSpecification.NAME);
                            }
                            break;
                        case SortBy.Brand:
                            if (MB.MobileSpecification.BRAND.Contains(_listOfFilter[i]) &&
                                MB.MobileSpecification.PRICE > minPrice &&
                                MB.MobileSpecification.PRICE < maxPrice
                      )
                            {
                                resultList.Add(MB.IDPHONE,MB);
                                //Console.WriteLine(MB.mobileSpecification.NAME);
                            }
                            break;
                    }
                   
                }

            }
            return resultList;
        }

       
        public static void returnAllMobile(MobileItem selectedPhone)
        {
            int count = 0;

            foreach (MobileItem MB in Global.listOfMobileFillter.Values) 
            {
                MB.Margin = new Thickness(MB.Width * count, 200, 0, 0);
                //MB.Margin = new Thickness(10,10, 0, 0);
                //Global.FadeinoutBtn(0, 1, MB, 0.1, 0);
                ScaleTransform ST = new ScaleTransform(1, 1, 0.5, 0.5); 
                MB.frontPhone.RenderTransform = ST;
                MB.RenderTransform = ST;

                DropShadowEffect glowEffect = new DropShadowEffect();
                glowEffect.BlurRadius = 0;
                glowEffect.Opacity = 0.4;

                MB.blurRect.Effect = glowEffect;

                MB.glow2.Opacity = 0;
                Global.scaleAnimation(MB.glow2, 1.3, 1.1, 0.1,0);
             
                //MB.Opacity = 1;
                //Console.WriteLine("Phone : "+MB.MobileSpecification.NAME +" Margin : "+MB.Margin);
                count++;
            }
            //Global.Scene2 = null;
            //Global.mainWindow.Introl = null;
            //Global.mainWindow.mobileReturn = false;
        }
        


        public enum SortBy
        {
            NewRelease = 0,
            Recommend = 1,
            BestSell = 2,
            Price = 3,
            OS = 4,
            Brand = 5
        };

        
    }
}

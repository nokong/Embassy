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

namespace Embassy_Project
{
	/// <summary>
	/// Interaction logic for PhoneModel.xaml
	/// </summary>
	public partial class PhoneModel : UserControl
	{
        public Storyboard PageSwip;
		public PhoneModel()
		{
			this.InitializeComponent();
            PageSwip = (Storyboard)TryFindResource("pageswip");
		}
	}
}
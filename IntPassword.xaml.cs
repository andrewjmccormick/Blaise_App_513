﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Blaise_App
{
    /// <summary>
    /// Interaction logic for IntPassword.xaml
    /// </summary>
    public partial class IntPassword : Window
    {
        public IntPassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AppController.Instance._intPass = UserPassword.Text;
        }
        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            AppController.Instance._intPass = "Cancel";
        }
    }
}

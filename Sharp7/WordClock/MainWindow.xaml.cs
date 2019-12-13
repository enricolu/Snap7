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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WordClock
{

    public partial class MainWindow : Window
    {

        public bool TaskAktiv;
        public bool DatenRangierenAktiv = true;
        public bool FensterAktiv = true;

        public TimeSpan Time;



        public MainWindow()
        {
            InitializeComponent();
            EinAusgabeFelderInitialisieren();
            System.Threading.Tasks.Task.Run(() => SPS_Pingen_Task());
            System.Threading.Tasks.Task.Run(() => Logikfunktionen_Task());

            DateTime dateTime = DateTime.Now;
            Time = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FensterAktiv = false;
        }

        private void ZeitUebernehmen_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            Time = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
        }
    }
}
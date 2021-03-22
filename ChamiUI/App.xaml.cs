﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ChamiUI.BusinessLayer;
using ChamiUI.BusinessLayer.Logger;
using ChamiUI.PresentationLayer;
using ChamiUI.Windows.MainWindow;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;

namespace ChamiUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
#if !DEBUG
            DispatcherUnhandledException += ShowExceptionMessageBox;
#endif
            Logger = new ChamiLogger();
            Logger.AddFileSink("chami.log");
        }
        
        public ChamiLogger Logger { get; }

        public static string GetConnectionString()
        {
            var chamiDirectory = Environment.CurrentDirectory;
            return String.Format(ConfigurationManager.ConnectionStrings["Chami"].ConnectionString, chamiDirectory);
        }

        public void ShowExceptionMessageBox(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var exceptionMessage = args.Exception.Message;
            args.Handled = true;
            MessageBox.Show(exceptionMessage, "An exception occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
            var logger = Logger.GetLogger();
            logger.Error(exceptionMessage);
            logger.Error(args.Exception.StackTrace);
        }

        public Logger GetLogger()
        {
            return Logger.GetLogger();
        }
    }
}
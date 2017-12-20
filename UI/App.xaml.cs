using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            mutex = new Mutex(false, "Team@M Maze");
            if (!mutex.WaitOne(0, false))
            {
                MessageBox.Show("App already running");
                Environment.Exit(0);
            }
        }
    }
}

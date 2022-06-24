using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(BackgroundService.Droid.BackgroundWorkerService))]
namespace BackgroundService.Droid
{
    public class BackgroundWorkerService : IBackgroundWorkerService
    {
        public void Start()
        {
            BackgroundWorker.ExecuteNow();
        }
    }
}
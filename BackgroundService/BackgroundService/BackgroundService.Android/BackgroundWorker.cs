using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.Work;
using System;
using System.Threading;
using Xamarin.Essentials;

namespace BackgroundService.Droid
{
    public class BackgroundWorker : Worker
    {
        public const string TAG = "BackgroundWorker";
        private NotificationManager notificationManager;
        const int SERVICE_RUNNING_NOTIFICATION_ID = 123;
        const string NOTIFICATION_CHANNEL_ID = "REP";

        public BackgroundWorker(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {
            notificationManager = (NotificationManager)context.GetSystemService(Android.Content.Context.NotificationService);
        }    
        
        public override Result DoWork()
        {
            SetForegroundAsync(CreateForegroundInfo());
            try
            {
                Log.Info(TAG, "Executing worker");
                //execution
                //if async methodAsync().GetAwaiter().GetResult();
                Thread.Sleep(5000);
            }
            catch (System.Exception ex)
            {
                //Crashes.TrackError(ex, null, ErrorAttachmentLog.AttachmentWithText(ConcactException(ex), "error.txt"));
            }
            //EnqueueSelf(TimeSpan.FromMinutes(1));
            return Result.InvokeSuccess();
        }
        string ConcactException(System.Exception ex, System.Text.StringBuilder str = null)
        {
            if (str == null)
                str = new System.Text.StringBuilder();
            str.AppendLine($"Message: {ex.Message}");
            str.AppendLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
                str.AppendLine(ConcactException(ex.InnerException, str));
            return str.ToString();
        }

        string _id = Guid.NewGuid().ToString();

        public static void ExecuteNow()
           => EnqueueSelf(TimeSpan.FromMilliseconds(5000));

        public static void EnqueueSelf(TimeSpan initialDelay)
        {
            Log.Info(TAG, "Setting up worker");
            var constraints = new Constraints.Builder()
                            .SetRequiredNetworkType(NetworkType.Connected)
                            .Build();

            var work = new OneTimeWorkRequest.Builder(typeof(BackgroundWorker))
                                   .SetInitialDelay(initialDelay)
                                   .SetConstraints(constraints)
                                   .AddTag(TAG)
                                   .Build();

            WorkManager.GetInstance(Application.Context).EnqueueUniqueWork(TAG, ExistingWorkPolicy.Replace, work);
        }

        private ForegroundInfo CreateForegroundInfo()
        {
            var appname = AppInfo.Name;
            // Build a notification using bytesRead and contentLength
            var notification = new NotificationCompat.Builder(ApplicationContext, NOTIFICATION_CHANNEL_ID)
                                   .SetContentTitle(appname)
                                   .SetContentText("Uploading images")
                                   .SetSmallIcon(Resource.Drawable.notification_icon_background)
                                   .SetOngoing(true)
                                   .Build();
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                CreateChannel();
            }
            return new ForegroundInfo(SERVICE_RUNNING_NOTIFICATION_ID, notification);
        }
        private void CreateChannel()
        {
            var chan = new NotificationChannel(NOTIFICATION_CHANNEL_ID, "On-going Notification", NotificationImportance.Min);
            notificationManager.CreateNotificationChannel(chan);
        }
    }
}
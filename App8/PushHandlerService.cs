using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using WindowsAzure.Messaging;
using Android.App;
using Android.Content;
using Android.Util;
using Gcm.Client;
using Android.Support.V4.App;


namespace App8
{

    [Service] //
    public class PushHandlerService : GcmServiceBase
    {
       
        public static string perdoruesi { get;  set; }
        public static int nr_not { get; set; } = 0;
        public static string RegistrationID { get; private set; }
        private NotificationHub Hub { get; set; }
        public static Context Context;

        public PushHandlerService() : base(Constants.SenderID)
        {
            Log.Info(AzurePushBroadcastReceiver.TAG, "PushHandlerService() constructor");
        }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose(AzurePushBroadcastReceiver.TAG, "GCM Registered: " + registrationId);
            RegistrationID = registrationId;

            createNotification("InfoNotat",
                                "Sherbimi u aktivizua !");
            
            Hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString,
                                        context);
            try
            {
                Hub.UnregisterAll(registrationId);
            }
            catch (Exception ex)
            {
                Log.Error(AzurePushBroadcastReceiver.TAG, ex.Message);
            }

            var tags = new List<string>() { perdoruesi }; // tags
           // var tags = new List<string>() { };

            try
            {
                var hubRegistration = Hub.Register(registrationId, tags.ToArray());
            }
            catch (Exception ex)
            {
                Log.Error(AzurePushBroadcastReceiver.TAG, ex.Message);
            }
        }

        protected override void OnMessage(Context context, Intent intent)
        {

            var msg = new StringBuilder();

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                    msg.AppendLine(key + "=" + intent.Extras.Get(key));

                string messageText = intent.Extras.GetString("message");
                if (!string.IsNullOrEmpty(messageText))
                {
                    createNotification("InfoNotat", messageText);
                    nr_not = nr_not + 1;
                }
                else
                {
                    createNotification("Ndodhi nje gabim ! ", msg.ToString());
                }
            }
        }

        void createNotification(string title, string desc)
        {
            //Create notification
           NotificationManager notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show UI
            var uiIntent = new Intent(this, typeof(MainActivity));
           
            //Create the notification
            Notification.Builder builder = new Notification.Builder(this);
          
            
            builder.SetSmallIcon(Android.Resource.Drawable.SymActionEmail);
            builder.SetContentTitle(title);
            builder.SetContentText(desc);
           
            builder.SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate);
            builder.SetAutoCancel(true);
            builder.SetPriority(4);

            Notification.BigTextStyle bigTextStyle = new Notification.BigTextStyle();
            bigTextStyle = bigTextStyle.BigText(desc);

            builder.SetStyle(bigTextStyle);


            // builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, PendingIntentFlags.OneShot));

            builder.SetContentIntent(PendingIntent.GetActivity(this, 0, new Intent(), 0));

       

            var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);
            notification = builder.Build();

            //Auto-cancel will remove the notification once the user touches it
           // notification.Flags = NotificationFlags.AutoCancel;




            //Set the notification info
            //we use the pending intent, passing our ui intent over, which will get called
            //when the notification is tapped.

            //notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, PendingIntentFlags.OneShot));


            NotificationManager notificationManager1 = (NotificationManager)GetSystemService(Context.NotificationService);
            //Show the notification

            // notificationManager.Notify(0, notification);
            notificationManager.Notify(nr_not, notification);
           
}

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Verbose(AzurePushBroadcastReceiver.TAG, "GCM Unregistered: " + registrationId);

            createNotification("GCM Unregistered...", "The device has been unregistered!");
        }

        protected override bool OnRecoverableError(Context context, string errorId)
        {
            Log.Warn(AzurePushBroadcastReceiver.TAG, "Recoverable Error: " + errorId);

            return base.OnRecoverableError(context, errorId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error(AzurePushBroadcastReceiver.TAG, "GCM Error: " + errorId);
        }
    }
}
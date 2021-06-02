using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using MounterApp.Helpers;
using MounterApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MounterApp.Droid {
    [Service(Enabled = false)]
    public class NotificationService : Service {
        private Handler handler;
        private Action runnable;
        private bool isStarted;
        private int DELAY_BETWEEN_LOG_MESSAGES = 100000;
        private int NOTIFICATION_SERVICE_ID = 1001;
        private int NOTIFICATION_AlARM_ID = 1002;
        private string NOTIFICATION_CHANNEL_ID = "1003";
        private string NOTIFICATION_CHANNEL_NAME = "MyChannel";
        public override void OnCreate() {
            base.OnCreate();

            handler = new Handler();

            //here is what you want to do always, i just want to push a notification every 5 seconds here
            runnable = new Action(() => {
                if (isStarted) {
                    DispatchNotificationThatAlarmIsGenerated("I'm running");
                    handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);
                }
            });
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId) {
            if (isStarted) {
                // service is already started
            }
            else {
                CreateNotificationChannel();
                DispatchNotificationThatServiceIsRunning();

                handler.PostDelayed(runnable, DELAY_BETWEEN_LOG_MESSAGES);
                isStarted = true;
            }
            return StartCommandResult.Sticky;
        }

        public override void OnTaskRemoved(Intent rootIntent) {
            //base.OnTaskRemoved(rootIntent);
        }

        public override IBinder OnBind(Intent intent) {
            // Return null because this is a pure started service. A hybrid service would return a binder that would
            // allow access to the GetFormattedStamp() method.
            return null;
        }

        public override void OnDestroy() {
            // Stop the handler.
            handler.RemoveCallbacks(runnable);

            // Remove the notification from the status bar.
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.Cancel(NOTIFICATION_SERVICE_ID);

            isStarted = false;
            base.OnDestroy();
        }

        private void CreateNotificationChannel() {
            //Notification Channel
            NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME, NotificationImportance.Max);
            notificationChannel.EnableLights(true);
            notificationChannel.EnableVibration(true);
            notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });


            NotificationManager notificationManager = (NotificationManager)this.GetSystemService(Context.NotificationService);
            notificationManager.CreateNotificationChannel(notificationChannel);
        }

        //start a foreground notification to keep alive 
        private void DispatchNotificationThatServiceIsRunning() {
            NotificationCompat.Builder builder = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                   .SetDefaults((int)NotificationDefaults.All)
                   .SetSmallIcon(Resource.Drawable.icon)
                   //.SetVibrate(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 })
                   //.SetVibrate(new long[0])
                   //.SetSound(null)
                   //.SetChannelId(NOTIFICATION_CHANNEL_ID)
                   //.SetPriority((int)NotificationPriority.Default)
                   .SetAutoCancel(true)
                   .SetContentTitle("MounterApp")
                   .SetContentText("Фоновое отслеживание новых заявок включено");
            //.SetVisibility((int)NotificationVisibility.Public)
            //.SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
            //.SetOngoing(false);

            //Notification notification = new Notification(Resource.Drawable.icon, "Фоновое отслеживание новых заявок включено",1);

            NotificationManagerCompat notificationManager = NotificationManagerCompat.From(this);
            StartForeground(NOTIFICATION_SERVICE_ID, builder.Build());
            //StartForeground(NOTIFICATION_SERVICE_ID, notification);
        }
        public string NormalizePhone(string phone) {
            string ret = null;
            if (string.IsNullOrEmpty(phone)) {
                return null;
            }

            char[] _phone_chars = phone.ToCharArray();
            foreach (char c in _phone_chars) {
                if (char.IsDigit(c)) {
                    ret += c.ToString();
                }
                else {
                    continue;
                }
            }
            return ret;
        }

        private List<NewServiceorderExtensionBase_ex> _OldServiceOrders = new List<NewServiceorderExtensionBase_ex>();
        public List<NewServiceorderExtensionBase_ex> OldServiceOrders {
            get => _OldServiceOrders;
            set {
                _OldServiceOrders = value;
            }
        }
        public NewServiceorderExtensionBase_ex CompareObject(NewServiceorderExtensionBase_ex _old, NewServiceorderExtensionBase_ex _new) {
            NewServiceorderExtensionBase_ex comparator = null;
            if (_old == null || _new == null) {
                return null;
            }

            foreach (var item in _old.GetType().GetProperties().Where(x => x.Name.Equals("NewCategory") || x.Name.Equals("NewNumber") || x.Name.Equals("NewName"))) {
                object oldValue = _old.GetType().GetProperty(item.Name).GetValue(_old);
                object newValue = _new.GetType().GetProperty(item.Name).GetValue(_new);
                if (oldValue != null) {
                    if (oldValue.Equals(newValue)) {
                        continue;
                    }
                    else if (_old.GetType().GetProperty(item.Name).GetValue(_old) != null && _new.GetType().GetProperty(item.Name).GetValue(_new) != null) {
                        if (!_old.GetType().GetProperty(item.Name).GetValue(_old).Equals(_new.GetType().GetProperty(item.Name).GetValue(_new))) {
                            comparator = _new;
                        }
                        else {
                            continue;
                        }
                    }
                }
            }
            return comparator;
        }




        //every 5 seconds push a notificaition
        private async void DispatchNotificationThatAlarmIsGenerated(string message) {
            string PhoneNumber = null;
            //string Phone = null;
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("Phone")) {
                PhoneNumber = Xamarin.Forms.Application.Current.Properties["Phone"] as string;
            }

            string Phone = NormalizePhone(PhoneNumber);
            List<NewServiceorderExtensionBase_ex> compr = new List<NewServiceorderExtensionBase_ex>();
            List<NewServicemanExtensionBase> Servicemans = await ClientHttp.Get<List<NewServicemanExtensionBase>>("/api/NewServicemanExtensionBases/phone?phone=" + Phone);

            List<NewServiceorderExtensionBase_ex> _serviceorders =
                await ClientHttp.Get<List<NewServiceorderExtensionBase_ex>>("/api/NewServiceorderExtensionBases/ServiceOrderByUserNew?usr_ID=" + Servicemans.FirstOrDefault().NewServicemanId + "&date=" + DateTime.Now.Date);
            //надо прописать сравнение моделей.
            //количество элементов равно - проверяем, есть ли что новое
            //if (OldServiceOrders.Count == _serviceorders.Count)
            //    if (OldServiceOrders.Count > 0 && _serviceorders.Count > 0)
            //        foreach (var _old in OldServiceOrders)
            //            foreach (var _new in _serviceorders) {
            //                var c = CompareObject(_old, _new);
            //                if (c != null)
            //                    compr.Add(c);
            //            }
            if (OldServiceOrders.Count < _serviceorders.Count) {
                var exp = _serviceorders.Except(OldServiceOrders).ToList();
                foreach (var item in exp) {
                    compr.Add(item);
                }
            }
            if (OldServiceOrders.Count == _serviceorders.Count) {
                foreach (var item in OldServiceOrders.Where(x => x.IsShowed == false).ToList()) {
                    compr.Add(item);
                }
            }
            //if (OldServiceOrders.Count == 0 && _serviceorders.Count > 0)
            //    return;



            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.Immutable);

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);

            foreach (var item in compr.Where(x => x.IsShowed == false && !x.NewIncome.HasValue).ToList()) {
                var rand = new Random();
                int alarm_ID = rand.Next(1, 1000000);

                Notification.Builder notificationBuilder = new Notification.Builder(this, NOTIFICATION_CHANNEL_ID)
                    .SetSmallIcon(Resource.Drawable.icon)
                    //.SetContentTitle(DateTime.Now.ToString())
                    .SetContentTitle("Новая заявка")
                    .SetContentText(string.Format(@"№:{0} - {1} {2}", item.NewNumber.HasValue ? item.NewNumber : 0, string.IsNullOrEmpty(item.NewObjName) ? "" : item.NewObjName, string.IsNullOrEmpty(item.NewAddress) ? "" : item.NewAddress))
                    .SetAutoCancel(true)
                    .SetContentIntent(pendingIntent);

                notificationManager.Notify(alarm_ID, notificationBuilder.Build());
            }
            OldServiceOrders = _serviceorders;
            foreach (var item in OldServiceOrders) {
                item.IsShowed = true;
            }

            compr.Clear();
        }
    }
}
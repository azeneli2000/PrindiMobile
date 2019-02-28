using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Util;
using System;
using Gcm.Client;
using System.Globalization;
using Android.Views;
using AndroidHUD;
using Android.Text;
using Android.Text.Style;
using Android.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App8
{
    [Activity(Label = "InfoNotat", MainLauncher = true, Icon = "@drawable/icon1")]
    public class MainActivity : Activity
    {
        public static MainActivity instance;

        private void RegisterWithGCM()
        {
            // Check to ensure everything's set up right
            GcmClient.CheckDevice(this);
            GcmClient.CheckManifest(this);

            // Register for push notifications
            Log.Info("MainActivity", "Registering...");
            PushHandlerService.Context = this;

            GcmClient.Register(this, Constants.SenderID);

        }



        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

          //  koverto("TandTiaPsTdsfgTn");
           
            //vendos perdoruesin qe gjendet tek settings
            EditText em_perdoruesi = FindViewById<EditText>(Resource.Id.perdoruesi_edit);
            EditText pass_perdoruesi = FindViewById<EditText>(Resource.Id.pass_edit);
            em_perdoruesi.Text = Settings.GeneralSettings;
            pass_perdoruesi.Text = Settings.GeneralSettingsPass;
            
            //settings
            ImageButton settings = FindViewById<ImageButton>(Resource.Id.imageButtonpass);
            settings.Click += settings_Click;
            settings.Touch += settings_Touch;
            //login
            Button hyr = FindViewById<Button>(Resource.Id.button1);
            hyr.Click += Hyr_Click;//subscribe eventin click te butonit me funksionin hyr_click
        }

        private void settings_Touch(object sender, View.TouchEventArgs e)
        {
            ImageButton settings = FindViewById<ImageButton>(Resource.Id.imageButtonpass);
            if (e.Event.Action == MotionEventActions.Down)
            {
                settings.SetImageResource(Resource.Drawable.settingsicon);
            }
            else if (e.Event.Action == MotionEventActions.Up)
            {
                settings.SetImageResource(Resource.Drawable.settingsicon1);
            }
            e.Handled = false;
        }

        private async void settings_Click(object sender, EventArgs e)
        {
           

            if (!Internet.internetConnectionCheck(this))
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(5));
                return;

            }
            EditText em_perd = FindViewById<EditText>(Resource.Id.perdoruesi_edit);
            EditText pass = FindViewById<EditText>(Resource.Id.pass_edit);


            //kontrollon nqs egziston useri
            var caller_user_check = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/UserCheck/?perdoruesi=" + em_perd.Text);
            AndHUD.Shared.Show(this, "Prisni...", 50, MaskType.Clear);
            Task<List<string>> task1 = new Task<List<string>>(caller_user_check.User_check);
            task1.Start();
            List<String> perd = await task1;
            if (perd.Count == 0)
            {
                AndHUD.Shared.Dismiss();
                AndHUD.Shared.ShowErrorWithStatus(this, "Ky perdorues nuk egziston !", MaskType.Clear, TimeSpan.FromSeconds(2));

                em_perd.Text = "";
                pass.Text = "";

                return;
            }
            user p = new user();

            //kontrollon passwordin
            var caller_user_pass = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/User/?user=" + em_perd.Text + "&pass=" + pass.Text);
            AndHUD.Shared.Show(this, "Prisni...", 100, MaskType.Clear);
            Task<user> task2 = new Task<user>(() => caller_user_pass.GetUserData());

            task2.Start();
            p = await task2;
            //   p = caller_user_pass.GetUserData();
            if (p.pass == null)
            {
                AndHUD.Shared.Dismiss();
                AndHUD.Shared.ShowErrorWithStatus(this, "Fjalekalimi nuk eshte i sakte !", MaskType.Clear, TimeSpan.FromSeconds(2));

                pass.Text = "";

                return;
            }
            //kontrollon ditet          
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            string datastring = p.data_reg;
            DateTime dateValue;
            DateTime d = DateTime.Now;


            if (DateTime.TryParseExact(datastring, formats,
                                       new CultureInfo("en-US"),
                                       DateTimeStyles.None,
                                       out dateValue))
                d = dateValue.AddDays(Convert.ToInt32(p.dite_falas) + Convert.ToInt32(p.dite_paguar));
            else
            {
                AndHUD.Shared.Dismiss();
            }

            if (d <= DateTime.Now)

            {
                // shko tek faqja e abonimit intent i ri 
                AndHUD.Shared.ShowErrorWithStatus(this, "Perdoruesi me kete emer i ka mbaruar abonimi.Pe ta riaktivizuar konataktoni me shkollen.Per me shume info klikoni ne www.shkollajopublikenr1.com", MaskType.Clear, TimeSpan.FromSeconds(5));
                pass.Text = "";
                //AndHUD.Shared.Dismiss();
               return;
            }
            AndHUD.Shared.Dismiss();
            //kalon te logini
            temdata.username = em_perd.Text.Trim().ToLower();

            var intent = new Intent(this, typeof(SettingsActivity));
            StartActivity(intent);
            Finish();

        }

        private async void Hyr_Click(object sender, EventArgs e)
        {
            //kontrollon internetin
            if (!Internet.internetConnectionCheck(this))
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(5));
                return;

            }
            EditText em_perd = FindViewById<EditText>(Resource.Id.perdoruesi_edit);
            EditText pass = FindViewById<EditText>(Resource.Id.pass_edit);


            //kontrollon nqs egziston useri
            var caller_user_check = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/UserCheck/?perdoruesi=" + em_perd.Text);
            AndHUD.Shared.Show(this, "Prisni...", 50, MaskType.Clear);
            Task<List<string>> task1 = new Task<List<string>>(caller_user_check.User_check);
            task1.Start();
            List<String> perd = await task1;
            if (perd.Count == 0)
            {
                AndHUD.Shared.Dismiss();
                AndHUD.Shared.ShowErrorWithStatus(this, "Ky perdorues nuk egziston !", MaskType.Clear, TimeSpan.FromSeconds(2));

                em_perd.Text = "";
                pass.Text = "";
               
                return;
            }
            user p = new user();

            //kontrollon passwordin
            var caller_user_pass = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/User/?user=" + em_perd.Text + "&pass=" + pass.Text);
            AndHUD.Shared.Show(this, "Prisni...", 100, MaskType.Clear);
            Task<user> task2 = new Task<user>(() => caller_user_pass.GetUserData());
           
            task2.Start();
            p = await task2;
         //   p = caller_user_pass.GetUserData();
            if (p.pass == null)
            {
                AndHUD.Shared.Dismiss();
                AndHUD.Shared.ShowErrorWithStatus(this, "Fjalekalimi nuk eshte i sakte !", MaskType.Clear, TimeSpan.FromSeconds(2));

                pass.Text = "";
               
                return;
            }
            //kontrollon ditet          
            string[] formats = {"M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm", "M/d/yyyy h:mm",
                         "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"};
            string datastring = p.data_reg;
            DateTime dateValue;
            DateTime d = DateTime.Now;


            if (DateTime.TryParseExact(datastring, formats,
                                       new CultureInfo("en-US"),
                                       DateTimeStyles.None,
                                       out dateValue))
                d = dateValue.AddDays(Convert.ToInt32(p.dite_falas) + Convert.ToInt32(p.dite_paguar));
            else
            {
                AndHUD.Shared.Dismiss();
            }

            if (d <= DateTime.Now)

            {
                // shko tek faqja e abonimit intent i ri 
                AndHUD.Shared.ShowErrorWithStatus(this, "Perdoruesi me kete emer i ka mbaruar abonimi.Pe ta riaktivizuar konataktoni me shkollen.Per me shume info klikoni ne www.shkollajopublikenr1.com", MaskType.Clear, TimeSpan.FromSeconds(2));
                pass.Text = "";
             
                return;
            }
            AndHUD.Shared.Dismiss();
            //kalon te dhenat ne temdata   
            temdata.username = em_perd.Text.Trim().ToLower();
            temdata.password = pass.Text;
            temdata.emri_nx = p.emri + " " + p.mbiemri;
            temdata.nr_amza = p.amza;
            temdata.klasa = p.klasa;
            temdata.indeksi = p.indeksi;
            temdata.id_shkolla = p.id_shkolla;

            //nqs nuk eshte bere ben regjistrimin e aparatit per notification
            if (Settings.GeneralSettings == "" || Settings.GeneralSettings != em_perd.Text.Trim().ToLower())
            {
                PushHandlerService.perdoruesi = temdata.username;
                RegisterWithGCM();
                Settings.GeneralSettings = temdata.username;
            }
            //nqs ka ndryshuar passwordi vendos passwordin e ri
            if (Settings.GeneralSettingsPass == "" || Settings.GeneralSettingsPass != pass.Text.Trim())
            {
              
                Settings.GeneralSettingsPass = temdata.password;
            }

            var intent = new Intent(this, typeof(NotatActivity));
            StartActivity(intent);
            Finish();
        }
        public void koverto (string origjinali)
        {
            var textView = FindViewById<TextView>(Resource.Id.textView3);
            var span = new SpannableStringBuilder(origjinali);
            //gjen T 

            var foundIndexesT = new List<int>();
            var foundIndexesP = new List<int>();
            var foundIndexesS = new List<int>();
            var foundIndexesTotal = new List<int>();

            //gjen indekset e T,P,S
            for (int i = 0; i < origjinali.Length; i++)
            {
                if (origjinali[i] == 'T')
                    foundIndexesT.Add(i);
                if (origjinali[i] == 'P')
                    foundIndexesP.Add(i);
                if (origjinali[i] == 'S')
                    foundIndexesS.Add(i);

            }

            foreach (int ind in foundIndexesT)
            {
                span.SetSpan(new ForegroundColorSpan(Color.Red), ind + 1, ind + 2, 0);
                                
            }
            foreach (int ind in foundIndexesP)
            {
                span.SetSpan(new ForegroundColorSpan(Color.Blue), ind + 1, ind + 2, 0);

            }

            foreach (int ind in foundIndexesS)
            {
                span.SetSpan(new ForegroundColorSpan(Color.Green), ind + 1, ind + 2, 0);

            }
            foundIndexesTotal.AddRange(foundIndexesT);
            foundIndexesTotal.AddRange(foundIndexesP);
            foundIndexesTotal.AddRange(foundIndexesS);
            int fi = 0;
            foundIndexesTotal.Sort();
            foreach (int index in foundIndexesTotal)
            {
               
                span.Delete(index-fi, index-fi+1);
                fi++;
            }
            textView.SetText(span, TextView.BufferType.Spannable);
        }
        
    }
}


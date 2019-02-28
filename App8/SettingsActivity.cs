using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using System.Globalization;
using System.Threading.Tasks;

namespace App8
{
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Rregullime);
            Button hyr1 = FindViewById<Button>(Resource.Id.buttonndrysho);
            hyr1.Click += Hyr_Click;
        }

            private  void Hyr_Click(object sender, EventArgs e)
        {
            //kontrollon internetin

            if (!Internet.internetConnectionCheck(this))
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Nuk jeni te lidhur me internet !", MaskType.Clear, TimeSpan.FromSeconds(2));
                return;

            }
            EditText p1 = FindViewById<EditText>(Resource.Id.perdoruesi_edit);
            EditText p2 = FindViewById<EditText>(Resource.Id.pass_edit);
            if (p1.Text == p2.Text)
            {
                var caller = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/User/?em_perd=" + temdata.username + "&password=" + p1.Text.Trim());
                caller.update();
                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            }
            else
            {
                AndHUD.Shared.ShowErrorWithStatus(this, "Fjalekalimet duhet te jene te njejta !", MaskType.Clear, TimeSpan.FromSeconds(2));
                return;
            }
                AndHUD.Shared.Dismiss();
            Settings.GeneralSettingsPass = p1.Text;
            Finish();

        }

    }
    
}
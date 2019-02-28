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
using Gcm.Client;
using Android.Util;
using System.IO;

namespace App8
{
    [Activity(Label = "Perdorues i RI")]
    public class PerdoruesiRIActivity : Activity
    {
        private List<Nxenesit_klasa> nx_list = new List<Nxenesit_klasa>();
        private List<string> emri = new List<string>();
        private List<string> amza = new List<string>();
        private List<shkollat> shkollat_s = new List<shkollat>();
        private List<string> emri_shkolla = new List<string>();
        private List<string> vendodhja_shkolla = new List<string>();
        private List<string> id_shkolla = new List<string>();


        //shikon nqs ka perd me kete emer
        public bool user_check (string p)
        {
           
            var caller6 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/UserCheck/?perdoruesi="+p);
            if (caller6.User_check().Count > 0)
                return true;
            else
                return false;
        }
        //shikon nqs nxenesi ka nje perdorues te mepareshem
        public bool nxenesi_check(string amza, string cikli,string id_shkolla)
        {
            var caller6 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/UserAmza/?nr_amza=" + amza+"&cikli="+ cikli + "&id_shkolla=" + id_shkolla);
            if (caller6.User_check().Count > 0)
                return true;
            else
                return false;

        }

        public static DateTime GetDate()
        {
            var result = DateTime.MinValue;
            // Initialize the list of NIST time servers
            // http://tf.nist.gov/tf-cgi/servers.cgi
            string[] servers = new string[] {
"nist1-ny.ustiming.org",
"nist1-nj.ustiming.org",
"nist1-pa.ustiming.org",
"time-a.nist.gov",
"time-b.nist.gov",
"nist1.aol-va.symmetricom.com",
"nist1.columbiacountyga.gov",
"nist1-chi.ustiming.org",
"nist.expertsmi.com",
"nist.netservicesgroup.com"
};

            // Try 5 servers in random order to spread the load
            Random rnd = new Random();
            foreach (string server in servers.OrderBy(s => rnd.NextDouble()).Take(5))
            {
                try
                {
                    // Connect to the server (at port 13) and get the response
                    string serverResponse = string.Empty;
                    using (var reader = new StreamReader(new System.Net.Sockets.TcpClient(server, 13).GetStream()))
                    {
                        serverResponse = reader.ReadToEnd();
                    }

                    // If a response was received
                    if (!string.IsNullOrEmpty(serverResponse))
                    {
                        // Split the response string ("55596 11-02-14 13:54:11 00 0 0 478.1 UTC(NIST) *")
                        string[] tokens = serverResponse.Split(' ');

                        // Check the number of tokens
                        if (tokens.Length >= 6)
                        {
                            // Check the health status
                            string health = tokens[5];
                            if (health == "0")
                            {
                                // Get date and time parts from the server response
                                string[] dateParts = tokens[1].Split('-');
                                string[] timeParts = tokens[2].Split(':');

                                // Create a DateTime instance
                                DateTime utcDateTime = new DateTime(
                                    Convert.ToInt32(dateParts[0]) + 2000,
                                    Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]),
                                    Convert.ToInt32(timeParts[0]), Convert.ToInt32(timeParts[1]),
                                    Convert.ToInt32(timeParts[2]));

                                // Convert received (UTC) DateTime value to the local timezone
                                result = utcDateTime.ToLocalTime();

                                return result;
                                // Response successfully received; exit the loop

                            }
                        }

                    }

                }
                catch
                {
                    // Ignore exception and try the next server
                }
            }
            return result;
        }
        public string gjej_vitin()
        {
            if (DateTime.Now.Month >= 7)
                return (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            else
                return
                 (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        }
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            temp_perdoruesi.klasa = "1";
            temp_perdoruesi.indeksi = "A";

             List<string> klasa_source = new List<string>();
            List<string> indeksi_source = new List<string>();
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Perdorues);

            //shkolla
            var caller5 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Shkolla");
            shkollat_s = caller5.GetShkollat();
            emri.Clear();
            amza.Clear();
            foreach (shkollat sh in shkollat_s)
            {
                emri_shkolla.Add(sh.emri_shkolla);
                id_shkolla.Add(sh.id_shkolla);
                vendodhja_shkolla.Add(sh.vendodhja);
            }
            ArrayAdapter<string> adapter_sh = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, emri_shkolla);
            Spinner sp_shkolla = FindViewById<Spinner>(Resource.Id.shkolla);
            sp_shkolla.Adapter = adapter_sh;
            sp_shkolla.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(shkolla_itemselected);

            //klasa
            Spinner sp_klasa = FindViewById<Spinner>(Resource.Id.klasa);
           // klasa_source.Add("Klasa");
            for (int i = 1; i <= 12; i++)
            {
                klasa_source.Add(i.ToString());

            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, klasa_source);
            sp_klasa.Adapter = adapter;
            sp_klasa.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(klasa_itemselected);
            //indeksi
           // indeksi_source.Add("Indeksi");
            indeksi_source.Add("A");
            indeksi_source.Add("B");
            indeksi_source.Add("C");
            indeksi_source.Add("D");
            indeksi_source.Add("E");
            indeksi_source.Add("F");
            indeksi_source.Add("G");
            ArrayAdapter<string> adapter1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, indeksi_source);
            Spinner sp_indeksi = FindViewById<Spinner>(Resource.Id.indeksi);
            sp_indeksi.Adapter = adapter1;
            sp_indeksi.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(indeksi_itemselected);
            //nxenesit
            var caller4 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Nxenesi/?klasa=" + temp_perdoruesi.klasa + "&indeksi=" + temp_perdoruesi.indeksi + "&id_shkolla=" + temp_perdoruesi.id_shkolla + "&viti_sh=" + gjej_vitin() + "&cikli=" + temp_perdoruesi.cikli);
            nx_list = caller4.GetNxenesi();
            emri.Clear();
            amza.Clear();
            foreach (Nxenesit_klasa nx in nx_list)
            {
                emri.Add(nx.emri);
                amza.Add(nx.nr_amza);
            }
            ArrayAdapter<string> adapter_lv1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, emri);
            Spinner sp_emri = FindViewById<Spinner>(Resource.Id.nxenesit);
            sp_emri.Adapter = adapter_lv1;                    
            sp_emri.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(emri_itemselected);

            ArrayAdapter<string> adapter_lv2 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, amza);

            //Button save = FindViewById<Button>(Resource.Id.ruaj);
            //save.Click += Save_Click;
            ImageButton ruaj = FindViewById<ImageButton>(Resource.Id.imageButton1);
            ruaj.Click += Ruaj_Click;

            ImageButton cancel = FindViewById<ImageButton>(Resource.Id.imageButton2);
            cancel.Click += Cancel_Click;

            ImageButton ndihme = FindViewById<ImageButton>(Resource.Id.imageButton3);
            ndihme.Click += Ndihme_Click;
        }

        private void Ndihme_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(NdihmaActivity));
            StartActivity(intent);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        private void Ruaj_Click(object sender, EventArgs e)
        {
            user u = new user();
            EditText perdoruesi = FindViewById<EditText>(Resource.Id.perd_emri);
            EditText pass = FindViewById<EditText>(Resource.Id.pass);
            EditText perserit = FindViewById<EditText>(Resource.Id.perserit_pass);
            EditText email = FindViewById<EditText>(Resource.Id.email);
            u.amza = temp_perdoruesi.amza_nx;
            u.cikli = temp_perdoruesi.cikli;
            u.id_shkolla = temp_perdoruesi.id_shkolla;
            u.key = "123";
            u.emri = "Andi";
            u.mbiemri = "Zeneli";
            u.perd = perdoruesi.Text;

            u.pass = pass.Text;
            u.email = email.Text;
            u.klasa = temp_perdoruesi.klasa;
            u.indeksi = temp_perdoruesi.indeksi;
            u.data_reg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            u.dite_falas = "1";
            u.dite_paguar = "0";
            if (user_check(perdoruesi.Text))
            {
                //msg qe egziston perd
                new AlertDialog.Builder(this)
                .SetMessage("Perdoruesi me kete emer egziston ! Zgjidhni nje emer perdorusi tjeter")
                .SetTitle("Gabim")
                .Show();
                perdoruesi.Text = "";
                return;
            }
            if (nxenesi_check(u.amza, u.cikli, u.id_shkolla))
            {
                //msg per nxenesin 
                new AlertDialog.Builder(this)
                .SetMessage("Nxenesi me kete emer e ka nje perdorues ! Per me shume informacione kontaktoni me shkollen perkatese")
               .SetTitle("Gabim")
                //.SetIcon(Resource.Drawable.)

                .Show();

                return;
            }
            if (perdoruesi.Text == "" || pass.Text == "" || perserit.Text == "" || email.Text == "")
            {
                new AlertDialog.Builder(this)
               .SetMessage("Plotesoni te gjitha fushat e kerkuara !")
              .SetTitle("Gabim")
               //.SetIcon(Resource.Drawable.)

               .Show();
                return;
            }

            if (pass.Text != perserit.Text)
            {
                new AlertDialog.Builder(this)
                              .SetMessage("Fjalekalimi dhe perseritja e tij nuk jane te njejta ! Provoni perseri")
                              .SetTitle("Gabim")
                              .Show();

                pass.Text = "";
                perserit.Text = "";
                return;
            }
            var caller6 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/User/");
            caller6.Create(u);
            PushHandlerService.perdoruesi = u.perd;
            RegisterWithGCM();
            var intent = new Intent(this, typeof(MainActivity));
            StartActivity(intent);
            Finish();
        }

        //private void Save_Click(object sender, EventArgs e)
        //{
        //    user u = new user();
        //    EditText perdoruesi = FindViewById<EditText>(Resource.Id.perd_emri);
        //    EditText pass = FindViewById<EditText>(Resource.Id.pass);
        //    EditText perserit = FindViewById<EditText>(Resource.Id.perserit_pass);
        //    EditText email = FindViewById<EditText>(Resource.Id.email);
        //    u.amza = temp_perdoruesi.amza_nx;
        //    u.cikli = temp_perdoruesi.cikli;
        //    u.id_shkolla = temp_perdoruesi.id_shkolla;
        //    u.key = "123";
        //    u.emri = "Andi";
        //    u.mbiemri = "Zeneli";
        //    u.perd = perdoruesi.Text;
           
        //    u.pass = pass.Text;
        //    u.email = email.Text;
        //    u.klasa = temp_perdoruesi.klasa;
        //    u.indeksi = temp_perdoruesi.indeksi;
        //    u.data_reg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //    u.dite_falas = "1";
        //    u.dite_paguar = "0";
        //    if (user_check(perdoruesi.Text))
        //    {
        //        //msg qe egziston perd
        //        new AlertDialog.Builder(this)
        //        .SetMessage("Perdoruesi me kete emer egziston ! Zgjidhni nje emer perdorusi tjeter")
        //        .SetTitle("Gabim")
        //        .Show();
        //        perdoruesi.Text = "";
        //        return;
        //    }
        //    if(nxenesi_check(u.amza,u.cikli,u.id_shkolla))
        //    {
        //        //msg per nxenesin 
        //        new AlertDialog.Builder(this)
        //        .SetMessage("Nxenesi me kete emer e ka nje perdorues ! Per me shume informacione kontaktoni me shkollen perkatese")
        //       .SetTitle("Gabim")
        //       //.SetIcon(Resource.Drawable.)
               
        //        .Show();
            
        //        return;
        //    }
        //    if (perdoruesi.Text==""||pass.Text==""||perserit.Text==""||email.Text=="")
        //    {
        //        new AlertDialog.Builder(this)
        //       .SetMessage("Plotesoni te gjitha fushat e kerkuara !")
        //      .SetTitle("Gabim")
        //       //.SetIcon(Resource.Drawable.)

        //       .Show();
        //        return;
        //    }

        //    if(pass.Text!=perserit.Text)
        //    {
        //        new AlertDialog.Builder(this)
        //                      .SetMessage("Fjalekalimi dhe perseritja e tij nuk jane te njejta ! Provoni perseri")
        //                      .SetTitle("Gabim")
        //                      .Show();

        //        pass.Text = "";
        //        perserit.Text = "";
        //        return;
        //    }
        //    var caller6 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/User/");
        //    caller6.Create(u);
        //    PushHandlerService.perdoruesi = u.perd;
        //    RegisterWithGCM();
        //    var intent = new Intent(this, typeof(MainActivity));
        //    StartActivity(intent);
        //    Finish();
        //}

        private void shkolla_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner shkolla = (Spinner)sender;
            temp_perdoruesi.id_shkolla = id_shkolla[e.Position].ToString();
        }

        //klasa
        private void klasa_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner klasa = (Spinner)sender;
            temp_perdoruesi.klasa = klasa.GetItemAtPosition(e.Position).ToString();
            if (Convert.ToInt16(temp_perdoruesi.klasa) < 10)
            {
                temp_perdoruesi.cikli = "True";
            }
            else
            {
                temp_perdoruesi.cikli = "False";
            }
            //gjen nxenesit e klases
            var caller4 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Nxenesi/?klasa=" + temp_perdoruesi.klasa + "&indeksi=" + temp_perdoruesi.indeksi + "&id_shkolla=" + temp_perdoruesi.id_shkolla + "&viti_sh=" + gjej_vitin() + "&cikli=" + temp_perdoruesi.cikli);
            nx_list = caller4.GetNxenesi();
            emri.Clear();
            amza.Clear();
            foreach (Nxenesit_klasa nx in nx_list)
            {
                emri.Add(nx.emri);
                amza.Add(nx.nr_amza);
            }
            ArrayAdapter<string> adapter_lv1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, emri);
            Spinner sp_emri = FindViewById<Spinner>(Resource.Id.nxenesit);
            sp_emri.Adapter = adapter_lv1;
            string cik = temp_perdoruesi.cikli;
        }


        //indeksi
        private void indeksi_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner indeksi = (Spinner)sender;
            temp_perdoruesi.indeksi = indeksi.GetItemAtPosition(e.Position).ToString();

            //gjen nxenesit e klases
            var caller4 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Nxenesi/?klasa=" + temp_perdoruesi.klasa + "&indeksi=" + temp_perdoruesi.indeksi + "&id_shkolla=" + temp_perdoruesi.id_shkolla + "&viti_sh=" + gjej_vitin() + "&cikli=" + temp_perdoruesi.cikli);
            nx_list = caller4.GetNxenesi();
            emri.Clear();
            amza.Clear();
            foreach (Nxenesit_klasa nx in nx_list)
            {
                emri.Add(nx.emri);
                amza.Add(nx.nr_amza);
            }
            ArrayAdapter<string> adapter_lv1 = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, emri);
            Spinner sp_emri = FindViewById<Spinner>(Resource.Id.nxenesit);
            sp_emri.Adapter = adapter_lv1;
        }

        //nxenesit
        private void emri_itemselected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner nxenesit = (Spinner)sender;
            temp_perdoruesi.amza_nx = amza[e.Position].ToString();
            temp_perdoruesi.emri_nx = emri[e.Position].ToString();
            string amzastr = temp_perdoruesi.amza_nx;
            string cik = temp_perdoruesi.cikli;
        }

        

      
    }
}
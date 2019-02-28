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
using App8.Resources;

namespace App8
{
    [Activity(Label = "InfoNotat")]
    public class NotatActivity : Activity
    {

        public string gjej_vitin()
        {
            if (DateTime.Now.Month >= 7)
                return (DateTime.Now.Year).ToString() + "-" + (DateTime.Now.Year + 1).ToString();
            else
                return
                 (DateTime.Now.Year - 1).ToString() + "-" + (DateTime.Now.Year).ToString();
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Notat);
            TextView tw1 = (TextView)FindViewById(Resource.Id.textViewemri);
            tw1.Text = temdata.emri_nx;
            var lista = FindViewById<ListView>(Resource.Id.listView1);
            List  <Nxenesi_Nota> list_data = new List<Nxenesi_Nota>();          
            var caller4 = new RestSharpCaller("http://restapishkolla20171002033922.azurewebsites.net/api/Prindi/?klasa="+temdata.klasa+"&indeksi="+temdata.indeksi+"&id_shkolla="+ temdata.id_shkolla+"&viti_sh="+gjej_vitin()+"&nr_amza="+temdata.nr_amza);
            list_data = caller4.GetPrindi();
            var adapter = new CustomAdapter(this, list_data);
            lista.Adapter = adapter;
        }
    }
}
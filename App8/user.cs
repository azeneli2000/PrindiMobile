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
using Firebase.Iid;
using Android.Util;
using Newtonsoft.Json.Linq;

namespace App8
{

   
  

  public  class user
    {
        public string id_shkolla { get; set; }

        public string cikli { get; set; }
        public string amza { get; set; }
        public string key { get; set; }
        public string perd { get; set; }
        public string pass { get; set; }
        public string emri { get; set; }
        public string mbiemri { get; set; }
        public string email { get; set; }
        public string klasa { get; set; }
        public string indeksi { get; set; }
        public string data_reg { get; set; }
        public string dite_falas { get; set; }
        public string dite_paguar { get; set; }

    }
}
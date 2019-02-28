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
using Java.Lang;
using Android.Text.Style;
using Android.Text;
using Android.Graphics;

namespace App8.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txtEmri { get; set; }
        public TextView TxtNotat { get; set; }
    }
    public class CustomAdapter : BaseAdapter
    {
        private Activity activity;
        private List<Nxenesi_Nota> notat;
        public CustomAdapter(Activity activity,List<Nxenesi_Nota> nota)
        {
            this.activity = activity;
            this.notat = nota;
        }


      
        public override int Count
        {
            get
            {
                return notat.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return Convert.ToInt64( notat[position].nr_amza);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.listviewlayout, parent, false);
                var Emritxt = view.FindViewById<TextView>(Resource.Id.textView1);
            var Notatxt = view.FindViewById<TextView>(Resource.Id.textView2);
            Emritxt.Text = notat[position].emri;

            //Notatxt.Text = notat[position].notat;
         
            
            //per te ndryshuar ngjyren e notave
            string origjinali = notat[position].notat;

            var span = new SpannableStringBuilder(origjinali);
            //gjen T 

            var foundIndexesT = new List<int>();
            var foundIndexesP = new List<int>();
            //var foundIndexesS = new List<int>();
            var foundIndexesTotal = new List<int>();

            //gjen indekset e T,P,S
            for (int i = 0; i < origjinali.Length; i++)
            {
                if (origjinali[i] == 'T')
                    foundIndexesT.Add(i);
                if (origjinali[i] == 'P')
                    foundIndexesP.Add(i);
                //if (origjinali[i] == 'S')
                //    foundIndexesS.Add(i);

            }

            foreach (int ind in foundIndexesT)
            {
                span.SetSpan(new ForegroundColorSpan(Color.Red), ind + 1, ind + 2, 0);

                if (origjinali[ind+2] == '0')
                    span.SetSpan(new ForegroundColorSpan(Color.Red), ind + 2, ind + 3, 0);

            }
            foreach (int ind in foundIndexesP)
            {
                span.SetSpan(new ForegroundColorSpan(Color.Blue), ind + 1, ind + 2, 0);
                if (origjinali[ind + 2] == '0')
                    span.SetSpan(new ForegroundColorSpan(Color.Red), ind + 2, ind + 3, 0);

            }

          
            foundIndexesTotal.AddRange(foundIndexesT);
            foundIndexesTotal.AddRange(foundIndexesP);
            //foundIndexesTotal.AddRange(foundIndexesS);
            int fi = 0;
            foundIndexesTotal.Sort();
            foreach (int index in foundIndexesTotal)
            {

                span.Delete(index - fi, index - fi + 1);
                fi++;
            }
           Notatxt.SetText(span, TextView.BufferType.Spannable);

            return view;
        }
    }
}
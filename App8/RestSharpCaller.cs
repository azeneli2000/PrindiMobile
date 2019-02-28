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
using RestSharp;

namespace App8
{
    class RestSharpCaller
    {
        public RestClient client;
        public RestSharpCaller(string baseUrl)
        {
            client = new RestClient(baseUrl);
        }

        public List<Nxenesi_Nota> GetPrindi()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<Nxenesi_Nota>>(request);
            return response.Data;
        }
        public List<Nxenesit_klasa> GetNxenesi()
        {

         
            var request = new RestRequest("", Method.GET);
                        
            var response = client.Execute<List<Nxenesit_klasa>>(request);

            return response.Data;
        }

        public List<shkollat> GetShkollat()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<shkollat>>(request);
            return response.Data;
        }

        //public string GetIdPerd()
        //{
        //    var request = new RestRequest("", Method.GET);
        //    var response = client.Execute<string>(request);
        //    return response.Data;
        //}
        public user GetUserData()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<user>(request);
            return response.Data;
        }

        public List<string> User_check()
        {
            var request = new RestRequest("", Method.GET);
            var response = client.Execute<List<string>>(request);
            return response.Data;
        }


        public void Create(user u)
        {
            var request = new RestRequest("", Method.POST);
            request.AddJsonBody(u);
            client.Execute(request);
        }

        public void update()
        {
            var request = new RestRequest("", Method.PUT);
      
            client.Execute(request);
        }

    }
}
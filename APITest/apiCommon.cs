using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace APITest
{
    public class apiCommon
    {
        public const string oKey = "API-44HR3B2JDP9QQTR";
        public const string oAPIUrl = "http://alltheclouds.com.au/api/";
        public Product oProduct = new Product();
        public List<Product> oProductList = new List<Product>();
        public List<Rate> oRateList = new List<Rate>();
        public Order oOrder = new Order();

        #region Method
        public static string GetResponseFromApiServerByPostRequest(string _url, string _parameter)
        {
            string oResponseFromServer = string.Empty;

            try
            {
                WebRequest oRequest = WebRequest.Create(_url);
                oRequest.Headers.Add("api-key", oKey);
                oRequest.Method = "POST";
                oRequest.Proxy = null;
                string oPostData = _parameter;
                byte[] oByteArray = Encoding.UTF8.GetBytes(oPostData);
                oRequest.ContentType = "application/json";
                oRequest.ContentLength = oByteArray.Length;
                Stream oDataStream = oRequest.GetRequestStream();
                oDataStream.Write(oByteArray, 0, oByteArray.Length);
                oDataStream.Close();
                WebResponse oResponse = oRequest.GetResponse();
                oDataStream = oResponse.GetResponseStream();
                StreamReader oReader = new StreamReader(oDataStream);
                oResponseFromServer = oReader.ReadToEnd();
                oReader.Close();
                oDataStream.Close();
                oResponse.Close();
            }
            catch (WebException ex)
            {
                using (var oStream = ex.Response.GetResponseStream())
                using (var oReader = new StreamReader(oStream))
                {
                    oResponseFromServer = oReader.ReadToEnd();
                }
            }

            return oResponseFromServer;
        }

        //GET request by query string
        public static string GetResponseFromApiServerByGetRequest(string _url, string _parameter)
        {
            string oResponseFromServer = string.Empty;

            try
            {
                HttpWebRequest oRequest = WebRequest.Create(_url + _parameter) as HttpWebRequest;
                oRequest.Headers.Add("api-key", oKey);
                oRequest.Method = "GET";
                oRequest.ContentType = "application/jsonw";
                HttpWebResponse oResponse = oRequest.GetResponse() as HttpWebResponse;
                Stream oResponseStream = oResponse.GetResponseStream();
                StreamReader oReader = new StreamReader(oResponseStream, Encoding.UTF8);
                oResponseFromServer = oReader.ReadToEnd();
                oReader.Close();
                oResponseStream.Close();
                oResponse.Close();
            }
            catch (Exception e)
            {
                oResponseFromServer = e.Message;
            }

            return oResponseFromServer;
        }
        #endregion


        public class Product{
            public string productId { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public decimal? unitPrice { get; set; }
            public int? maximumQuantity { get; set; }
            private void Clear()
            {
                this.productId = string.Empty;
                this.name = string.Empty;
                this.description = string.Empty;
                this.unitPrice = 0;
                this.maximumQuantity = 0;
            }
        }
        public class Rate
        {
            public string sourceCurrency { get; set; }
            public string targetCurrency { get; set; }
            public decimal? rate { get; set; }
            private void Clear()
            {
                this.sourceCurrency = string.Empty;
                this.targetCurrency = string.Empty;
                this.rate = 0;
            }
        }
        public class Lineitems
        {
            public string productId { get; set; }
            public int? quantity { get; set; }
            private void Clear()
            {
                this.productId = string.Empty;
                this.quantity = 0;
            }
        }
        public class Order
        {
            public string customerName { get; set; }
            public string customerEmail { get; set; }
            public List<Lineitems> lineItems { get; set; }
            private void Clear()
            {
                this.customerName = string.Empty;
                this.customerEmail = string.Empty;
                this.lineItems = new List<Lineitems>();
            }
            public string GetJasonFormat()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}

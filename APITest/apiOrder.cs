using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace APITest
{
    public class apiOrder
    {
        public const string oUrl = "Orders";
        public apiOrder()
        {
            this.Clear();
        }

        public string Message { get; set; }
        public bool Status { get; set; }
        public string Value { get; set; }

        #region method
        public void Call(string _data)
        {
            string oFinalUrl = apiCommon.oAPIUrl + oUrl;
            string oResponse = apiCommon.GetResponseFromApiServerByPostRequest(oFinalUrl, _data);

            try
            {
                if (!string.IsNullOrEmpty(oResponse))
                {
                    this.Status = true;
                    this.Value = oResponse;
                }
                else
                {
                    this.Status = false;
                }
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
            }
        }
        #endregion
        private void Clear()
        {
            this.Message = string.Empty;
            this.Status = false;
            this.Value = string.Empty;
        }
    }
}

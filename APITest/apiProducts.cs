﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace APITest
{
    public class apiProducts
    {
        public const string oUrl = "Products";
        public apiProducts()
        {
            this.Clear();
        }

        public string Message { get; set; }
        public bool Status { get; set; }
        public List<apiCommon.Product> Value { get; set; }
        public object ValueInObject { get; set; }
        #region method
        public void Call()
        {
            string oFinalUrl = apiCommon.oAPIUrl + oUrl;
            string oResponse = apiCommon.GetResponseFromApiServerByGetRequest(oFinalUrl, string.Empty);

            try
            {
                JArray oVal = JArray.Parse(oResponse);

                if (oVal.Count > 0)
                {
                    this.Status = true;
                    this.Value = oVal.ToObject<List<apiCommon.Product>>();
                    this.ValueInObject = oVal;
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
            this.Value = new List<apiCommon.Product>();
            this.ValueInObject = null;
        }

    }
}

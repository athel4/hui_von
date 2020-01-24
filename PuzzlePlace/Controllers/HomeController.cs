using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PuzzlePlace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies["username"] == null)
                return RedirectToAction("Login");
            else if (string.IsNullOrEmpty(Request.Cookies["username"].ToString()))
                return RedirectToAction("Login");
            return View();
        }

        public ActionResult GetListing()
        {
            if (Request.Cookies["username"] == null)
                return RedirectToAction("Login");
            else if (string.IsNullOrEmpty(Request.Cookies["username"].ToString()))
                return RedirectToAction("Login");

            Models.ViewModel oVM = new Models.ViewModel();
            oVM.oApiFixRate.Call();
            oVM.oApiProduct.Call();
            return View(oVM);
        }
        public ActionResult CheckMyOrder()
        {
            if (Request.Cookies["username"] == null)
                return RedirectToAction("Login");
            else if (string.IsNullOrEmpty(Request.Cookies["username"].ToString()))
                return RedirectToAction("Login");

            return View();
        }
        public ActionResult Login()
        {
            if (Request.Cookies["username"] != null)
                return RedirectToAction("Index");
            return View();
        }

        #region HttpPost
        [HttpPost]
        public ActionResult LoginNow(FormCollection _form)
        {
            JObject oJson = new JObject(
                                    new JProperty("status", 0),
                                    new JProperty("message", string.Empty),
                                    new JProperty("username", string.Empty),
                                    new JProperty("email", string.Empty),
                                    new JProperty("pwd", string.Empty)
                                    );
            if (!string.IsNullOrEmpty(_form.Get("username")) && !string.IsNullOrEmpty(_form.Get("pwd")))
            {
                oJson["status"] = 1;
                oJson["username"] = _form.Get("username");
                oJson["email"] = _form.Get("email");
                oJson["pwd"] = _form.Get("pwd");
            }
            return Content(JsonConvert.SerializeObject(oJson, Formatting.Indented));
        }
        [HttpPost]
        public ActionResult ChangeMyOrder(FormCollection _form)
        {
            JObject oJson = new JObject(
                                    new JProperty("status", 0),
                                    new JProperty("message", string.Empty));

            return Content(JsonConvert.SerializeObject(this, Formatting.Indented));
        }
        [HttpPost]
        public ActionResult PlaceOrder(FormCollection _form)
        {
            JObject oJson = new JObject(
                                    new JProperty("status", 0),
                                    new JProperty("message", string.Empty));

            APITest.apiCommon.Order oData = new APITest.apiCommon.Order();
            Models.ViewModel oVM = new Models.ViewModel();
            oVM.oApiFixRate.Call();
            oVM.oApiProduct.Call();
            var oItem = oVM.oApiProduct.Value.Where(x => x.productId == _form.Get("reqid")).FirstOrDefault();
            if (oItem.maximumQuantity != null)
            {
                if (oItem.maximumQuantity < Convert.ToInt32(_form.Get("reqqty")))
                    _form.Set("reqqty", oItem.maximumQuantity.ToString());

                APITest.apiCommon.Lineitems oLineItems = new APITest.apiCommon.Lineitems();
                List<APITest.apiCommon.Lineitems> oList = new List<APITest.apiCommon.Lineitems>();
                oLineItems.productId = _form.Get("reqid").ToString();
                oLineItems.quantity = Convert.ToInt32(_form.Get("reqqty"));
                APITest.apiCommon.Order oOrder = new APITest.apiCommon.Order();
                oList.Add(oLineItems);
                oOrder.customerName = Request.Cookies["username"].ToString();
                oOrder.customerEmail = Request.Cookies["email"].ToString();
                oOrder.lineItems = oList;

                oVM.oApiOrder.Call(JsonConvert.SerializeObject(oOrder, Formatting.Indented));
                if (!string.IsNullOrEmpty(oVM.oApiOrder.Value))
                    oJson["message"] = oVM.oApiOrder.Value;
                else
                    oJson["message"] = "No Result";

            }
            else if (oItem.maximumQuantity != null && Convert.ToInt32(_form.Get("reqqty")) > 0)
            {
                oJson["message"] = "Unable To Get The Remaining Quantity From Supplier";
            }
            return Content(JsonConvert.SerializeObject(oJson, Formatting.Indented));
        }
        #endregion

        
    }
}
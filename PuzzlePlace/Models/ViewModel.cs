using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PuzzlePlace.Models
{
    public class ViewModel
    {
        public APITest.apiFxRates oApiFixRate = new APITest.apiFxRates();
        public APITest.apiOrder oApiOrder = new APITest.apiOrder();
        public APITest.apiProducts oApiProduct = new APITest.apiProducts();
    }
}
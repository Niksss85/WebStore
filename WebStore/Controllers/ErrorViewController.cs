﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using WebStore.Models;

namespace WebStore.Controllers
{
    public class ErrorViewController : Controller
    {


        public IActionResult Index() => View();



    }
}

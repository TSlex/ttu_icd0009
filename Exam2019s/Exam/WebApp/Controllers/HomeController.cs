﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Models;

namespace WebApp.Controllers
{
    /// <inheritdoc />
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <inheritdoc />
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Not found page
        /// </summary>
        /// <returns></returns>
        public  IActionResult PageNotFound()
        {
            return View("404");
        }
    }
}
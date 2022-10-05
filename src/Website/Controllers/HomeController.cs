﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LiteDB;
using Website.Models;

namespace EventualConsistencyDemo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            throw new System.NotImplementedException();
        }
    }
}

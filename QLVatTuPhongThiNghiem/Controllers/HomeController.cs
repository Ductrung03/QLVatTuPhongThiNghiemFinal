﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLVatTuPhongThiNghiem.Models;
using System.Diagnostics;

namespace QLVatTuPhongThiNghiem.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
    }


}
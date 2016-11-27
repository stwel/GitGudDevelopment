﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GitGud.Models;
using Microsoft.Extensions.Configuration;
using GitGud.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using GitGud.Controllers.Web;
using GitGud.Services;

namespace GitGud.Controllers.Web
{
    public class AppController : Controller
    {

        private IConfigurationRoot _config;
        private IHostingEnvironment _environment;
        private IUploadService _uploadService;
        private IGitGudRepository _repository;

        public AppController(IConfigurationRoot config, IGitGudRepository repository, IHostingEnvironment environment, IUploadService uploadService)
        {
            _config = config;
            _repository = repository;
            _environment = environment;
            _uploadService = uploadService;
        }

        public IActionResult Index()
        {
            var tags = _repository.GetTopTags();
            var categories = _repository.GetAllCategories();
            ViewData["categories"] = categories;

            return View(tags);
        }

        public IActionResult Browse()
        {
            var data = _repository.GetAllSongs();//TODO: Change this

            return View(data);
        }

        public IActionResult Charts()
        {
            return View();
        }

        public IActionResult HotTracks()
        {
            return View();
        }

        //[Authorize]//ToDo - find out why redirect path is not working 
        [HttpGet]
        public IActionResult Upload()
        {
            //until then - instead of annotations we just check for user login
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "User");
            }

            var categories = _repository.GetAllCategories();
            ViewData["categories"] = categories;

            return View();
        }

        [HttpPost][Authorize]
        public IActionResult Upload(UploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                _uploadService.UploadSong(
                    model.MusicFile,
                    model.SongName, 
                    model.Artist,
                    model.Category,
                    model.Tags.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList(),
                    this.User.Identity.Name);

                
                ModelState.Clear();
                ViewBag.InputFields = "Song send/uploaded";
            }
            return RedirectToAction("Upload");
        }

        public IActionResult Search()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult SearchByCategory(string strCategoryId)
        {
            int categoryId = int.Parse(strCategoryId);
            var songsByCategory = _repository.SearchSongsByCategory(categoryId);

            string categoryName = _repository.SearchCategoryById(categoryId).Name;
            ViewData["categoryName"] = categoryName;

            return View(songsByCategory);
        }
    }
}

﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Models
{
    public class Song
    {
        public string Name { get; set; }
        public string ArtistName { get; set; }

        public DateTime DateUploaded
        {
            get
            {
                return DateTime.Now;
            }
        }
        public string UploaderName { get; set; }
        public string fileAdress { get; set; }
        public ICollection<Tag> Tags { get; set; }

    }
}
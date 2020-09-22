﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Database.DbModels
{
    public class Skill
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
    }
}
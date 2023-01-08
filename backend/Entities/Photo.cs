﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DatingApp.DTOs;

namespace DatingApp.Entities
{
    [Table("Photos")]
    public class Photo
    {
        private PhotoDto dto;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }


    }
}
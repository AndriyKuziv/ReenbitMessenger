﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReenbitMessenger.Infrastructure.Models.DTO
{
    public class Token
    {
        public string tokenType { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}

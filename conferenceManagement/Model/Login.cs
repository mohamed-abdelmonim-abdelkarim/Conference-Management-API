﻿using MediatR;

namespace conferenceManagement.Model
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}

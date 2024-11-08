using MediatR;

namespace conferenceManagement.Model
{
    public class RegisterCommand : IRequest<string>
    { 
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}

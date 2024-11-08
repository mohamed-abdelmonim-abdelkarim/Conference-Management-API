using MediatR;

namespace conferenceManagement.Commends
{
    public class Commends
    {
        public record RegisterUserCommand(string Username, string Email, string Password) : IRequest<bool>;
        
        public record LoginUserCommand(string Username, string Password) : IRequest<string?>;

    }
}

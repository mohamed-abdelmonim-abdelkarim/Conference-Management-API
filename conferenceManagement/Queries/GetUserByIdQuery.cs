using MediatR;
using Microsoft.AspNetCore.Identity;

namespace conferenceManagement.Queries
{
    public class Queries
    {
        public record GetUserByIdQuery(string UserId) : IRequest<IdentityUser>;

    }
}

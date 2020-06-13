using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateFriendshipInvitation(Person sender, Person receiver);
        Task<IEnumerable<Friendship>> GetRequests(Person user);
        Task<IEnumerable<Person>> GetAllUsers();
        Task<IEnumerable<User>> GetFriends(User user);
        Task<Friendship> GetRequestWhere(string user, string inviteSender);
        Task<IdentityResult> UpdateUser(Person user);
        Task<IdentityResult> DeleteFriendship(Friendship friendship);

    }
}

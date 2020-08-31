using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IUserRepository: IGenericRepository<Person>
    {
        Task<IEnumerable<Friendship>> GetRequests(Person user);
        Task<IEnumerable<Friendship>> GetInvitations(Person user);
        Task<IEnumerable<Person>> GetAllUsers();
        Task<IEnumerable<Friendship>> GetFriends(User user);
        Task<Friendship> GetSpecificRequest(string user, string inviteSender);
        void DeleteFriendship(Friendship friendship);
    }
}

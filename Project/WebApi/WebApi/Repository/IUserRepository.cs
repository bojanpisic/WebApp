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
        //Task<IdentityResult> CreateFriendshipInvitation(Person sender, Person receiver);
        Task<IEnumerable<Friendship>> GetRequests(Person user);
        Task<IEnumerable<Person>> GetAllUsers();
        Task<IEnumerable<User>> GetFriends(User user);
        Task<Friendship> GetRequestWhere(string user, string inviteSender);
        //void UpdateUser(Person user);
        void DeleteFriendship(Friendship friendship);

        Task<CarRent> GetRent(int id);
        Task<IEnumerable<CarRent>> GetRents(User user);
    }
}

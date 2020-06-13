using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly UserManager<Person> userManager;
        public UserRepository(DataContext _context, UserManager<Person> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public async Task<IdentityResult> CreateFriendshipInvitation(Person sender, Person receiver)
        {
            using (var transaction = context.Database.BeginTransactionAsync())
            {
                try
                {
                    //flight.Stops = new List<FlightDestination>
                    //{
                    //    new FlightDestination{
                    //        Flight = flight,
                    //        Destination = stop
                    //    }
                    //};
                    User s = (User)sender;
                    User r = (User)receiver;
                    var f = new Friendship() {Rejacted = false, Accepted = false, User1 = s, User2 = r };
                    s.FriendshipInvitations.Add(f);
                    r.FriendshipRequests.Add(f);

                    await this.UpdateUser(s);
                    await this.UpdateUser(r);
                    await transaction.Result.CommitAsync();
                    return IdentityResult.Success;
                }
                catch (Exception)
                {
                    await transaction.Result.RollbackAsync();

                    return IdentityResult.Failed();
                }
            }
        }

        public async Task<IdentityResult> DeleteFriendship(Friendship friendship)
        {
            context.Friendships.Remove(friendship);
            var res = await context.SaveChangesAsync();

            if (res > 0)
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(new IdentityError() { Description = "Delete error" });
        }

        public async Task<IEnumerable<Person>> GetAllUsers()
        {
            return await context.Persons.ToListAsync();
        }

        public Task<IEnumerable<User>> GetFriends(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Friendship>> GetRequests(Person user)
        {
            return await context.Friendships.Include(f=>f.User1).Where(f => f.User2Id == user.Id).ToListAsync();
        }

        public async Task<Friendship> GetRequestWhere(string user, string inviteSender)
        {
            return await context.Friendships.Include(f => f.User1).FirstOrDefaultAsync(f => f.User1Id == inviteSender);
        }

        public async Task<IdentityResult> UpdateUser(Person user) 
        {
            context.Entry(user).State = EntityState.Modified;
            var res = await context.SaveChangesAsync();
            if (res > 0)
            {
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError() { Description = "Update error" });
        }
    }
}

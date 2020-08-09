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
    public class UserRepository : GenericRepository<Person>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }

        //public async Task<IdentityResult> CreateFriendshipInvitation(Person sender, Person receiver)
        //{
        //    //using (var transaction = context.Database.BeginTransactionAsync())
        //    //{
        //        try
        //        {
        //            //flight.Stops = new List<FlightDestination>
        //            //{
        //            //    new FlightDestination{
        //            //        Flight = flight,
        //            //        Destination = stop
        //            //    }
        //            //};
        //            User s = (User)sender;
        //            User r = (User)receiver;
        //            var f = new Friendship() {Rejacted = false, Accepted = false, User1 = s, User2 = r };

        //            s.FriendshipInvitations.Add(f);
        //            r.FriendshipRequests.Add(f);

        //            this.UpdateUser(s);
        //            this.UpdateUser(r);
        //        //await transaction.Result.CommitAsync();
        //            unitOfWork.Commit();
        //            return IdentityResult.Success;
        //        }
        //        catch (Exception)
        //        {
        //        //await transaction.Result.RollbackAsync();
                    
        //            return IdentityResult.Failed();
        //        }
        //    //}
        //}

        public void DeleteFriendship(Friendship friendship)
        {
            context.Friendships.Remove(friendship);
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

        public async Task<CarRent> GetRent(int id)
        {
            return await context.CarRents.Include(c => c.RentedCar).FirstOrDefaultAsync(c =>c.CarRentId == id);
        }

        public async Task<IEnumerable<CarRent>> GetRents(User user)
        {
            return await context.CarRents
                .Include(c => c.RentedCar)
                .ThenInclude(car => car.Branch)
                .ThenInclude(car => car.RentACarService)
                .Where(c => c.User == user).ToListAsync();
        }
    }
}

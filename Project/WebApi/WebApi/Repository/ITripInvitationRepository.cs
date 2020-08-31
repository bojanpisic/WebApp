using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ITripInvitationRepository: IGenericRepository<Invitation>
    {
        Task<IEnumerable<Invitation>> GetTripInvitations(User user);
        Task<Invitation> GetTripInvitationById(int id);
    }
}

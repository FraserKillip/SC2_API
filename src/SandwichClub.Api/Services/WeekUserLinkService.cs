using System.Collections.Generic;
using System.Threading.Tasks;
using SandwichClub.Api.Repositories;
using SandwichClub.Api.Repositories.Models;

namespace SandwichClub.Api.Services
{
    public class WeekUserLinkService : BaseService<WeekUserLinkId, WeekUserLink, IWeekUserLinkRepository>, IWeekUserLinkService
    {
        public WeekUserLinkService(IWeekUserLinkRepository weekUserLinkRepository) : base(weekUserLinkRepository)
        {
        }

        public Task<IEnumerable<WeekUserLink>> GetByWeekIdAsync(int weekId)
        {
            return Repository.GetByWeekIdAsync(weekId);
        }

        public override Task<WeekUserLink> InsertAsync(WeekUserLink link)
        {
            return InsertOrUpdateAsync(link);
        }

        public override Task UpdateAsync(WeekUserLink link)
        {
            return InsertOrUpdateAsync(link);
        }

        public async Task<WeekUserLink> InsertOrUpdateAsync(WeekUserLink link)
        {
            var id = new WeekUserLinkId {UserId = link.UserId, WeekId = link.WeekId};
            var existingLink = await Repository.GetByIdAsync(id);
            var exists = existingLink != null;

            // Check if we should delete this link
            var delete = link.Paid <= 0 && link.Slices <= 0;

            if (exists && delete)
                await Repository.DeleteAsync(id);
            else if (exists)
                await Repository.UpdateAsync(link);
            else if (!delete)
                link = await Repository.InsertAsync(link);

            return link;
        }
    }
}

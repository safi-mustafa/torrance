using DataLibrary;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Services.CommonServices.PossibleApproverService
{
    public class PossibleApproverService : IPossibleApproverService
    {
        private readonly ToranceContext _db;
        public PossibleApproverService(ToranceContext db)
        {
            _db = db;
        }
        public async Task<string> GetPossibleApprovers(long? unitId, long? departmentId)
        {
            try
            {
                var approverFullNames = await
                               _db.ApproverAssociations
                               .Include(x => x.Approver)
                               .Include(x => x.Department)
                               .Include(x => x.Unit)
                               .Where(x => x.UnitId == unitId && x.DepartmentId == departmentId && x.Approver.ActiveStatus == Enums.ActiveStatus.Active)
                               .Select(x => x.Approver.FullName)
                               .Distinct()
                               .ToListAsync();

                return string.Join(", ", approverFullNames);
            }
            catch (Exception)
            {

                return "";
            }

        }
    }
}

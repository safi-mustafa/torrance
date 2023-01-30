using System;
using Centangle.Common.ResponseHelpers.Models;
using Enums;
using Models.Common.Interfaces;
using Repositories.Interfaces;
using ViewModels.Shared;

namespace Repositories.Services.CommonServices.ApprovalService.Interface
{
    public interface IApprovalService : IBaseSearch
    {
        Task<IRepositoryResponse> Delete(long id, LogType type);
    }
}


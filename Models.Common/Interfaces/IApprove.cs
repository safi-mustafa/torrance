using System;
using Enums;

namespace Models.Common.Interfaces
{
    public interface IApprove
    {
        Status Status { get; set; }
        bool IsArchived { get; set; }
    }
}


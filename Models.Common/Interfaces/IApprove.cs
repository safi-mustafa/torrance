using System;
using Enums;

namespace Models.Common.Interfaces
{
    public interface IApprove
    {
        string? Comment { get; set; }
        Status Status { get; set; }
        bool IsArchived { get; set; }
    }
}


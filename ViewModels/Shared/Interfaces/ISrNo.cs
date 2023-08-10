using System;
using ViewModels.Common.Unit;

namespace Models.Common.Interfaces
{
    public interface ISrNo
    {
        long SrNo { get; set; }
        UnitBriefViewModel Unit { get; set; }
    }
}


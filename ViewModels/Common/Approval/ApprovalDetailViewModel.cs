using System;
using Enums;
using Models.Common.Interfaces;
using ViewModels.Shared;

namespace ViewModels.Common
{
    public class ApprovalDetailViewModel : IBaseCrudViewModel, IIdentitifier
    {
        public ApprovalDetailViewModel()
        {
        }
        public long Id { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Type { get; set; }
        public string Requester { get; set; }
        public string Department { get; set; }
        public string Contractor { get; set; }
        public string Unit { get; set; }
        public string TWR { get; set; }
        public ActiveStatus ActiveStatus { get; set; }
    }
}


using AutoMapper;
using Centangle.Common.ResponseHelpers;
using Centangle.Common.ResponseHelpers.Models;
using DataLibrary;
using Enums;
using Helpers.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Common.Interfaces;
using Models.OverrideLogs;
using Models.TimeOnTools;
using Pagination;
using Repositories.Common;
using Repositories.Shared;
using Repositories.Shared.NotificationServices;
using Repositories.Shared.UserInfoServices;
using Select2.Model;
using System.Data;
using System.Linq.Expressions;
using ViewModels;
using ViewModels.Notification;
using ViewModels.OverrideLogs.ORLog;
using ViewModels.Shared;
using ViewModels.TimeOnTools.TOTLog;

namespace Repositories.Services.TimeOnToolServices.TOTLogService
{
    public class TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel> : ApproveBaseService<TOTLog, CreateViewModel, UpdateViewModel, DetailViewModel>, ITOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>
        where DetailViewModel : class, IBaseCrudViewModel, new()
        where CreateViewModel : class, IBaseCrudViewModel, new()
        where UpdateViewModel : class, IBaseCrudViewModel, IIdentitifier, new()
    {
        private readonly ToranceContext _db;
        private readonly ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> _logger;
        private readonly IMapper _mapper;
        private readonly IRepositoryResponse _response;
        private readonly IUserInfoService _userInfoService;
        private readonly INotificationService _notificationService;

        public TOTLogService(ToranceContext db, ILogger<TOTLogService<CreateViewModel, UpdateViewModel, DetailViewModel>> logger, IMapper mapper, IRepositoryResponse response, IUserInfoService userInfoService, INotificationService notificationService) : base(db, logger, mapper, response, notificationService)
        {
            _db = db;
            _logger = logger;
            _mapper = mapper;
            _response = response;
            _userInfoService = userInfoService;
            _notificationService = notificationService;
        }

        public override Expression<Func<TOTLog, bool>> SetQueryFilter(IBaseSearchModel filters)
        {
            var searchFilters = filters as TOTLogSearchViewModel;
            searchFilters.OrderByColumn = "Status";
            var status = (Status?)((int?)searchFilters.Status);
            var loggedInUserRole = _userInfoService.LoggedInUserRole() ?? _userInfoService.LoggedInWebUserRole();
            var loggedInUserId = loggedInUserRole == "Employee" ? _userInfoService.LoggedInEmployeeId() : _userInfoService.LoggedInUserId();
            var parsedLoggedInId = long.Parse(loggedInUserId);

            return x =>
                            (string.IsNullOrEmpty(searchFilters.Search.value) || x.EquipmentNo.ToString().Contains(searchFilters.Search.value.ToLower()))
                            &&
                            (searchFilters.EquipmentNo == null || x.EquipmentNo == searchFilters.EquipmentNo)
                            //&&
                            //(searchFilters.Contractor.Id == 0 || x.Contractor.Id == searchFilters.Contractor.Id)
                            //&&
                            //(searchFilters.Department.Id == 0 || x.Department.Id == searchFilters.Department.Id)
                            &&
                            (searchFilters.Unit.Id == 0 || searchFilters.Unit.Id == null || x.Unit.Id == searchFilters.Unit.Id)
                            &&
                            (
                                (loggedInUserRole == "SuperAdmin")
                                ||
                                (loggedInUserRole == "Approver" && x.ApproverId == parsedLoggedInId)
                                ||
                                (loggedInUserRole == "Employee" && x.EmployeeId == parsedLoggedInId)
                            )
                            &&
                            (status == null || status == x.Status)
                            &&
                            (searchFilters.StatusNot == null || searchFilters.StatusNot != x.Status)
                            &&
                            x.IsDeleted == false
            ;
        }

        public override async Task<IRepositoryResponse> GetById(long id)
        {
            try
            {
                var dbModel = await _db.TOTLogs
                    .Include(x => x.Unit)
                    .Include(x => x.Department)
                    .Include(x => x.Company)
                    .Include(x => x.ReworkDelay)
                    .Include(x => x.ShiftDelay)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Approver)
                    .Include(x => x.Foreman)
                    .Include(x => x.Employee)
                    .Include(x => x.PermittingIssue)
                    .Include(x => x.DelayType)
                    .Include(x => x.ReasonForRequest)
                    .Where(x => x.Id == id && x.IsDeleted == false).IgnoreQueryFilters().FirstOrDefaultAsync();
                if (dbModel != null)
                {
                    var mappedModel = _mapper.Map<TOTLogDetailViewModel>(dbModel);
                    mappedModel.TWRModel = new TWRViewModel(mappedModel.Twr);
                    var response = new RepositoryResponseWithModel<TOTLogDetailViewModel> { ReturnModel = mappedModel };
                    return response;
                }
                _logger.LogWarning($"No record found for id:{id} for TOTLog");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetById() for TOTLog threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        public override async Task<IRepositoryResponse> GetAll<M>(IBaseSearchModel search)
        {
            try
            {
                var filters = SetQueryFilter(search);
                var resultQuery = _db.Set<TOTLog>()
                    .Include(x => x.Unit)
                    .Include(x => x.DelayType)
                    .Include(x => x.Shift)
                    .Include(x => x.PermitType)
                    .Include(x => x.Employee)
                    .Include(x => x.Company)
                    .Include(x => x.ReasonForRequest)
                    .Where(filters).IgnoreQueryFilters();
                var query = resultQuery.ToQueryString();
                var result = await resultQuery.Paginate(search);
                if (result != null)
                {
                    var paginatedResult = new PaginatedResultModel<M>();
                    paginatedResult.Items = _mapper.Map<List<M>>(result.Items.ToList());
                    paginatedResult._meta = result._meta;
                    paginatedResult._links = result._links;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<M>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(TOTLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TOTLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public async override Task<IRepositoryResponse> Create(CreateViewModel model)
        {
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var mappedModel = _mapper.Map<TOTLog>(model);
                    await SetRequesterId(mappedModel);
                    await _db.Set<TOTLog>().AddAsync(mappedModel);
                    var result = await _db.SaveChangesAsync() > 0;
                    await _notificationService.AddNotificationAsync(new NotificationViewModel(mappedModel.Id, typeof(OverrideLog), mappedModel.ApproverId?.ToString() ?? "", "A new TOT Log has been created", NotificationType.Push, NotificationEntityType.Logs));
                    await transaction.CommitAsync();
                    var response = new RepositoryResponseWithModel<long> { ReturnModel = mappedModel.Id };
                    return response;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, $"Exception thrown in Create method of {typeof(TOTLog).FullName}");
                    return Response.BadRequestResponse(_response);
                }
            }
        }

        public async override Task<IRepositoryResponse> Update(UpdateViewModel model)
        {
            try
            {
                var updateModel = model as BaseUpdateVM;
                if (updateModel != null)
                {
                    var record = await _db.Set<TOTLog>().FindAsync(updateModel?.Id);
                    if (record != null)
                    {
                        var dbModel = _mapper.Map(model, record);
                        await SetRequesterId(dbModel);
                        await _db.SaveChangesAsync();
                        var response = new RepositoryResponseWithModel<long> { ReturnModel = record.Id };
                        return response;
                    }
                    _logger.LogWarning($"Record for id: {updateModel?.Id} not found in {typeof(TOTLog).FullName} in Update()");
                }
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Update() for {typeof(TOTLog).FullName} threw the following exception");
                return Response.BadRequestResponse(_response);
            }
        }

        private async Task SetRequesterId(TOTLog mappedModel)
        {
            var role = _userInfoService.LoggedInUserRole();
            if (role == "Employee")
            {
                mappedModel.EmployeeId = long.Parse(_userInfoService.LoggedInEmployeeId());
            }
            mappedModel.CompanyId = await _db.Employees.Where(x => x.Id == mappedModel.EmployeeId).Select(x => x.CompanyId).FirstOrDefaultAsync();
        }

        public async Task<IRepositoryResponse> GetTWRNumericValues<BaseBriefVM>(IBaseSearchModel search)
        {
            try
            {
                var list = GetTWRNumericList();

              
                if (list != null && list.Count > 0)
                {
                    var paginatedResult = new PaginatedResultModel<Select2ViewModel>();
                    paginatedResult.Items = list;
                    paginatedResult._meta = new();
                    paginatedResult._links = new() ;
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(TOTLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TOTLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public async Task<IRepositoryResponse> GetTWRAphabeticValues<M>(IBaseSearchModel search)
        {
            try
            {
                var list = GetTWRAlphabeticList();



                if (list != null && list.Count > 0)
                {
                    var paginatedResult = new PaginatedResultModel<Select2ViewModel>();
                    paginatedResult.Items = list;
                    paginatedResult._meta = new();
                    paginatedResult._links = new();
                    var response = new RepositoryResponseWithModel<PaginatedResultModel<Select2ViewModel>> { ReturnModel = paginatedResult };
                    return response;
                }
                _logger.LogWarning($"No record found for {typeof(TOTLog).FullName} in GetAll()");
                return Response.NotFoundResponse(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetAll() method for {typeof(TOTLog).FullName} threw an exception.");
                return Response.BadRequestResponse(_response);
            }
        }

        public static List<Select2ViewModel> GetTWRNumericList() => new List<Select2ViewModel>()
        {
            new Select2ViewModel
            {
                id = "01",
                text = "01"
            },
            new Select2ViewModel
            {
                id = "02",
                text = "02"
            },
            new Select2ViewModel
            {
                id = "03",
                text = "03"
            },
            new Select2ViewModel
            {
                id = "04",
                text = "04"
            },
            new Select2ViewModel
            {
                id = "05",
                text = "05"
            },
            new Select2ViewModel
            {
                id = "06",
                text = "06"
            },
            new Select2ViewModel
            {
                id = "07",
                text = "07"
            },
            new Select2ViewModel
            {
                id = "08",
                text = "08"
            },
            new Select2ViewModel
            {
                id = "09",
                text = "09"
            },
            new Select2ViewModel
            {
                id = "10",
                text = "10"
            },
            new Select2ViewModel
            {
                id = "12",
                text = "12"
            },
            new Select2ViewModel
            {
                id = "13",
                text = "13"
            },
            new Select2ViewModel
            {
                id = "17",
                text = "17"
            },
            new Select2ViewModel
            {
                id = "19",
                text = "19"
            },
            new Select2ViewModel
            {
                id = "20",
                text = "20"
            },
            new Select2ViewModel
            {
                id = "21",
                text = "21"
            },
            new Select2ViewModel
            {
                id = "22",
                text = "22"
            },
            new Select2ViewModel
            {
                id = "24",
                text = "24"
            },
            new Select2ViewModel
            {
                id = "25",
                text = "25"
            },
            new Select2ViewModel
            {
                id = "27",
                text = "27"
            },
            new Select2ViewModel
            {
                id = "28",
                text = "28"
            },
            new Select2ViewModel
            {
                id = "29",
                text = "29"
            },
            new Select2ViewModel
            {
                id = "30",
                text = "30"
            },
            new Select2ViewModel
            {
                id = "50",
                text = "50"
            },
            new Select2ViewModel
            {
                id = "51",
                text = "51"
            },
            new Select2ViewModel
            {
                id = "52",
                text = "52"
            },
            new Select2ViewModel
            {
                id = "53",
                text = "53"
            },
            new Select2ViewModel
            {
                id = "55",
                text = "55"
            },
            new Select2ViewModel
            {
                id = "56",
                text = "56"
            },
            new Select2ViewModel
            {
                id = "64",
                text = "64"
            },
            new Select2ViewModel
            {
                id = "65",
                text = "65"
            },
            new Select2ViewModel
            {
                id = "66",
                text = "66"
            },
            new Select2ViewModel
            {
                id = "67",
                text = "67"
            },
            new Select2ViewModel
            {
                id = "68",
                text = "68"
            },
            new Select2ViewModel
            {
                id = "72",
                text = "72"
            },
            new Select2ViewModel
            {
                id = "75",
                text = "75"
            },
            new Select2ViewModel
            {
                id = "76",
                text = "76"
            },
            new Select2ViewModel
            {
                id = "80",
                text = "80"
            },
            new Select2ViewModel
            {
                id = "81",
                text = "81"
            },
            new Select2ViewModel
            {
                id = "82",
                text = "82"
            },
            new Select2ViewModel
            {
                id = "83",
                text = "83"
            },
            new Select2ViewModel
            {
                id = "85",
                text = "85"
            },
            new Select2ViewModel
            {
                id = "98",
                text = "98"
            },
            new Select2ViewModel
            {
                id = "266",
                text = "266"
            }
        };

        public static List<Select2ViewModel> GetTWRAlphabeticList() => new List<Select2ViewModel>() {
          new Select2ViewModel {
            text = "A-Analyzer",
            id = "A"
          },
          new Select2ViewModel {
            text = "B-Fin Fan",
            id = "B"
          },
          new Select2ViewModel {
            text = "C-Vessel",
            id = "C"
          },
          new Select2ViewModel {
            text = "CC-Chem Clean",
            id = "CC"
          },
          new Select2ViewModel {
            text = "D-Drum",
            id = "D"
          },
          new Select2ViewModel {
            text = "E-Exchanger",
            id = "E"
          },
          new Select2ViewModel {
            text = "Ei-Electrical",
            id = "Ei"
          },
          new Select2ViewModel {
            text = "F-Heater",
            id = "F"
          },
          new Select2ViewModel {
            text = "FC-Flow Controller",
            id = "FC"
          },
          new Select2ViewModel {
            text = "FE-Orifice Plate",
            id = "FE"
          },
          new Select2ViewModel {
            text = "FI-Flow Indicator",
            id = "FI"
          },
          new Select2ViewModel {
            text = "FO-Orifice Plate",
            id = "FO"
          },
          new Select2ViewModel {
            text = "FS-Flow Switch",
            id = "FS"
          },
          new Select2ViewModel {
            text = "FT-Flow Transmitter",
            id = "FT"
          },
          new Select2ViewModel {
            text = "FV-Control Valve",
            id = "FV"
          },
          new Select2ViewModel {
            text = "G-Pumps",
            id = "G"
          },
          new Select2ViewModel {
            text = "Gi-General Instrumentation",
            id = "Gi"
          },
          new Select2ViewModel {
            text = "HC-Control Valve",
            id = "HC"
          },
          new Select2ViewModel {
            text = "HV-Control Valve",
            id = "HV"
          },
          new Select2ViewModel {
            text = "J-Filters",
            id = "J"
          },
          new Select2ViewModel {
            text = "K-Compressor",
            id = "K"
          },
          new Select2ViewModel {
            text = "LC-Level Controller",
            id = "LC"
          },
          new Select2ViewModel {
            text = "LG-Level Glass",
            id = "LG"
          },
          new Select2ViewModel {
            text = "LI-Level Indicator",
            id = "LI"
          },
          new Select2ViewModel {
            text = "LS-Level Switch",
            id = "LS"
          },
          new Select2ViewModel {
            text = "LT-Level Transmitter",
            id = "LT"
          },
          new Select2ViewModel {
            text = "LV-Control Valve",
            id = "LV"
          },
          new Select2ViewModel {
            text = "M-Miscellaneous",
            id = "M"
          },
          new Select2ViewModel {
            text = "OR-Other Rotating",
            id = "OR"
          },
          new Select2ViewModel {
            text = "P-Piping",
            id = "P"
          },
          new Select2ViewModel {
            text = "PC-Pressure Controller",
            id = "PC"
          },
          new Select2ViewModel {
            text = "PCV-Control Valve",
            id = "PCV"
          },
          new Select2ViewModel {
            text = "PI-Pressure Indicator",
            id = "PI"
          },
          new Select2ViewModel {
            text = "PS-Pressure Switch",
            id = "PS"
          },
          new Select2ViewModel {
            text = "PT-Pressure Transmitter",
            id = "PT"
          },
          new Select2ViewModel {
            text = "PV-Control Valve",
            id = "PV"
          },
          new Select2ViewModel {
            text = "SD-Shut Down",
            id = "SD"
          },
          new Select2ViewModel {
            text = "SE-Specialty Equipment",
            id = "SE"
          },
          new Select2ViewModel {
            text = "SOL-Solenoid",
            id = "SOL"
          },
          new Select2ViewModel {
            text = "SU-Start Up",
            id = "SU"
          },
          new Select2ViewModel {
            text = "SV-Safety Valve",
            id = "SV"
          },
          new Select2ViewModel {
            text = "T-Tank",
            id = "T"
          },
          new Select2ViewModel {
            text = "TC-Temperature Controller",
            id = "TC"
          },
          new Select2ViewModel {
            text = "TCV-Control Valve",
            id = "TCV"
          },
          new Select2ViewModel {
            text = "TE-Temperature Element",
            id = "TE"
          },
          new Select2ViewModel {
            text = "TI-Temperature Indication",
            id = "TI"
          },
          new Select2ViewModel {
            text = "TS-Temperature Switch",
            id = "TS"
          },
          new Select2ViewModel {
            text = "TT-Temperature Transmitter",
            id = "TT"
          },
          new Select2ViewModel {
            text = "TV-Control Valve",
            id = "TV"
          },
          new Select2ViewModel {
            text = "V-Valve",
            id = "V"
          },
          new Select2ViewModel {
            text = "XV-Control Valve",
            id = "XV"
          }
        };

    }
}

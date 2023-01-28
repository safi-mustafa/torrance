
using DataLibrary;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.WeldingRodRecord;

namespace IdentityProvider.Seed
{
    public class SeedWorker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SeedWorker(IServiceProvider serviceProvider)
            => _serviceProvider = serviceProvider;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<ToranceContext>();
            var _userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ToranceUser>>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ToranceUser>>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            try
            {
                var alreadyCreatedRoles = context.Roles.Any();
                if (!alreadyCreatedRoles)
                {
                    List<ToranceRole> roles = new List<ToranceRole>
                {
                    new ToranceRole
                    {
                        Name = "SuperAdmin",
                        NormalizedName = "SUPERADMIN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Approver",
                        NormalizedName = "APPROVER",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Foreman",
                        NormalizedName = "FOREMAN",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    },
                    new ToranceRole
                    {
                        Name = "Employee",
                        NormalizedName = "EMPLOYEE",
                        ConcurrencyStamp = "5acd8273-22f2-487b-8971-f0208a532051",
                    }
                };

                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }
                var alreadyCreatedUsers = context.Users.Any();
                var userId = "";
                if (!alreadyCreatedUsers)
                {
                    var user = new ToranceUser();
                    await _userStore.SetUserNameAsync(user, "admin@centangle.com", CancellationToken.None);
                    user.Email = "admin@centangle.com";
                    if ((await _userManager.CreateAsync(user, "Admin@123")).Succeeded)
                    {
                        var roleCheck = "SuperAdmin";
                        if ((await _userManager.AddToRoleAsync(user, roleCheck)).Succeeded)
                        {
                            userId = await _userManager.GetUserIdAsync(user);
                        }
                    }
                }
                else
                {
                    var adminId = context.Users.Where(x => x.Email == "admin@centangle.com").Select(x => x.Id).FirstOrDefault();
                    userId = adminId.ToString();
                }
                var employees = context.Employees.Count();
                if (employees < 1)
                {
                    var empUser = new ToranceUser();
                    await _userStore.SetUserNameAsync(empUser, "nunez_adrian@yahoo.com", CancellationToken.None);
                    empUser.Email = "nunez_adrian@yahoo.com";
                    var empResult = await _userManager.CreateAsync(empUser, "5555");
                    if (empResult.Succeeded)
                    {
                        var roleCheck = "Employee";
                        if ((await _userManager.AddToRoleAsync(empUser, roleCheck)).Succeeded)
                        {
                            var empUserId = await _userManager.GetUserIdAsync(empUser);
                            if (long.Parse(empUserId) > 0)
                            {
                                context.Employees.Add(new Employee
                                {
                                    FirstName = "Nunez",
                                    LastName = "Adrian",
                                    ActiveStatus = Enums.ActiveStatus.Active,
                                    Address = "4060 Electric Avenue",
                                    City = "San Bernardino",
                                    State = "CA - California",
                                    ZipCode = "92337",
                                    Telephone = 3102561519,
                                    //ApproverId = long.Parse(userId),
                                    DateOfHire = DateTime.Now,
                                    Email = "nunez_adrian@yahoo.com",
                                    EmployeeId = "5555",
                                    TerminationDate = DateTime.Now.AddYears(3),
                                    BankAccount = null,
                                    SocialSecurity = null,
                                    DriverLicense = null,
                                    EmergencyContactName = null,
                                    EmergencyContactNumber = null,
                                    RoutingNumber = null,
                                    UserId = long.Parse(empUserId)
                                });
                                context.SaveChanges();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }
}

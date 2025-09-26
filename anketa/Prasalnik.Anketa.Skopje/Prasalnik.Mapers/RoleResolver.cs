using AutoMapper;
using Prasalnik.Domain.Enums;
using Prasalnik.Domain.Models;
using Prasalnik.ViewModels.Models;

namespace Prasalnik.Mapers
{
    public class RoleResolver : IValueResolver<UserViewModel, User, RoleEnum>
    {
        public RoleEnum Resolve(UserViewModel source, User destination, RoleEnum destMember, ResolutionContext context)
        {
            var roleRaw = source.Role?.Trim();
            if (string.IsNullOrEmpty(roleRaw))
                return RoleEnum.Employee;

            // Try parse name or number
            if (Enum.TryParse<RoleEnum>(roleRaw, ignoreCase: true, out var parsed)
                && Enum.IsDefined(typeof(RoleEnum), parsed))
            {
                return parsed;
            }

            // Optional: log unexpected value (context.Items or ILogger via DI)
            // throw new ArgumentException($"Invalid role value: {source.Role}");

            return RoleEnum.Employee;
        }
    }
}

using NewsHeadline.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Services
{
    /// <summary>
    /// User setting repository interface.
    /// Implimented in the database user setting repository
    /// </summary>
    public interface IUserSettingRepository
    {
        UserSetting Read(string userName);
        void Update(string userName, UserSetting userSetting, ICollection<string> newsCategoryNames);
    }
}

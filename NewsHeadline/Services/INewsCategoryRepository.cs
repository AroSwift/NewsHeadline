using NewsHeadline.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsHeadline.Services
{
    /// <summary>
    /// A news category repository interface.
    /// Implimented in the database news category repository
    /// </summary>
    public interface INewsCategoryRepository
    {
        void CreateNewsSource(string namem, UserSettingNewsCategory category);
        void CreateUserSettingNewsCategory(string name);
        void Delete(int id, UserSetting userSetting);
        void DeleteNewsSource(int id, UserSetting user);
        IQueryable<NewsCategory> ReadAll();
        UserSetting Read(int userSettingId);
        UserSettingNewsCategory ReadUserSettingNewsCategory(int category);
    }
}

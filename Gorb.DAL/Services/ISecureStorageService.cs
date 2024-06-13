using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gorb.DAL.Services
{
    public interface ISecureStorageService
    {
        Task SetAsync(SecureStorageKey key, string value);

        Task<string> GetAsync(SecureStorageKey key);

        Task RemoveAsync(SecureStorageKey key);

        Task RemoveAllAsync();
    }

    public enum SecureStorageKey
    {
        AccessToken,
        RefreshToken,
        UserId,
        Nickname,
    }
}

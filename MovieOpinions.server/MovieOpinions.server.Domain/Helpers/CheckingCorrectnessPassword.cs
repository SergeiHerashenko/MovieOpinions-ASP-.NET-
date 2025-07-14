using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Helpers
{
    public class CheckingCorrectnessPassword
    {
        public async Task<bool> VerifyPassword(string EnteredPassword, string PasswordSalt, string StoredHash)
        {
            // Шифруємо введений пароль з використанням ключа (солі)
            string EnteredHash = await new HashPassword().GetHashedPassword(EnteredPassword, PasswordSalt);
            // Порівнюємо отриманий хеш зі збереженим хешем
            return StoredHash.Equals(EnteredHash);
        }
    }
}

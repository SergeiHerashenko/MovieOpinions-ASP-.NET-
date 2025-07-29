using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOpinions.server.Domain.Helpers
{
    public class CheckingCorrectnessPassword
    {
        public async Task<bool> VerifyPassword(string enteredPassword, string passwordSalt, string storedHash)
        {
            // Шифруємо введений пароль з використанням ключа (солі)
            string enteredHash = await new HashPassword().GetHashedPassword(enteredPassword, passwordSalt);
            // Порівнюємо отриманий хеш зі збереженим хешем
            return storedHash.Equals(enteredHash);
        }
    }
}

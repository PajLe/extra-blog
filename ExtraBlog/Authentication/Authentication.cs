using System.Collections.Generic;
using System.Threading.Tasks;
using ExtraBlog.Models;
using Neo4jClient;

namespace ExtraBlog.Auth
{
    public class Authentication : IAuthentication
    {
        private readonly IGraphClient _context;

        public Authentication(IGraphClient context)
        {
            _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var userFromDB = await _context.Cypher
                                .Match("(n:User)")
                                .Where("exists(n.Username) and n.Username =~'" + username.ToLower() + "'")
                                .Return<User>("n").ResultsAsync as List<User>;

            if (userFromDB == null || userFromDB.Count == 0)
                return null;

            if (!ValidatePassword(userFromDB[0], password))
                return null;

            return userFromDB[0];
        }

        private bool ValidatePassword(User userFromDB, string enteredPasswordStr) // tes'
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userFromDB.PasswordSalt))
            {
                var enteredPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(enteredPasswordStr));
                if (userFromDB.PasswordHash.Length != enteredPasswordHash.Length)
                    return false;

                for (int i = 0; i < enteredPasswordHash.Length; i++)
                {
                    if (enteredPasswordHash[i] != userFromDB.PasswordHash[i])
                        return false;
                }
                return true;
            }
        }

        public async Task<User> Register(User user, string password, string passportNumber)
        {
            GeneratePassword(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = user.Username.ToLower();
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Cypher
                .Create("(user:User $user)")
                .WithParam("user", user)
                .ExecuteWithoutResultsAsync();

            return user;
        }

        private void GeneratePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            List<User> res = await _context.Cypher
                                .Match("(n:User)")
                                .Where("exists(n.Username) and n.Username =~'" + username.ToLower() + "'")
                                .Return<User>("n").ResultsAsync as List<User>;
            if(res.Count == 0)
                return false;

            return true;
        }
    }
}
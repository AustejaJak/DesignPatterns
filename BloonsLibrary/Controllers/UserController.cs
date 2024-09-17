using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using BloonsProject;

namespace BloonLibrary{
    public class UserController {
        private readonly GameDbContext _dbContext;

        public UserController(GameDbContext dbContext){
            _dbContext = dbContext;
        }

        public void CreateUser(User user){
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User GetUserByUsername(string username){
            return _dbContext.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
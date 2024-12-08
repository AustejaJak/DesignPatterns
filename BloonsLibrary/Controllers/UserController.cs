using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using BloonsProject;
using Microsoft.EntityFrameworkCore;

namespace BloonLibrary{
    public class UserController {
        private readonly GameDbContext _dbContext;
        private Stack<PasswordMemento> PasswordHistory;
        public User CurrentUser;

        public UserController(GameDbContext dbContext){
            PasswordHistory = new Stack<PasswordMemento>();
            _dbContext = dbContext;
        }

        public void AddPassword(string password) 
        { 
            CurrentUser.Password = password;
            PasswordHistory.Push(CurrentUser.SavePasswordToMemento());
        }

        public bool RestorePreviuosPassword() 
        {
            if (PasswordHistory.Count > 1)
            {
                PasswordHistory.Pop();
                CurrentUser.RestorePasswordFromMemento(PasswordHistory.Peek());
                ChangePassword(CurrentUser.Password);
                return true;
            }
            return false;
        }

        public void CreateUser(User user){
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            CurrentUser = user;
        }

        public User GetUserByUsername(string username){
            using (var dbContext = new GameDbContext())
            {
                User user = dbContext.Users.FirstOrDefault(u => u.Username == username);
                CurrentUser = user;
                return user;
            }
        }

        public void ChangePassword(string newPassword)
        {
            using (var dbContext = new GameDbContext()) {
                // Find the user in the database
                var existingUser = dbContext.Users.FirstOrDefault(u => u.Username == CurrentUser.Username);

                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                // Update the user's password (ensure it's hashed if necessary)
                //existingUser.Password = HashPassword(newPassword);
                existingUser.Password = newPassword;

                // Save changes to the database
                dbContext.SaveChanges();
            }
        }
    }
}
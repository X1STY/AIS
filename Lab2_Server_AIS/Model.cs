using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace lab2_Server_AIS
{
    public class Model
    {
        static Logger logger;
        private List<User> people;
        public List<User> People { get { return people; } }
        
        public Model()
        {
            logger = LogManager.GetCurrentClassLogger();
            people = new List<User>();
            GetDataFromDataBase();
        }

        void GetDataFromDataBase()
        {
            List<User> users = new List<User>();
            using (var db = new AISEntities())
            {
                try
                {
                    users = db.Users.OrderBy(x => x.id).ToList();

                } catch (Exception ex) { logger.Error(ex); }
            }
            foreach (var user in users)
            {
                people.Add(user);
            }
        }

        public void UpdateDataBase(string input)
        {
            using (var db = new AISEntities())
            {
                foreach (var item in db.Users)
                {
                    try
                    {
                        db.Users.Remove(item);
                    }
                    catch (Exception ex) { logger.Error(ex); }
                }
                foreach (var user in input.Split('\n'))
                {
                    try
                    {
                        db.Users.Add(new User().ToStruct(user));
                    }
                    catch (Exception ex) { logger.Error(ex); }
                }
                db.SaveChanges();
            }
        }

    }
}

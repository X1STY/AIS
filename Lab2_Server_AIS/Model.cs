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
            getDataFromDataBase();
        }

        void getDataFromDataBase()
        {
            List<User> users = new List<User>();
            using (var db = new AISEntities())
            {
                 users = db.Users.OrderBy(x => x.id).ToList();
            }
            foreach (var user in users)
            {
                people.Add(user);
            }
        }
        public string DeleteRecord(List<User> people, int recordNumber) 
        {
            if (recordNumber <= 0 || recordNumber > people.Count) {logger.Error("Incorrect record number id");  throw new Exception($"There is no record with id {recordNumber}\n");  }
            User human = people[recordNumber - 1];
            using (var db = new AISEntities())
            {
                db.Users.Remove(db.Users.Find(human.id));
                db.SaveChanges();
            }
            people.Remove(human);
            return $"record №{recordNumber} deleted successfully";

        }
        public string AddRecord(List<User> people, string input)
        {
            User human = new User().ToStruct(input);
            using (var db = new AISEntities())
            {
                db.Users.Add(human);
                db.SaveChanges();
            }
            people.Add(human);
            return "New record added successfully";
        }

        public User GetSingleRecord(int recordNumber)
        {
            if (recordNumber <= 0 || recordNumber > people.Count) { logger.Error("Unexisted record id");throw new Exception($"There is no record with id {recordNumber}\n"); }
            return people[recordNumber - 1];

        }

    }
}

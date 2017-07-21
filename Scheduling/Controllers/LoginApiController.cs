using Scheduling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Scheduling.Controllers
{
    public class LoginApiController : ApiController
    {

        [HttpGet]
        public Dictionary<string,bool> isValidUser(string username, string password)
        {
            Dictionary<string, bool> valid = new Dictionary<string, bool>();
            DBEntities db = new DBEntities();
            if (db.Students.Where(l => l.Password == password && (l.StudentId.ToString() == username)).Count() > 0)
            {
                valid.Add("value", true);
                return valid;
            }
            valid.Add("value", false);
            return valid;
            //DO something as you want 

        }
    }
}

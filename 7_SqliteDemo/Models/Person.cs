using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Framework
{
    public class Person
    {
        public Int64 Id { get; set; } //注意要用Int64

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }


}

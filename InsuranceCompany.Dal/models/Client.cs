using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.models
{
    [Table("clients")]
    public class Client
    {
        [Key]
        [Column("client_id")]
        public int client_id { get; set; }
        public string full_name { get; set; }
        public DateTime birth_date  { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public DateTime created_at { get; set; }


    }
}

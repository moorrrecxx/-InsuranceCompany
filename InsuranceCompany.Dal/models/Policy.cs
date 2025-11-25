using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.models
{
    [Table("policies")]
    public class policy
    {
        [Key]
        [Column("policy_id")]
        public int policy_id { get; set; }
        [Column("client_id")]
        public int client_id { get; set; }
        [Column("policy_type_id")]
        public int policy_type_id { get; set; }
        [Column("policy_number")]
        public string policy_number { get; set; }
        [Column("start_date")]
        public DateTime start_date { get; set; }
        [Column("end_date")]
        public DateTime end_date { get; set; }
        [Column("premium_amount")]
        public decimal premium_amount { get; set; }
        [Column("status")]
        public string status { get; set; }
        
    }
}

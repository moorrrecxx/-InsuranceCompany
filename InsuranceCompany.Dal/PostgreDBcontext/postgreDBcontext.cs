using InsuranceCompany.Dal.models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.PostgreDBcontext
{
    public class postgreDBcontext : DbContext
    {
        public postgreDBcontext(DbContextOptions<postgreDBcontext> options): base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<policy> Policies { get; set; }

    } 
}

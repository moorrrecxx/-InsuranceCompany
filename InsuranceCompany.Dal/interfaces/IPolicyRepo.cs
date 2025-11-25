using InsuranceCompany.Dal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.interfaces
{
    public interface IPolicyRepo
    {
        Task<policy> Create(policy bonusAccount);
        Task<policy> GetById(long accountId);
        Task<List<policy>> GetAll();
        Task Update(policy bonusAccount);
        Task<bool> Delete(long accountId);
    }
}

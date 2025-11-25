using InsuranceCompany.Dal.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.interfaces
{
    public interface IClientRepo
    {
        Task<Client> Create(Client bonusAccount);
        Task<Client> GetById(long accountId);
        Task<List<Client>> GetAll();
        Task Update(Client bonusAccount);
        Task<bool> Delete(long accountId);
    }
}

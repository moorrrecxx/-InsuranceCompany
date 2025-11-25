using InsuranceCompany.Dal.interfaces;
using InsuranceCompany.Dal.models;
using InsuranceCompany.Dal.PostgreDBcontext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceCompany.Dal.concret
{
    public class PolicyRepo : IPolicyRepo
    {
        private postgreDBcontext _context;
        public PolicyRepo(postgreDBcontext context)
        {
            _context = context;
        }
        public async Task<policy> Create(policy policy)
        {
            var newpolicy = await _context.Policies.AddAsync(policy);
            await _context.SaveChangesAsync();
            return newpolicy.Entity;

        }

        public async Task<bool> Delete(long accountId)
        {
            var account = await GetById(accountId);
            _context.Policies.Remove(account);
            await _context.SaveChangesAsync();
            if (account.policy_id.Equals(null))
                return true;
            else
                return false;

        }

        public async Task<List<policy>> GetAll()
        {
            return await _context.Policies.ToListAsync();
        }

        public async Task<policy> GetById(long accountId)
        {
            if (!accountId.Equals(null))
            {
                var account = await _context.Policies.FirstOrDefaultAsync(c => c.policy_id == accountId);
                if (!account.Equals(null))
                    return account;
                else
                    throw new KeyNotFoundException($"Account with ID {accountId} not found.");
            }
            else
                throw new ArgumentNullException(nameof(accountId), "Account ID cannot be null.");
        }

        public Task Update(policy policy)
        {
            _context.Policies.Update(policy);
            return _context.SaveChangesAsync();
        }
    }
}

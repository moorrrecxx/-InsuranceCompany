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
    public class ClientRepo : IClientRepo
    {
        private postgreDBcontext _context;
        public ClientRepo(postgreDBcontext context)
        {
            _context = context;
        }
        public async Task<Client> Create(Client client)
        {
            var newclient = await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
            return newclient.Entity;

        }

        public async Task<bool> Delete(long accountId)
        {
            var account = await GetById(accountId);
            _context.Clients.Remove(account);
            await _context.SaveChangesAsync();
            if (account.client_id.Equals(null))
                return true;
            else
                return false;

        }

        public async Task<List<Client>> GetAll()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> GetById(long accountId)
        {
            if (!accountId.Equals(null))
            {
                var account = await _context.Clients.FirstOrDefaultAsync(c => c.client_id == accountId);
                if (!account.Equals(null))
                    return account;
                else
                    throw new KeyNotFoundException($"Account with ID {accountId} not found.");
            }
            else
                throw new ArgumentNullException(nameof(accountId), "Account ID cannot be null.");
        }

        public Task Update(Client client)
        {
            _context.Clients.Update(client);
            return _context.SaveChangesAsync();
        }
    }
}


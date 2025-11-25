using InsuranceCompany.Dal.interfaces; 
using InsuranceCompany.Dal.concret; 
using InsuranceCompany.Dal.PostgreDBcontext;
using InsuranceCompany.Dal.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace InsuranceCompany 
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

           
            var connectionString = "Host=localhost;Username=postgres;Password=1302062006;Database=mydb";


            var optionsBuilder = new DbContextOptionsBuilder<postgreDBcontext>();
            
            optionsBuilder.UseNpgsql(connectionString);

            using (var dbContext = new postgreDBcontext(optionsBuilder.Options))
            {
 
                IClientRepo clientRepo = new ClientRepo(dbContext);
                IPolicyRepo policyRepo = new PolicyRepo(dbContext);

                while (true)
                {
                    Console.WriteLine("\n\n--- Insurance Company Management System ---");
                    Console.WriteLine("1. Manage Clients");
                    Console.WriteLine("2. Manage Policies");
                    Console.WriteLine("q. Exit");
                    Console.Write("Select an option: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await ClientMenu(clientRepo);
                            break;
                        case "2":
                            await PolicyMenu(policyRepo, clientRepo);
                            break;
                        case "q":
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }

        private static async Task ClientMenu(IClientRepo clientRepo)
        {
            while (true)
            {
                Console.WriteLine("\n\n\n--- Client Management ---");
                Console.WriteLine("1. Add Client");
                Console.WriteLine("2. View Clients");
                Console.WriteLine("3. Update Client");
                Console.WriteLine("4. Delete Client");
                Console.WriteLine("b. Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                Console.WriteLine("\n");

                switch (choice)
                {
                    case "1": 
                        Console.Write("Enter Full Name: ");
                        var fullName = Console.ReadLine();

                        Console.Write("Enter Birth Date (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out var birthDate))
                        {
                            Console.WriteLine("Invalid date format.");
                            break;
                        }

                        
                        var birthDateUtc = DateTime.SpecifyKind(birthDate, DateTimeKind.Utc);

                        Console.Write("Enter Phone: ");
                        var phone = Console.ReadLine();
                        Console.Write("Enter Email: ");
                        var email = Console.ReadLine();

                        var newClient = new Client
                        {
                            full_name = fullName,
                            birth_date = birthDateUtc, 
                            phone = phone,
                            email = email,
                            created_at = DateTime.UtcNow
                        };
                        await clientRepo.Create(newClient);
                        Console.WriteLine($"Client '{newClient.full_name}' successfully added.");
                        break;

                    case "2": 
                        var clients = await clientRepo.GetAll();
                        if (clients == null || clients.Count == 0)
                        {
                            Console.WriteLine("No clients found.");
                            break;
                        }
                        foreach (var client in clients)
                        {
                            Console.WriteLine($"ID: {client.client_id} | Name: {client.full_name} | Email: {client.email} | Phone: {client.phone} | Created: {client.created_at.ToShortDateString()}");
                        }
                        break;

                    case "3": 
                        Console.Write("Enter Client ID to update: ");
                        if (!int.TryParse(Console.ReadLine(), out var clientIdUpdate))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        var clientToUpdate = await clientRepo.GetById(clientIdUpdate);
                        if (clientToUpdate == null)
                        {
                            Console.WriteLine("Client not found.");
                            break;
                        }

                        Console.Write($"Enter new Full Name (Current: {clientToUpdate.full_name}, leave blank to keep): ");
                        var newFullName = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newFullName))
                            clientToUpdate.full_name = newFullName;

                        Console.Write($"Enter new Phone (Current: {clientToUpdate.phone}, leave blank to keep): ");
                        var newPhone = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newPhone))
                            clientToUpdate.phone = newPhone;

                        Console.Write($"Enter new Email (Current: {clientToUpdate.email}, leave blank to keep): ");
                        var newEmail = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newEmail))
                            clientToUpdate.email = newEmail;

                        

                        await clientRepo.Update(clientToUpdate);
                        Console.WriteLine($"Client ID {clientIdUpdate} successfully updated.");
                        break;

                    case "4": 
                        Console.Write("Enter Client ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out var clientIdDelete))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        await clientRepo.Delete(clientIdDelete);
                        Console.WriteLine($"Client ID {clientIdDelete} successfully deleted (Policies associated with this client may need manual deletion or cascade settings).");
                        break;

                    case "b":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

       
        private static async Task PolicyMenu(IPolicyRepo policyRepo, IClientRepo clientRepo)
        {
            while (true)
            {
                Console.WriteLine("\n\n\n--- Policy Management ---");
                Console.WriteLine("1. Add Policy");
                Console.WriteLine("2. View Policies");
                Console.WriteLine("3. Update Policy");
                Console.WriteLine("4. Delete Policy");
                Console.WriteLine("b. Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();
                Console.WriteLine("\n");

                switch (choice)
                {
                    case "1": 
                        Console.Write("Enter Client ID for the policy: ");
                        if (!int.TryParse(Console.ReadLine(), out var clientId))
                        {
                            Console.WriteLine("Invalid Client ID.");
                            break;
                        }

                        var client = await clientRepo.GetById(clientId);
                        if (client == null)
                        {
                            Console.WriteLine("Client with this ID was not found. Cannot create policy.");
                            break;
                        }

                        Console.Write("Enter Policy Type ID: ");
                        if (!int.TryParse(Console.ReadLine(), out var policyTypeId))
                        {
                            Console.WriteLine("Invalid Policy Type ID.");
                            break;
                        }
                        Console.Write("Enter Policy Number: ");
                        var policyNumber = Console.ReadLine();

                        
                        Console.Write("Enter Start Date (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out var startDate))
                        {
                            Console.WriteLine("Invalid date format.");
                            break;
                        }
                        
                        var startDateUtc = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);

                       
                        Console.Write("Enter End Date (YYYY-MM-DD): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out var endDate))
                        {
                            Console.WriteLine("Invalid date format.");
                            break;
                        }
                        
                        var endDateUtc = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);


                        Console.Write("Enter Premium Amount: ");
                        
                        if (!decimal.TryParse(Console.ReadLine().Replace(',', '.'),
                                               System.Globalization.NumberStyles.Currency,
                                               System.Globalization.CultureInfo.InvariantCulture,
                                               out var premiumAmount))
                        {
                            Console.WriteLine("Invalid amount format.");
                            break;
                        }

                        Console.Write("Enter Status (e.g., Active, Expired): ");
                        var status = Console.ReadLine();

                        var newPolicy = new policy
                        {
                            client_id = client.client_id, 
                            policy_type_id = policyTypeId,
                            policy_number = policyNumber,
                            start_date = startDateUtc, 
                            end_date = endDateUtc,     
                            premium_amount = premiumAmount,
                            status = status
                        };
                        await policyRepo.Create(newPolicy);
                        Console.WriteLine($"Policy '{newPolicy.policy_number}' successfully added for Client ID {clientId}.");
                        break;

                    case "2": 
                        var policies = await policyRepo.GetAll();
                        if (policies == null || policies.Count == 0)
                        {
                            Console.WriteLine("No policies found.");
                            break;
                        }
                        foreach (var pol in policies)
                        {


                            Console.WriteLine($"ID: {pol.policy_id} | Client: {pol.client_id} | Number: {pol.policy_number} | Type: {pol.policy_type_id} | Premium: {pol.premium_amount:C} | Status: {pol.status}");
                        }
                        break;

                    case "3": 
                        Console.Write("Enter Policy ID to update: ");
                        if (!int.TryParse(Console.ReadLine(), out var policyIdUpdate))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        var policyToUpdate = await policyRepo.GetById(policyIdUpdate);
                        if (policyToUpdate == null)
                        {
                            Console.WriteLine("Policy not found.");
                            break;
                        }

                        Console.Write($"Enter new Premium Amount (Current: {policyToUpdate.premium_amount}, leave blank to keep): ");
                        var newPremiumInput = Console.ReadLine();
                        
                        if (decimal.TryParse(newPremiumInput.Replace(',', '.'),
                                             System.Globalization.NumberStyles.Currency,
                                             System.Globalization.CultureInfo.InvariantCulture,
                                             out var newPremium))
                            policyToUpdate.premium_amount = newPremium;

                        Console.Write($"Enter new Status (Current: {policyToUpdate.status}, leave blank to keep): ");
                        var newStatus = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(newStatus))
                            policyToUpdate.status = newStatus;

                       

                        await policyRepo.Update(policyToUpdate);
                        Console.WriteLine($"Policy ID {policyIdUpdate} successfully updated.");
                        break;

                    case "4": 
                        Console.Write("Enter Policy ID to delete: ");
                        if (!int.TryParse(Console.ReadLine(), out var policyIdDelete))
                        {
                            Console.WriteLine("Invalid ID.");
                            break;
                        }
                        await policyRepo.Delete(policyIdDelete);
                        Console.WriteLine($"Policy ID {policyIdDelete} successfully deleted.");
                        break;

                    case "b":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
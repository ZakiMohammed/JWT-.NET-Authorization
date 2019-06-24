using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Woo.Model;
using Woo.Data;

namespace Woo.Repository
{
    public class ApplicationRepository
    {
        #region Product
        public List<Product> GetProducts()
        {
            using (var context = new WooContext())
            {
                return context.Products.ToList();
            }
        }
        public PaginationDTO<Product> GetProducts(string search, int index, int size, string orderBy, string orderDir)
        {
            var dataAccess = new DataAccess();
            var dataSet = dataAccess.ExecuteStoredProcedure("usp_ProductPagination", new List<DataAccess.Parameters>
            {
                new DataAccess.Parameters { Name = "@search", Value = search },
                new DataAccess.Parameters { Name = "@skip", Value = index },
                new DataAccess.Parameters { Name = "@take", Value = size },
                new DataAccess.Parameters { Name = "@orderBy", Value = orderBy },
                new DataAccess.Parameters { Name = "@orderDir", Value = orderDir },
            });

            var data = dataAccess.ToList<Product>(dataSet.Tables[0]);
            var filterCount = Convert.ToInt32(dataSet.Tables[1].Rows[0][0]);
            var totalCount = Convert.ToInt32(dataSet.Tables[2].Rows[0][0]);

            return new PaginationDTO<Product>
            {
                Data = data,
                FilterCount = filterCount,
                TotalCount = totalCount
            };
        }
        public Product GetProduct(int id)
        {
            using (var context = new WooContext())
            {
                return context.Products.ToList().Find(i => i.ID == id);
            }
        }
        #endregion

        #region Service
        public List<Service> GetServices()
        {
            using (var context = new WooContext())
            {
                return context.Services.ToList();
            }
        }
        public Service GetService(int id)
        {
            using (var context = new WooContext())
            {
                return context.Services.ToList().Find(i => i.ID == id);
            }
        }
        #endregion

        #region Contact
        public Contact AddContact(Contact contact)
        {
            using (var context = new WooContext())
            {
                context.Contacts.Add(contact);
                context.SaveChanges();
                return contact;
            }
        }
        #endregion

        #region User
        public List<User> GetUsers()
        {
            using (var context = new WooContext())
            {
                return context.Users.ToList();
            }
        }
        public User AuthenticateUser(string userName, string password)
        {
            using (var context = new WooContext())
            {
                return context.Users.ToList().Find(i => i.UserName == userName && i.Password == password);
            }
        }
        public bool IsUserExist(User user)
        {
            using (var context = new WooContext())
            {
                var users = context.Users.Where(i => i.UserName == user.UserName || i.Email == user.Email);
                return users.Any();
            }
        }
        public User AddUser(User user)
        {
            using (var context = new WooContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
                return user;
            }
        }
        #endregion
    }
}

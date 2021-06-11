using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using CustomerApi.Repository;
using CustomerApi.Models;


namespace CustomerNunitTest
{
    public class Tests
    {
        List<Customer> customers = new List<Customer>();
        IQueryable<Customer> data;
        Mock<DbSet<Customer>> mockSet;
        Mock<shopContext> Customercontextmock;
        [SetUp]
        public void Setup()
        {
            customers = new List<Customer>()
            {
                new Customer{Custid=1,CustomerName="Akash",Phoneno="9876543210",Mailid="ash4472@gmail.com"},
                 new Customer{Custid=2,CustomerName="Aravind",Phoneno="9876543310",Mailid="aravind@gmail.com"}

            };
            data = customers.AsQueryable();
            mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var p = new DbContextOptions<shopContext>();
            Customercontextmock = new Mock<shopContext>(p);
            Customercontextmock.Setup(x => x.Customers).Returns(mockSet.Object);
        }

        [Test]
        public void AddCustomerTest()
        {
            var repo = new CustomerRepo(Customercontextmock.Object);
            var obj = repo.PostCustomer(new Customer { Custid = 1, CustomerName = "Ajay", Phoneno = "9876543211", Mailid = "ash4472@gmail.com" });
            Assert.IsNotNull(obj);
        }

        [TestCase(1)]
        public void GetCustomerbyID(int Custid)
        {

            Mock<ICustomerRepo> mock = new Mock<ICustomerRepo>();

            mock.Setup(p => p.GetById(Custid)).Returns(customers[0]);

            Customer a = mock.Object.GetById(Custid);

            Assert.AreEqual("Akash", a.CustomerName);

        }

        [Test]
        public void GetCustomers()
        {
            Mock<ICustomerRepo> mock = new Mock<ICustomerRepo>();
            Assert.IsNotNull(mock.Setup(p => p.GetCustomer()).Returns(customers));

        }

        [Test]
        public void Get_AllcustomerList()
        {
            var repo = new CustomerRepo(Customercontextmock.Object);
            var Customerlist = repo.GetCustomer();
            Assert.AreEqual(2, Customerlist.Count());
        }

        /*[TestCase(1)]
        public void DeleteCustomers(int id)
        {
            var repo = new CustomerRepo(Customercontextmock.Object);
            Assert.IsNotNull(repo.DeleteCustomer(id));


        }*/

        [Test]
        public void UpdateCustomerTest()
        {
            int id = 1;
            var repo = new CustomerRepo(Customercontextmock.Object);
            var obj = repo.PutCustomer(id, new Customer { Custid = 1, CustomerName = "Akash", Phoneno = "9876543210", Mailid = "ash4472@gmail.com" });
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }

        [Test]
        public void DeleteCustomerTest()
        {
            int id = 1;
            var repo = new CustomerRepo(Customercontextmock.Object);
            var obj = repo.DeleteCustomer(id);
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }
    }
}
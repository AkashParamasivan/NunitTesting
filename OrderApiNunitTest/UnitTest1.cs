using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using OrderApi1.Repository;
using OrderApi1.Models;

namespace OrderApiNunitTest
{
    public class Tests
    {
        List<Ordertable> orders = new List<Ordertable>();
        IQueryable<Ordertable> data;
        Mock<DbSet<Ordertable>> mockSet;
        Mock<shopContext> Ordercontextmock;
        [SetUp]
        public void Setup()
        {
            orders = new List<Ordertable>()
            {
                new Ordertable{Orderid=101,Pid=1,Customerid=1,OrderDate="17-05-2021", ListofProducts="soap",TotalAmount=200},
                 new Ordertable{Orderid=102,Pid=2,Customerid=1,OrderDate="17-05-2021", ListofProducts="shampoo",TotalAmount=200}

            };
            data = orders.AsQueryable();
            mockSet = new Mock<DbSet<Ordertable>>();
            mockSet.As<IQueryable<Ordertable>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Ordertable>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Ordertable>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Ordertable>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var p = new DbContextOptions<shopContext>();
            Ordercontextmock = new Mock<shopContext>(p);
            Ordercontextmock.Setup(x => x.Ordertables).Returns(mockSet.Object);
        }

        [Test]
        public void AddOrderTest()
        {
            var repo = new OrderRepo(Ordercontextmock.Object);
            var obj = repo.PostOrder(new Ordertable { Orderid = 101, Pid = 1, Customerid = 1, OrderDate = "17-05-2021", ListofProducts = "soap", TotalAmount = 200 });
            Assert.IsNotNull(obj);
        }

        [TestCase("17-05-2021")]
        public void GetOrderbydate(string date)
        {
       
                var repo = new OrderRepo(Ordercontextmock.Object);
                var Orderlist = repo.GetByOrderdate(date);
                Assert.AreEqual(2, Orderlist.Count());
        }

        [TestCase(1)]
        public void GetByList_With_ProductIDTest(int pid)
        {
            var repo = new OrderRepo(Ordercontextmock.Object);
            var Orderlist = repo.GetByProductID(pid);
            Assert.AreEqual(1, Orderlist.Count());
        }

        [TestCase(1)]
        public void GetByList_With_CustomerIDTest(int cid)
        {
            var repo = new OrderRepo(Ordercontextmock.Object);
            var Orderlist = repo.GetByCustomerID(cid);
            Assert.AreEqual(2, Orderlist.Count());
        }

        [Test]
        public void UpdateOrderTest()
        {
            int id = 101;
            var repo = new OrderRepo(Ordercontextmock.Object);
            var obj = repo.PutOrder(id, new Ordertable { Orderid = 101, Pid = 1, Customerid = 1, OrderDate = "17-05-2021", ListofProducts = "soap", TotalAmount = 200 });
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }

        [Test]
        public void DeleteProductTest()
        {
            int id = 1;
            var repo = new OrderRepo(Ordercontextmock.Object);
            var obj = repo.DeleteOrder(id);
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }
    }
}
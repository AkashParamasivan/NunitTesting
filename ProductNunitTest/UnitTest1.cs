using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ProductApi.Repository;
using ProductApi.Models;
using ProductApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductNunitTest
{
    public class Tests
    {
        List<Product> products = new List<Product>();
        IQueryable<Product> data;
        Mock<DbSet<Product>> mockSet;
        Mock<shopContext> Productcontextmock;

        [SetUp]
        public void Setup()
        {
            products = new List<Product>()
            {
                new Product{Pid = 1, ProductName = "Shampoo",Price = 100,  Quantity= 2,Description="xxxxxxx"},
                 new Product{Pid = 2, ProductName = "Soap",Price = 50,  Quantity= 2,Description="yyyyyyy"}

            };
            data = products.AsQueryable();
            mockSet = new Mock<DbSet<Product>>();
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var p = new DbContextOptions<shopContext>();
            Productcontextmock = new Mock<shopContext>(p);
            Productcontextmock.Setup(x => x.Products).Returns(mockSet.Object);
        }

        [Test]
        public void AddProductTest()
        {
            var repo = new ProductRepo(Productcontextmock.Object);
            var obj = repo.PostProduct(new Product { Pid = 1, ProductName = "Shampoo", Price = 100, Quantity = 2, Description = "xxxxxxx" });
            Assert.IsNotNull(obj);
        }

        [TestCase(1)]
        public void GetProductbyID(int pid)
        {

            Mock<IProductRepo> mock = new Mock<IProductRepo>();

            mock.Setup(p => p.GetById(pid)).Returns(products[0]);

            Product a = mock.Object.GetById(pid);

            //Assert.IsNotNull(a);
            Assert.AreEqual("Shampoo", a.ProductName);

        }
        [Test]
        public void GetProducts()
        {
            Mock<IProductRepo> mock = new Mock<IProductRepo>();
            Assert.IsNotNull(mock.Setup(p => p.GetProduct()).Returns(products));
           
        }
        [Test]
        public void Get_AllProductList()
        {
            var repo = new ProductRepo(Productcontextmock.Object);
            var productlist = repo.GetProduct();
            Assert.AreEqual(2, productlist.Count());
        }


       /* [TestCase(1)]
        public async  Task DeleteProducts(int id)
        {
            *//* Mock<IProductRepo> mock = new Mock<IProductRepo>();
             var a = mock.Setup(p => p.DeleteProduct(id));
             Assert.Pass();*//*
            var repo =  new ProductRepo(Productcontextmock.Object);
            var data = await repo.DeleteProduct(id);
             Assert.AreEqual(products[0], data);
            //Assert.AreEqual(1, Deletedlist.Count());
        }*/

        [Test]
        public void UpdateProductTest()
        {
            int id = 1;
            var repo = new ProductRepo(Productcontextmock.Object);
            var obj = repo.PutProduct(id,new Product { Pid = 1, ProductName = "Shampoo", Price = 100, Quantity = 2, Description = "xxxxxxx" });
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }

        [Test]
        public void DeleteProductTest()
        {
            int id = 1;
            var repo = new ProductRepo(Productcontextmock.Object);
            var obj = repo.DeleteProduct(id);
            Assert.IsNotNull(obj);
            //Assert.AreNotEqual(products[0], obj);
        }



        /*[Test]
        public async Task UpdateProduct()
        {
            var repo = new ProductRepo(Productcontextmock.Object);
            int id = 1;
            Product item=new Product();
            //item.Pid = 1;
            item.ProductName = "Shampoo";
            item.Price = 150;
            item.Quantity = 2;
            item.Description = "xxxxx";
            Product data=await repo.PutProduct(id,item );
            Assert.AreEqual(150, data.Price);

        }*/




    }
}
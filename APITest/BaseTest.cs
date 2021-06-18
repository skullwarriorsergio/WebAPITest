using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAPITest.Controllers;
using WebAPITest.Models;
using Xunit;

namespace APITest 
{
    [TestClass]
    public class BaseTest
    {
        protected DbContextOptions<TodoContext> ContextOptions { get; }


        public BaseTest()
        {
            ContextOptions = new DbContextOptionsBuilder<TodoContext>().UseInMemoryDatabase("test", null, null).Options;

            using (var context = new TodoContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Add(new Gateway() { IP = "127.0.0.1", Name = "CISCO Router", SerialNumber = "SN123456789" });
                context.SaveChanges();
            }
        }


        [TestMethod]
        public void CanGetGateways()
        {
            using (var context = new TodoContext(ContextOptions))
            {
                var controller = new GatewaysController(context);

                var items = controller.GetGateways().Result.Value.ToList();

                Assert.AreEqual(1, items.Count);
            }
        }

        [TestMethod]
        public void CanGetDevices()
        {
            using (var context = new TodoContext(ContextOptions))
            {
                var controller = new GatewaysController(context);

                var items = controller.GetDevices("SN123456789").Result.Value;
                Assert.AreEqual(null, items);
            }
        }

        [TestMethod]
        public void AddDevices()
        {
            using (var context = new TodoContext(ContextOptions))
            {
                var controller = new GatewaysController(context);

                //Test only allow up to 10 devices per Gateway
                Task<ActionResult<Device>> result = null;
                for (int i = 0; i < 11; i++)
                {
                    result = controller.PostDevices("SN123456789", new Device() { Vendor = "CISCO", DateCreated = DateTime.MinValue, Status = DeviceStatus.Offline, UID = 564654+i });
                }
                Assert.AreEqual(true, result.Result.Result is ForbidResult);
            }
        }

        [TestMethod]
        public void AddGateway()
        {
            using (var context = new TodoContext(ContextOptions))
            {
                var controller = new GatewaysController(context);

                //Test normal gateway post
                var result = controller.PostGateway(new Gateway() { IP = "192.168.2.5", Name = "CISCO Router", SerialNumber = "SN0508070903" });
                Assert.AreEqual(true, result.IsCompleted);

                //Test Bad IP address
                result = controller.PostGateway(new Gateway() { IP = "192.168.582.5", Name = "CISCO Router", SerialNumber = "SN12121212" });
                Assert.AreEqual(true, result.Result.Result is BadRequestObjectResult);                
            }
        }
    }
}

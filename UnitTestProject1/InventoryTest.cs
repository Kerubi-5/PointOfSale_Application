using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using PointOfSale_Application.Inventory;
using PointOfSale_Application.Classes;

namespace UnitTestProject1
{
    [TestClass]
    public class InventoryTest
    {
        [TestMethod]
        public void Stock_UpdateIsWorking()
        {
            ReceivingPage receivingPage = new ReceivingPage();
            bool res = true;

            try
            {
                receivingPage.Update_Stock(1, 1);
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void Stock_UpdatePriceWorking()
        {
            ReceivingPage receivingPage = new ReceivingPage();
            bool res = true;

            try
            {
                receivingPage.Update_Price(1, 19299);
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void stock_UpdateNameIsWorking()
        {
            ReceivingPage receivingPage = new ReceivingPage();
            bool res = true;

            try
            {
                receivingPage.Update_Name(1, "GTX 1080 TI");
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void GetProductMaxIDIsWorking()
        {
            ReceivingPage receivingPage = new ReceivingPage();

            int res = ProductModel.GetMaxID("product");

            Assert.AreEqual(35, res);
        }

        [TestMethod]
        public void GetMaxProductStatusId()
        {
            ReceivingPage receivingPage = new ReceivingPage();

            int res = ProductModel.GetMaxID("status");

            Assert.AreEqual(41, res);
        }

        [TestMethod]
        public void ReturnToSellerButtonIsWorking()
        {
            Product_Transaction_Model transact = new Product_Transaction_Model();
        }
    }
}

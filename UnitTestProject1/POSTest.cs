using Microsoft.VisualStudio.TestTools.UnitTesting;
using PointOfSale_Application.Classes;
using PointOfSale_Application.POS;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace UnitTestProject1
{
    [TestClass]
    public class POSTest
    {
        //Checking Class Models Methods Working
        [TestMethod]
        public void Check_Product_InTransaction_Records()
        {
            List<Product_Transaction_Model> transaction_list = Product_Transaction_Model.GetRecords();

            Assert.AreEqual(13, transaction_list.Count);
        }

        [TestMethod]
        public void Check_Good_Product_Records()
        {
            List<ProductModel> product_list = ProductModel.GetGoodProducts();

            Assert.AreEqual(34, product_list.Count);
        }

        [TestMethod]
        public void Check_Defective_Product_Records()
        {
            List<ProductModel> product_list = ProductModel.GetDefectiveProducts();

            Assert.AreEqual(1, product_list.Count);
        }

        [TestMethod]
        public void Check_All_Product_Records()
        {
            List<ProductModel> product_list = ProductModel.GetRecords();

            Assert.AreEqual(36, product_list.Count);
        }

        [TestMethod]
        public void Check_Receipt_Records()
        {
            List<ReceiptModel> receipt_list = ReceiptModel.GetRecords();

            Assert.AreEqual(9, receipt_list.Count);
        }

        [TestMethod]
        public void Check__Staff_Records()
        {
            List<StaffModel> staff_list = StaffModel.GetRecords();

            Assert.AreEqual(4, staff_list.Count);
        }

        [TestMethod]
        public void VerifyLogin()
        {
            bool res = StaffModel.VerifyLogin("kerubi", "12345");

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckMoneyIfEquals()
        {
            POS pOS = new POS();

            bool res = pOS.CheckMoney(2, 2);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckMoneyIfFirstIsHigher()
        {
            POS pOS = new POS();

            bool res = pOS.CheckMoney(3, 2);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckMoneyIfSecondIsHiger()
        {
            POS pOS = new POS();

            bool res = pOS.CheckMoney(2, 3);

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void CheckPayIsWorking()
        {
            POS pOS = new POS();

            decimal res = pOS.Pay(1000, 500);

            Assert.AreEqual(500, res);
        }

        [TestMethod]
        public void GetMaxIDWorking()
        {
            POS pOS = new POS();
            int res = ReceiptModel.GetMaxID();

            Assert.AreEqual(10, res);
        }

        [TestMethod]
        public void CheckRefundDefectiveIsWorking()
        {
            POS pOS = new POS();
            bool res = pOS.CheckProductDefectiveStatus(30);

            Assert.IsTrue(res);
        }


    }
}

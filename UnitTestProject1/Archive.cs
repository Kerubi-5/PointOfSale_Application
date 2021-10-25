using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject1
{
    /// <summary>
    /// Summary description for Archive
    /// </summary>
    [TestClass]
    public class Archive
    {
        public Archive()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //Check POS App is Working
        /*
        [TestMethod]
        public void TransactTableIsWorking()
        {
            bool res = true;
            POS pOS = new POS();
            SqlConnection con = DBConnection.GetConnection();

            try
            {
                pOS.TransactTable(1, 0, "Payment");
            }
            catch
            {
                res = false;
            }

            Assert.AreEqual(res, true);
        }

        
        [TestMethod]
        public void SaveReceiptIsWorking()
        {
            POS pOS = new POS();
            ReceiptModel receiptModel = new ReceiptModel();
            bool res = true;

            receiptModel.report_id = 100;
            receiptModel.sales_type = "Return to Seller";
            receiptModel.address = "Seller's Address";
            receiptModel.paid_amount = 10.55m;
            receiptModel.date_issued = System.DateTime.Now;

            try
            {
                ReceiptModel.SaveReceipt(receiptModel);
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }
        

        [TestMethod]
        public void SaveTransactionIsWorking()
        {
            POS pOS = new POS();
            bool res = true;
            Product_Transaction_Model product_transaction = new Product_Transaction_Model();

            product_transaction.prod_id = 100;
            product_transaction.name = "Try";
            product_transaction.qty_transacted = 1000;
            product_transaction.total_amount = 100.99m;

            try
            {
                Product_Transaction_Model.Save_Production_Transaction(100, product_transaction);
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }
        */

        //Inventory
        /*
        [TestMethod]
        public void AddToProductModelIsWorking()
        {
            bool res = true;

            try
            {
                int id = ProductModel.GetMaxID("product");
                ProductModel.Add_ProductModel(id, "AMD RX 5600", "Gigabyte", 29999.99m);
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }

        [TestMethod]
        public void AddToStatusIsWorking()
        {
            bool res = true;

            try
            {
                int id = ProductModel.GetMaxID("status");
                ProductModel.Add_ProductQuantity(id, 32, 5);
                
            }
            catch
            {
                res = false;
            }

            Assert.IsTrue(res);
        }
        */
    }
}

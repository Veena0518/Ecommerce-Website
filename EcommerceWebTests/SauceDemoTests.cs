using EcommerceWebsiteLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Threading;

namespace EcommerceWebTests
{
    [TestClass]
    public class SauceDemoTests
    {
        OpenAndClose setup;
        IWebDriver driver;

        [TestInitialize]
        public void SetUp()
        {
            setup = new OpenAndClose();
            driver = setup.Open(null);
        }

        [TestCleanup]
        public void TearDown()
        {
            setup.Close();
        }
        
        [TestMethod]
        [DataRow("standard_user", "secret_sauce", null)]
        [DataRow("performance_glitch_user", "secret_sauce", null)]
        [DataRow("problem_user", "secret_sauce", null)]
        [DataRow("locked_out_user", "secret_sauce", "Epic sadface: Sorry, this user has been locked out.")]
        [DataRow("standard_user", "q2343254", "Epic sadface: Username and password do not match any user in this service")]
        [DataRow("asdfds890", "secret_sauce", "Epic sadface: Username and password do not match any user in this service")]
        [DataRow("", "secret_sauce", "Epic sadface: Username is required")]
        [DataRow("standard_user", "", "Epic sadface: Password is required")]
        [DataRow("", "", "Epic sadface: Username is required")]
        [DataRow("null", "null", "Epic sadface: Username and password do not match any user in this service")]
        [DataRow("-23@@#$", "+_@#$3", "Epic sadface: Username and password do not match any user in this service")]
        public void TestLoginPanel(string username, string password, string errormsg = null)
        {
            //logs in
            LoginPage login = Login(username, password);
            Thread.Sleep(1000);
            if (errormsg == null)
            {
                Assert.AreEqual("Swag Labs", driver.Title, "Title does not match");
            }
            else if (errormsg != null)
            {
                Assert.AreEqual(errormsg, login.ErrorMessageField.Text, "Error msg does not match");
            }
        }
        //helper method to Login
        private LoginPage Login(string username, string password)
        {
            Thread.Sleep(1000);
            LoginPage login = new LoginPage(driver);
            login.UsernameTextBox.Click();
            login.UsernameTextBox.SendKeys(username);
            login.PasswordTextBox.Clear();
            login.PasswordTextBox.SendKeys(password);
            login.LoginButton.Click();
            return login;
        }

        [TestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void TestLogOutLink(string username, string password)
        {
            LoginPage signin = Login(username, password);

            ProductPage product = new ProductPage(driver);
            //clicks on backpack and adds to cart
            product.BackPack.Click();
            product.AddToCart.Click();
            //clicks on menu button and clicks on all items link
            product.OpenMenuButton.Click();
            Thread.Sleep(500);
            product.AllItemsLink.Click();
            //clicks on bike light and adds to cart
            product.BikeLight.Click();
            product.AddToCart.Click();
            //clicks on menu button and clicks on log out
            product.OpenMenuButton.Click();
            Thread.Sleep(500);
            product.LogOutLink.Click();
        }
        [TestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void TestResetAppStateLink(string username, string password)
        {
            LoginPage signin = Login(username, password);

            ProductPage product = new ProductPage(driver);
            //clicks on bike light and adds to cart
            product.BikeLight.Click();
            product.AddToCart.Click();
            //selects cart icon. 
            product.CartIcon.Click();
            //asserts item label 
            Assert.IsTrue(product.CartItemLabel.Text.Contains("Sauce Labs Bike Light"));
            //clicks on menu button and clicks on reset app state link
            product.OpenMenuButton.Click();
            Thread.Sleep(500);
            product.ResetAppLink.Click();
            //clicks on all items link and selects cart icon 
            product.AllItemsLink.Click();
            product.CartIcon.Click();
        }

        [TestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void TestAddItemToCart(string username, string password)
        {
            LoginPage signin = Login(username, password);

            ProductPage product = new ProductPage(driver);
            //asserting the product page title
            Assert.AreEqual("Swag Labs", driver.Title, "Title doesn't match");
            //selecting the product sort dropdown
            product.ProductSortContainer.Click();
            product.ProductSortHiLow.Click();
            Thread.Sleep(500);
            //selects the item and clicks on add to cart
            product.FleeceJacket.Click();
            product.AddToCart.Click();
            //selects cart icon and clicks on check out button. 
            product.CartIcon.Click();
            //asserts item label and clicks on check out button
            Assert.IsTrue(product.CartItemLabel.Text.Contains("Sauce Labs Fleece Jacket"));
        }

        [TestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void TestRemoveItemFromCart(string username, string password)
        {
            LoginPage signin = Login(username, password);

            ProductPage product = new ProductPage(driver);
            //asserting the product page title
            Assert.AreEqual("Swag Labs", driver.Title, "Title doesn't match");
            //selecting the product sort dropdown
            product.ProductSortContainer.Click();
            product.ProductSortLowHi.Click();
            Thread.Sleep(500);
            //selects the item and clicks on add to cart
            product.BikeLight.Click();
            product.AddToCart.Click();
            //selects cart icon and clicks on remove button. 
            product.CartIcon.Click();
            //asserts item label and clicks on remove button
            Assert.IsTrue(product.CartItemLabel.Text.Contains("Sauce Labs Bike Light"));
            product.RemoveItem.Click();
        }

        [TestMethod]
        [DataRow("standard_user", "secret_sauce")]
        public void TestCheckOutItem(string username, string password)
        {
            LoginPage signin = Login(username, password);

            ProductPage product = new ProductPage(driver);
            //asserting the product page title
            Assert.AreEqual("Swag Labs", driver.Title, "Title doesn't match");
            Thread.Sleep(500);
            //selects the item and clicks on add to cart
            product.BikeLight.Click();
            product.AddToCart.Click();
            //selects cart icon and clicks on chekc out button. 
            product.CartIcon.Click();
            product.CheckOutButton.Click();
            //calls check out method and asserts check out overview
            CheckOut("sammmmm", "johnnnnnn", "12344sdf");
            Assert.IsTrue(product.CheckOutOverviewInfo.Text.Contains("Checkout: Overview"));
            //clicks on finish button and asserts order finished text
            product.FinishButton.Click();
            Assert.AreEqual("THANK YOU FOR YOUR ORDER", product.OrderFinished.Text, "text doesn't match");
        }

        //helper method to check out
        private ProductPage CheckOut(string firstname, string lastname, string zip)
        {
            Thread.Sleep(500);
            ProductPage checkout = new ProductPage(driver);
            checkout.CheckOutFirstName.Clear();
            checkout.CheckOutFirstName.SendKeys(firstname);
            checkout.CheckOutLastName.Clear();
            checkout.CheckOutLastName.SendKeys(lastname);
            checkout.CheckOutZipcode.Clear();
            checkout.CheckOutZipcode.SendKeys(zip);
            checkout.CheckOutContinuwButton.Click();
            return checkout;
        }
    }//end of class
}
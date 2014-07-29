using System;
using MemberDemo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MemberDemo.Controllers;

namespace MemberDemo.Tests
{
    [TestClass]
    public class LoginUnitTest
    {
        AccountController controller = new MemberDemo.Controllers.AccountController();
        //[TestMethod]
        //public void Signin()
        //{
        //    var controller = new MemberDemo.Controllers.AccountController();
        //    var member = new LoginModel
        //    {
        //        UserName = "demo_SignupTest",
        //        Password = "12345"
        //    };
        //    var result = controller.Login(member);
        //    Assert.IsNotNull(result);
            
        //}

        [TestMethod]
        public void Signup()
        {
            var member = new SignupModel { 
                UserName="demo_SignupTest",
                Password="12345"
            };

            
            var result = controller.Signup(member);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Signup_UserName_MinlengthUnexpectd()
        {
            var member = new SignupModel { 
                UserName="aa",
                Password="aa"
            };
            controller.Signup(member);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void Signup_UserName_MaxlengthUnexpectd()
        {
            var member = new SignupModel
            {
                UserName = "aa",
                Password = "aa"
            };
            controller.Signup(member);
        }
    }
}

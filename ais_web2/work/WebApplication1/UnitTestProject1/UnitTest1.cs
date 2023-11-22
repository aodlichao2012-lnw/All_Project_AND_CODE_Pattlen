using ais_web3.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            FrmDetailController controller = new FrmDetailController();
            ChatHub chatHub = new ChatHub();
            chatHub.Get_Project("", "");
        }
    }
}

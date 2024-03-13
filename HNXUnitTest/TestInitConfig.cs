using CommonLib;
using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace HNXUnitTest
{
    [TestClass]
    public class TestInitConfig
    {
        #region Test Administrator Message
        private MessageFactoryFIX c_MsgFactory = new MessageFactoryFIX();

        [TestInitialize]
        public void Initialize()
        {
            InitObject.ReadConfigTest();
            Assert.AreNotEqual(ConfigData.AES_IV, string.Empty);
        }
        [TestMethod]
        public void TestAES()
        {
            string Key = "87c580ef1e5dfe7f89c3b869eb00c67c";
            string IV = "eb00c67cd56bd758";
            string CipherText = "123456";
            string Encrypt = Utils.EncryptAES(CipherText, Key, IV);
            string Decrypt = Utils.DecryptAES(Encrypt, Key, IV);
            Assert.AreEqual(CipherText, Decrypt);
            string Encrypt1 = Utils.EncryptAES(CipherText, Key, "");
            string Decrypt1 = Utils.DecryptAES(Encrypt, Key, "");
            Assert.AreEqual(Encrypt1, string.Empty);
            Assert.AreEqual(Decrypt1, string.Empty);
        }


        #endregion

        #region Test Bussiness Message
        [TestMethod]
        public void TestReadErrorCodeText()
        {
            ConfigData.ReadErrorCodeData("ErrorCodeText.csv");
            Assert.IsTrue(true);
        }
        
      
        #endregion
    }
}

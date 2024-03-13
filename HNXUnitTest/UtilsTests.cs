using CommonLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.WebSockets;

namespace HNXUnitTest
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void EncryptAESTest()
        {
            string AES_Key = "87c580ef1e5dfe7f89c3b869eb00c67c";
            string AES_IV = "eb00c67cd56bd758";

            string _dataraw = "nguyễn văn a 1990";

            string _encryptdata = Utils.EncryptAES(_dataraw, AES_Key, AES_IV);
            string _decryptdata = Utils.DecryptAES(_encryptdata, AES_Key, AES_IV);

            Assert.AreEqual(_dataraw, _decryptdata);
        }


        [TestMethod()]
        public void EncryptAESTest_exception()
        {
            string AES_Key = "87c580ef1e5dfe7f89c3b869eb00c67cd";
            string AES_IV = "eb00c67cd56bd758";

            string _dataraw = "nguyễn văn a 1990";

            string _encryptdata = Utils.EncryptAES(_dataraw, AES_Key, AES_IV);
            string _decryptdata = Utils.DecryptAES(_encryptdata, AES_Key, AES_IV);

            Assert.AreEqual(_encryptdata, "");
            Assert.AreEqual(_decryptdata, "");
        }

        [TestMethod()]
        public void ParseIntTest()
        {
            string s = "5";

            var _return = Utils.ParseInt(s);
            Assert.AreEqual(5, _return);
        }

        [TestMethod()]

        public void ParseLongSpanTest()
        {
            string s = "500";

            var _return = Utils.ParseLongSpan(s);
            Assert.AreEqual(500, _return);
        }

        [TestMethod()]
        [DataRow("20230103", true)]
        [DataRow("20232103", false)]
        public void Validator_Data_DateTest(string p_value, bool p_Exp)
        {
            var _return = Utils.Validator_Data_Date(p_value);
            Assert.AreEqual(p_Exp, _return);
        }

        [TestMethod()]
        public void GenOrderNoTest()
        {
            var _return = Utils.GenOrderNo( );
            Assert.IsTrue (_return !="");
        }

        [TestMethod()]
        public void FileHelper_Test()
        {
            FileHelper _FileHelper = new FileHelper("REV.txt", "test");
            _FileHelper.ReadAllData();
            _FileHelper.Dispose();
            Assert.IsTrue(true);
        }


		[TestMethod()]

		public void CommonLibTest()
		{
            VaultDataResponse _VaultDataResponse = new VaultDataResponse();
            _VaultDataResponse.MemberPass = "1234";
            string gMemberpass = _VaultDataResponse.MemberPass;

            SeqFileHelper _SeqFileHelper = new SeqFileHelper();
            _SeqFileHelper.Readdata(0);

			_SeqFileHelper.Dispose();

			var myData = new
			{
				Host = @"sftp.myhost.gr",
				UserName = "my_username",
				Password = "my_password",
				SourceDir = "/export/zip/mypath/",
				FileName = "my_file.zip"
			};

			var _Serialize= JsonHelper.Serialize(myData);

            var stringJson = "{\"Roll\":110,\"name\":\"Alex\",\"courses\":[\"Math230\",\"Calculus1\",\"CS100\",\"ML\"]}";
            var _Deserialize = JsonHelper.Deserialize<object>(stringJson);

			Assert.IsTrue(true);
		}
	}
}
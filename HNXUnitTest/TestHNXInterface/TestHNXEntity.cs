using HNX.FIXMessage;
using HNXInterface;
using KafkaInterface;
using Moq;
using BusinessProcessResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace HNXUnitTest
{
   /* [TestClass]
    public class TestHNXEntity
    {

        

        [TestMethod]
        public void TestSendMessage()
        {
            //Setup
            Mock<IResponseInterface> mockProcessResponse = new Mock<IResponseInterface>();
            HNXTCPClient c_HNXEntity = new HNXTCPClient(mockProcessResponse.Object);

            //Act
            FIXMessageBase fMsg = new FIXMessageBase();

            c_HNXEntity.Send2HNX(fMsg);

            //Assert
            mockHNXTCPClient.Verify(helper => helper.Send2HNX(It.IsAny<FIXMessageBase>()), Times.Exactly(1));

        }
        [TestMethod]
        public void TestSendSecurityStatus()
        {
            //Setup
            mockHNXTCPClient.Reset();
            c_HNXEntity.c_HNXTCPClient = mockHNXTCPClient.Object;
            //Act
            c_HNXEntity.SendSecurityStatusRequest("XDCR12101");

            //Assert
            //mockHNXTCPClient.Verify(helper => helper.SendSecurityStatusRequest(It.IsAny<String>()), Times.Once);
            mockHNXTCPClient.Verify(helper => helper.Send2HNX(It.IsAny<MessageSecurityStatusRequest>()), Times.Once);

        }

        [TestMethod]
        public void TestSendTradingSessionStatus()
        {
            //Setup
            mockHNXTCPClient.Reset();
            c_HNXEntity.c_HNXTCPClient = mockHNXTCPClient.Object;

            //Act
            c_HNXEntity.SendTradingSessionRequest();

            //Assert
            mockHNXTCPClient.Verify(helper => helper.Send2HNX(It.IsAny<MessageTradingSessionStatusRequest>()), Times.Once);
        }

        [TestMethod]
        public void TestGetSeq()
        {
            //Setup
            var mockIHHNXTCPClient = new Mock<iHNXClient>();
            c_HNXEntity.c_HNXTCPClient = mockIHHNXTCPClient.Object;
            int seq = c_HNXEntity.Seq();
            mockIHHNXTCPClient.Verify(helper => helper.Seq(), Times.Once);

            mockIHHNXTCPClient.Reset();
            int Lastseq = c_HNXEntity.LastSeqProcess();
            mockIHHNXTCPClient.Verify(helper => helper.LastSeqProcess(), Times.Once);
        }

        [TestMethod]
        public void TestGetClientStatus()
        {
            var mockIHHNXTCPClient = new Mock<iHNXClient>();
            c_HNXEntity.c_HNXTCPClient = mockIHHNXTCPClient.Object;
            c_HNXEntity.ClientStatus();
            mockIHHNXTCPClient.Verify(helper => helper.ClientStatus(), Times.Once);
        }
        [TestMethod]
        public void TestSendLogout()
        {
            //Arrange
            var mock_iHNXEntity = new Mock<iHNXClient>();
            var test_HNXEntity = new HNXEntity();
            test_HNXEntity.c_HNXTCPClient = mock_iHNXEntity.Object;
            //Setup
            mock_iHNXEntity.Setup(helper => helper.ClientStatus()).Returns(enumClientStatus.DATA_TRANSFER);
            test_HNXEntity.Logout();
            mock_iHNXEntity.Verify(helper => helper.Logout(), Times.Once);
        }
    }*/
}

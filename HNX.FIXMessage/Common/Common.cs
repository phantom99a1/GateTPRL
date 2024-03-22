/* 
 * Project: FIXMessage
 * Author : Nguyen Nhat Linh – Navisoft.
 * Summary: Define common const and type for message FIX
 * Modification Logs:
 * DATE             AUTHOR      DESCRIPTION
 * --------------------------------------------------------
 * Jul 10, 2009  	Linh.Nguyen     Created
 */

using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HNX.FIXMessage
{
    public class Common
    {
        public const char SOH = (char)0x01; //phan cach giua cac field
        public const string VERSION = "FIX.4.4";
        public const char DELIMIT = '='; //phan cach giua tag va gia tri

        //hh: 12h, HH 24h
        public static CultureInfo CULTURE_PROVIDER = new CultureInfo("en-US");
        public const string FORMAT_DATE = "yyyyMMdd";
        public const string FORMAT_DATETIME = "yyyyMMdd-HH:mm:ss";
        public const string FORMAT_DATETIME_LONG = "yyyyMMdd-HH:mm:ss.ffffff";

        //tag for body
        public const string HNX_TargetCompID = "HNX";
        public const string HNX_TargetSubID = "HNX.LISTED";
    }

    public class MessageType
    {
        public const string Heartbeat = "0"; // ACK
        public const string TestRequest = "1"; // Ping
        public const string ResendRequest = "2"; // Y/C gui mot doan du lieu mat
        public const string Reject = "3"; // Tu troi
        public const string SequenceReset = "4"; // Yeu cau dong bo Sequence
        public const string Logon = "A"; // LogIn
        public const string Logout = "5"; //LogOut

        public const string CrossOrderAdjustRequest = "t";//sua lenh thoa thuan
        //cho thong tin thi truong va danh sach chung khoan
        public const string TradingSessionStatusRequest = "g"; // yeu cau trang thai thi truong
        public const string TradingSessionStatus = "h"; // tra loi trang thai thi truong
        public const string SecurityStatusRequest = "e"; // dat lenh thoa thuan
        public const string SecurityStatus = "f"; // tra loi danh sach chung khoan
        public const string TopicTradingInfomation = "MN"; 

        public const string ExecutionReport = "8"; // Message tra ve cua lenh.
        public const string UserRequest = "BE"; // yeu cau của CTCK(thay doi mat khau)
        public const string UserResponse = "BF"; // tra loi yeu cau CTCK

        // trading 
        public const string ExecOrder = "E"; //khop lenh -- dùng cho lệnh khớp Outright
        public const string NewOrder = "D"; //Lệnh đặt gửi vào hệ thống
        public const string CancelOrder = "F"; //Lệnh hủy gửi vào hệ thống
        public const string ReplaceOrder = "G"; //Lệnh sửa gửi vào hệ thống

        public const string NewOrderCross = "s"; //Lệnh thỏa thuận gửi vào hệ thống -> lenh BCGD Outright
        public const string CrossOrderCancelRequest = "u"; //Lệnh hủy thỏa thuận gửi vào hệ thống 
        public const string CrossOrderCancelReplaceRequest = "t";// 
        public const string ReportOrderCross = "PT";

        public const string Quote = "S";// Lệnh quote --> thoa thuan dien tu
        public const string QuoteResponse = "AJ";//  --> Chap nhân thoa thuan dien tu
        public const string QuoteRequest = "R";//   --> Sưa thoa thuan dien tu
        public const string QuoteCancel = "Z";//  -->   Huy thoa thuan dien tu
        public const string QuoteStatusReport = "AI";//  --> Report for Quote Orders

        public const string ReposInquiry = "N01";
        public const string ReposInquiryReport = "N02";
        public const string ReposFirm = "N03";
        public const string ReposFirmReport = "N04";    //Report đối với lệnh firm
        public const string ReposFirmAccept = "N05";    //Chấp nhận firm
        public const string ReposFirmDTHModify = "N06";    //Sửa Firm đã thực hiện
        public const string ReposFirmDTHCancel = "N07";    //Hủy FIrm đã thực hiện

        public const string ReposBCGD = "MA";           //đặt lệnh báo cáo giao dịch repos
        public const string ReposBCGDCancel = "MC";      //Hủy lệnh bcgd repos
        public const string ReposBCGDModify = "ME";     //Sửa lệnh bao cáo gd repos
        public const string ReposBCGDReport = "MR";     //Thông báo của lệnh bcgd

        public const string HoldOrders = "HO";     //Thông báo treo bỏ treo lenh Outright
        public const string HoldOrdersRepos = "HR";     //Thông báo treo bỏ treo lenh Repos
        public const string ExecOrderRepos = "EE"; //khop lenh -- dùng cho lệnh khớp Repos
    }

    public class ExecutionReportType
    {
        public const char ER_Order_0 = '0'; // Lenh da vao san
        public const char ER_CancelOrder_4 = '4'; // Tra ve lenh huy
        public const char ER_ReplaceOrder_5 = '5'; // Tra ve lenh sua
        public const char ER_ExecOrder_3 = '3'; // Tra ve lenh khop
        public const char ER_ExecCrossOrder_F = 'F'; // Tra ve lenh khop thoa thuan
        public const char ER_CrossOrder_G = 'G'; // Lenh thoa thuan
        public const char ER_CancelCrossOrder_H = 'H'; // Tra ve lenh huy thoa thuan
        public const char ER_Rejected_8 = '8'; // tra ve thong tin tu choi lenh
    }

    public class enuSide
    {
        public const char Buy = '1';
        public const char Sell = '2';
        public const char Cross = '8'; //lenh thoa thuan
        //public const char CrossShort = '9'; //lenh confirm cho lenh thoa thuan,ap dung cho lenh thoa thuan 2firm
        public const char AsDefined = 'B'; //lenh quang cao
    }

    public class enuAdvSide
    {
        public const char Buy = 'B';
        public const char Sell = 'S';

    }
  

    public class ErrorCode
    {
        //cac ly do reject
        public const int REJ_NOT_LOGIN = -70001; //Chua dang nhap, khong the gui cac message khac!
        //public const int REJ_LOGINED = -70002; // Logined. Can't login again!
        //public const int REJ_LOGOUTNED = -70003; //  Logouted. Can't logout again 
        //public const int REJ_INVALID_USER_PASS = -70004; // Login Wrong Username or Password
        //public const int REJ_INVALID_MSGORDER = -70005;// message đặt lệnh không hợp lệ
        //public const int REJ_INVALID_MSGORDERCANCEL = -70006;// message hủy lệnh không hợp lệ
        //public const int REJ_INVALID_MSGORDERREPLACE = -70007;// message sửa lệnh không hợp lệ
        //public const int REJ_UNSUPPORT_MSGTYPE = -70008;//loại message không được hỗ trợ
        //public const int REJ_NOT_FIXMESSAGE = -70009;//Không phải là message FIX
        //public const int REJ_INVALID_PORT = -70010;//Gửi sai port
        //public const int REJ_INVALID_SENDER = -70011;//sai ten người gửi
        //public const int REJ_NOTEXIST_SENDER = -70012;//người gửi không có trong hệ thống
        //public const int REJ_OVER_BUFFERSIZE = -70013;//vuot qua buffersize
        //public const int REJ_STOCK_NOTEXIST = -70014;//ma chứng khoán không tồn tại
        ////
        //public const int REJ_INVALID_RSACODE = -70015; // Sai rsa code
        //                                               //

        //public const int REJ_INVALID_USERREQUEST = -70016; //Sai hinh thuc yeu cau cua User
        //public const int REJ_INVALID_IP = -70017; //Sai IP

        //public const int REJ_USER_STOP = -70020; // User da bi tam dung
        //public const int REJ_USER_STOPODER = -70021; // User da bi tam dung gưi lệnh


    }

    public class enumQuoteType
    {
        public const int NewQuote = 1;
        public const int ReplaceQuote = 2;
        public const int CancelQuote = 3;
        public const int Accept = 4;
    }
    
}

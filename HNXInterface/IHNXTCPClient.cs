﻿using HNX.FIXMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNXInterface
{
    public interface iHNXClient
    {
        public bool StartHNXTCPClient();

        public void Logout();
        public void StopCurrentConnected(string p_Reason);

        public bool Send2HNX(FIXMessageBase fMsgBase);

        public enumClientStatus ClientStatus();
        public int Seq();
        public int LastSeqProcess();
        public bool SendSecurityStatusRequest(string tradingCode, string Symbol);

        public int LastMapSeqProcess { get; }

        public bool SendTradingSessionRequest(string tradingCode,string tradingName);

        public bool SendUserRequest(string userName, string oldPass, string newPass);

        public void Recovery();

    }
}

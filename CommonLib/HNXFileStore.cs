using Disruptor;
using Disruptor.Dsl;
using HNX.FIXMessage;

namespace CommonLib
{
	public class HNXFileStore : IEventHandler<MessageStoreEvent>, IDisposable
	{
		private MessageFactoryFIX c_MsgFactoryFIX;
		private FileHelper FileSendStore;
		private FileHelper FileRecvStore;
		private FileHelper FileRecoStore;
		private SeqFileHelper SeqMapFile;

		private Disruptor<MessageStoreEvent> c_Disruptor;
		private RingBuffer<MessageStoreEvent> c_RingBuffer;

		public HNXFileStore()
		{
			VaultSetting.Token = ""; // reset Token Vault mỗi khi app đc khởi tạo lại
									 //
			c_MsgFactoryFIX = new MessageFactoryFIX();
			FileSendStore = new FileHelper("SEND_EXCHANGE", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));
			FileRecvStore = new FileHelper("RECV_EXCHANGE", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));
			FileRecoStore = new FileHelper("RECOVER_STORE", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));
			SeqMapFile = new SeqFileHelper("SEQMAP", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));
			c_Disruptor = new Disruptor<MessageStoreEvent>(() => new MessageStoreEvent(), 2048, TaskScheduler.Default, ProducerType.Multi, ConfigData.StrategyMode);
			c_RingBuffer = c_Disruptor.RingBuffer;
			c_Disruptor.HandleEventsWith(this);
			c_Disruptor.Start();
		}

		public void StoreSendMsg(string MessageType, string Message, long CliSeq, long CliLastProcess, long SerSeq, long SerLastProcess, long OrgSeq = 0)
		{
			EnqueueData(MessageType: MessageType, Message: Message, CliSeq: CliSeq, CliLastProcess: CliLastProcess, SerSeq: SerSeq, SerLastProcess: SerLastProcess, OrgSeq: OrgSeq, Flag: MessageFlag.SEND_FLAG);
		}

		public void StoreResendMsg(string MessageType, string Message, long CliSeq, long CliLastProcess, long SerSeq, long SerLastProcess)
		{
			EnqueueData(MessageType: MessageType, Message: Message, CliSeq: CliSeq, CliLastProcess: CliLastProcess, SerSeq: SerSeq, SerLastProcess: SerLastProcess, Flag: MessageFlag.RESEND_FLAG);
		}

		public void StoreRecvMsg(string MessageType, string Message, long CliSeq, long CliLastProcess, long SerSeq, long SerLastProcess)
		{
			EnqueueData(MessageType: MessageType, Message: Message, CliSeq: CliSeq, CliLastProcess: CliLastProcess, SerSeq: SerSeq, SerLastProcess: SerLastProcess, Flag: MessageFlag.RECV_FLAG);
		}

		public void StoreRecoverMsg_GateSendHNX(string MessageType, string Message, int OrgSeq)
		{
			EnqueueData(MessageType: MessageType, Message: Message, OrgSeq: OrgSeq, Flag: MessageFlag.RECOVER_FWD_FLAG);
		}

		//public void StoreRecoverMsg_GateFwdKafka(string MessageType, string Message, int OrgSeq)
		//{
		//    EnqueueData(MessageType: MessageType, Message: Message, OrgSeq: OrgSeq, Flag: MessageFlag.RECOVER_RSP_FLAG);
		//}

		public void EnqueueData(string MessageType, string Message, long CliSeq = 0, long CliLastProcess = 0, long SerSeq = 0, long SerLastProcess = 0, long OrgSeq = 0, char Flag = MessageFlag.SEND_FLAG)
		{
			long sequence = c_RingBuffer.Next();
			try
			{
				// (2) Get and configure the event for the sequence
				c_RingBuffer[sequence].MessageType = MessageType;
				c_RingBuffer[sequence].Message = Message;
				c_RingBuffer[sequence].CliSeq = CliSeq;
				c_RingBuffer[sequence].CliLastProcess = CliLastProcess;
				c_RingBuffer[sequence].SerSeq = SerSeq;
				c_RingBuffer[sequence].SerLastProcess = SerLastProcess;
				c_RingBuffer[sequence].flag = Flag;
				c_RingBuffer[sequence].OrgSeq = OrgSeq;
			}
			finally
			{
				c_RingBuffer.Publish(sequence);
			}
		}

		public List<string> ReadAllSENDFile()
		{
			//c_Disruptor.Halt();
			List<string> result = FileSendStore.ReadAllData();
			//c_Disruptor.();
			return result;
		}

		public List<string> ReadAllREVFile()
		{
			List<string> result = FileRecvStore.ReadAllData();
			return result;
		}

		/// <summary>
		/// Lưu map Seq
		/// </summary>
		/// <param name="Seq"></param>
		/// <param name="Position">0: Seq Message gate chấp nhận và phản cho Kafka, 8: Seq Message Gate thông bảo gửi sở và trả về kakfa ,16: Seq Message Gate nhận từ sở và foward về kakfa</param>
		public void StoreMapSeq(long Seq, int Position)
		{
			SeqMapFile.WriteData(Seq, Position);
		}

		/// <summary>
		/// Recover lại Sequence
		/// </summary>
		/// <param name="Position">0: Seq Message gate chấp nhận và phản cho Kafka, 8: Seq Message Gate thông bảo gửi sở và trả về kakfa ,16: Seq Message Gate nhận từ sở và foward về kakfa</param>
		/// <returns></returns>
		public long RecoverMapSeq(int Position = 0)
		{
			return SeqMapFile.Readdata(Position);
		}

		public List<string> ReadAllRECOVERFile()
		{
			List<string> result = FileRecoStore.ReadAllData();
			return result;
		}

		public void OnEvent(MessageStoreEvent p_data, long sequence, bool endOfBatch)
		{
			string strCliSeq = p_data.CliSeq.ToString().PadLeft(10, '0');
			string strSerSeq = p_data.SerSeq.ToString().PadLeft(10, '0');
			string strCliLastProcess = p_data.CliLastProcess.ToString().PadLeft(10, '0');
			string strSerLastProcess = p_data.SerLastProcess.ToString().PadLeft(10, '0');
			//
			string msgType = p_data.MessageType;
			string messsage = p_data.Message;

			// mục đích để che pass trong log
			if (ConfigData.EnableMaskSensitiveData)
			{
				if (msgType == MessageType.Logon)
				{
					MessageLogon _MessageLogon = (MessageLogon)c_MsgFactoryFIX.Parse(messsage);
					_MessageLogon.Password = "******";
					//
					string buildMsgRaw = c_MsgFactoryFIX.Build(_MessageLogon);
					messsage = buildMsgRaw;
				}
				if (msgType == MessageType.UserRequest)
				{
					MessageUserRequest _MessageUserRequest = (MessageUserRequest)c_MsgFactoryFIX.Parse(messsage);
					_MessageUserRequest.Password = "******";
					_MessageUserRequest.NewPassword = "******";
					//
					string buildMsgRaw = c_MsgFactoryFIX.Build(_MessageUserRequest);
					messsage = buildMsgRaw;
				}
			}

			if (p_data.flag == MessageFlag.SEND_FLAG)
			{
				FileSendStore.WriteData($"N|{DateTime.Now.ToString("HH:mm:ss:fffffff")}|({strCliSeq},{strCliLastProcess})|({strSerSeq},{strSerLastProcess})|{p_data.OrgSeq.ToString().PadLeft(10, '0')}|{messsage}");
			}
			else if (p_data.flag == MessageFlag.RECV_FLAG)
			{
				FileRecvStore.WriteData($"{DateTime.Now.ToString("HH:mm:ss:fffffff")}|({strSerSeq},{strSerLastProcess})|({strCliSeq},{strCliLastProcess})|{messsage}");
			}
			else if (p_data.flag == MessageFlag.RESEND_FLAG)
			{
				FileSendStore.WriteData($"R|{DateTime.Now.ToString("HH:mm:ss:fffffff")}|({strCliSeq},{strCliLastProcess})|({strSerSeq},{strSerLastProcess})|{messsage}");
			}
			else
			{
				//Còn lại sẽ là message để Recover
				FileRecoStore.WriteData($"{p_data.flag}|{p_data.OrgSeq.ToString().PadLeft(10, '0')}|{messsage}");
			}
		}

		public void Dispose()
		{
			if (c_Disruptor.HasStarted)
				c_Disruptor.Shutdown();
			FileSendStore.Dispose();
			FileRecvStore.Dispose();
			FileRecoStore.Dispose();
		}
	}

	public class MessageStoreEvent
	{
		public string MessageType { get; set; }
		public string Message { get; set; }
		public long CliSeq { get; set; }
		public long CliLastProcess { get; set; }
		public long SerSeq { get; set; }
		public long SerLastProcess { get; set; }
		public char flag { get; set; }
		public long OrgSeq { get; set; }

		public MessageStoreEvent()
		{
		}
	}

	public class MessageFlag
	{
		public const char RECV_FLAG = 'R';
		public const char SEND_FLAG = 'S';
		public const char RESEND_FLAG = 'r';
		public const char RECOVER_RSP_FLAG = 'C';//Là Cờ báo message gửi từ Gate ra Kafka(chấp nhận và báo gửi sở)
		public const char RECOVER_FWD_FLAG = 'c'; //Là cờ báo message được gate gửi sở
	}
}
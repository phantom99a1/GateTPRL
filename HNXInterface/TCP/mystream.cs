using CommonLib;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HNXInterface
{
    public class MyStream : IDisposable
    {
        private const char SOH = (char)0x01; //phan cach giua cac field
        private const char DELIMIT = '='; //phan cach giua tag va gia tri

        //
        private const string BEGIN_TAG = "8=FIX.4.4";

        private readonly string LENGTH_TAG = SOH + "9=";
        private readonly string CHECKSUM_TAG = SOH + "10=";
        private object objLock4SND = new object();
        private NetworkStream c_stream;
        private BinaryWriter c_BinaryWriter;
        private TcpClient c_TcpClient;
        //
        private bool StartBytesReadzero = false;

        private long TimeStartBytesReadzero = DateTime.Now.Ticks;

        //
        private string _streamkey;

        private bool _isAvaiable = false;
        private string c_LocalIP;
        private string c_LocalPort;

        public bool isAvaiable
        {
            get { return _isAvaiable; }
        }

        public string LocalIP
        {
            get { return c_LocalIP; }
        }

        public string LocalPort
        {
            get { return c_LocalPort; }
        }

        public string streamkey
        {
            get { return _streamkey; }
        }

        /// <summary>
        /// hàm khởi tạo này chỉ dùng để giả lập unit test - không dùng khi code
        /// </summary>
        public MyStream()
        {
            _streamkey = DateTime.Now.ToString();
            _isAvaiable = true;
        }

        public MyStream(TcpClient _clientSocket, string p_key)
        {
            c_TcpClient = _clientSocket;
            c_stream = _clientSocket.GetStream();
            c_BinaryWriter = new BinaryWriter(c_stream);
            c_stream.ReadTimeout = 2 * (ConfigData.Heartbeat * 1000);// mặc định để 2 chu kì heartbeat,  ConfigData.Heartbeat tính bằng giây, nên phải chuyển sang minigiay

            IPEndPoint ipend = (IPEndPoint)_clientSocket.Client.LocalEndPoint;
            c_LocalIP = ipend.Address.ToString();
            c_LocalPort = ipend.Port.ToString();

            _streamkey = p_key;
            _isAvaiable = true;
        }

        public virtual void CloseStream()
        {
            _isAvaiable = false;
            c_BinaryWriter.Close();
            c_stream.Close();
            c_TcpClient.Close();
        }

        //Đọc theo cách của HNX gateway server
        private StringBuilder _incomingMessage = new StringBuilder();

        private static int DefaultBufferSize = CommonLib.ConfigData.DefaultBufferSize;
        private byte[] _Defaultbuffer = new byte[DefaultBufferSize];
        private int ValidBufferSize = DefaultBufferSize * 3;

        public virtual string ReadString()
        {
            // string ReturnString = "";
            int BytesRead = 0;
            int _count = 0;
            int IndexCheckSum;
            int lastmesssage;
            int BeginTagIndex;
            int LastSOH = 0;
            //DungNT- thử đoạn code mới này xem sao

            try
            {
                if (_incomingMessage.Length > 0)
                {//còn dư,  Đọc message theo cách 1
                    string MessageRaw = _incomingMessage.ToString();
                    BeginTagIndex = MessageRaw.IndexOf(BEGIN_TAG);
                    if (BeginTagIndex >= 0)
                    {
                        IndexCheckSum = MessageRaw.IndexOf(CHECKSUM_TAG, BeginTagIndex, StringComparison.Ordinal);

                        /* if (IndexCheckSum >= 0 && _incomingMessage.Length > IndexCheckSum + 4)
                         {
                             lastmesssage = IndexCheckSum + 7;//sau 3 kí tự IndexCheckSum sẽ là 3 ký tự check sum + SOH
                             _incomingMessage.Remove(0, lastmesssage);

                             return _data.Substring(BeginTagIndex, lastmesssage - BeginTagIndex + 4);
                         }*/
                        if (IndexCheckSum > 0)
                        {
                            LastSOH = MessageRaw.IndexOf(SOH, IndexCheckSum + 1);
                            if (LastSOH != -1)
                            {
                                //Cắt Message ở đây
                                _incomingMessage.Remove(0, LastSOH);
                                return MessageRaw.Substring(BeginTagIndex, LastSOH - BeginTagIndex + 1);
                            }
                        }
                    }
                }

                //ra đây thì có nghĩa là _incomingMessage chưa đủ 1 mesasge, đọc tiếp
                BytesRead = c_stream.Read(_Defaultbuffer, 0, DefaultBufferSize);
                if (BytesRead > 0)
                {
                    _incomingMessage.Append(Encoding.ASCII.GetString(_Defaultbuffer, 0, BytesRead));
                    StartBytesReadzero = false;
                    //Đọc message theo cách 2
                    string MessageRaw = _incomingMessage.ToString();
                    BeginTagIndex = MessageRaw.IndexOf(BEGIN_TAG);
                    if (BeginTagIndex != -1)
                    {
                        IndexCheckSum = MessageRaw.IndexOf(CHECKSUM_TAG, BeginTagIndex);
                        if (IndexCheckSum > 0)
                        {
                            LastSOH = MessageRaw.IndexOf(SOH, IndexCheckSum + 1);
                            if (LastSOH != -1)
                            {
                                //Cắt Message ở đây
                                _incomingMessage.Remove(0, LastSOH + 1);
                                return MessageRaw.Substring(BeginTagIndex, LastSOH - BeginTagIndex + 1);
                            }
                        }
                    }
                }
                else
                {
                    //KHi BytesRead =0, có thể do socket bị lỗi, nên sẽ đọc thêm mọt thời gian nữa rồi đóng
                    if (StartBytesReadzero == false)
                    {
                        StartBytesReadzero = true;
                        TimeStartBytesReadzero = DateTime.Now.Ticks;
                    }
                    else
                    {
                        if (DateTime.Now.Ticks - TimeStartBytesReadzero > 3 * TimeSpan.TicksPerSecond)
                        {
                            string remainstring = _incomingMessage.ToString();

                            if (remainstring == "") remainstring = "off";
                            _incomingMessage.Clear();
                            return remainstring;
                        }
                    }
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                Logger.HNXTcpLog.Error(ex);
                return "off";
            }
            return null; //ra đến đây là chưa có message đủ
        }

        public virtual bool WriteString(string p_str)
        {
            if (c_stream.CanWrite)
            {
                if ((p_str != null && p_str != ""))
                {
                    byte[] bufContent = Encoding.ASCII.GetBytes(p_str);

                    //send message
                    lock (objLock4SND)
                    {
                        c_BinaryWriter.Write(bufContent, 0, bufContent.Length);
                        c_BinaryWriter.Flush();
                    }

                    return true;
                }
                else return false;
            }
            else return false;
        }

        public virtual bool WriteByte(byte[] p_byte)
        {
            if (c_stream.CanWrite)
            {
                if (p_byte != null && p_byte.Length > 0)
                {
                    //send message
                    lock (objLock4SND)
                    {
                        c_BinaryWriter.Write(p_byte, 0, p_byte.Length);
                        c_BinaryWriter.Flush();
                    }

                    return true;
                }
                else
                    return false;
            }
            else return false;
        }

        public void Dispose()
        {
            c_BinaryWriter.Dispose();
        }
    }
}
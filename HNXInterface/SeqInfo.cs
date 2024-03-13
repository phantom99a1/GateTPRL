using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNXInterface
{
    public class SeqInfo
    {
      
        private int _CliSeq = 0;
      
        private int _SerSeq = 0;
        
        private int _LastCliProcessSeq = 0;
       
        private int _LastSerProcessSeq = 0;

        private int _MaxSeqProcess = 0;

        //Lưu giữ thông tin Seq, Last Process Seq của client
        //private SeqFileHelper CliSeqFileStore = new SeqFileHelper("GateSequence.seq", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));

        //private SeqFileHelper SerSeqFileStore = new SeqFileHelper("HNXSequence.seq", ConfigData.LogHNXDataPath + Path.DirectorySeparatorChar + DateTime.Now.ToString("yyyyMMdd"));//Lưu thông tin Seqm Last Process Seq của sở
        /// <summary>
        /// Sequence của Gate
        /// </summary>
        public int CliSeq
        {
            get 
            {
                return _CliSeq; 
            }
        }
        /// <summary>
        /// Sequence của sở
        /// </summary>
        public int SerSeq
        {
            get
            {
                return _SerSeq;
            }
        }
        /// <summary>
        /// Last Sequence Process của Cli
        /// </summary>
        public int LastCliProcessSeq
        {
            get
            {
                return _LastCliProcessSeq;
            }
        }
        /// <summary>
        /// Last Sequence Process của sở
        /// </summary>
        public int LastSerProcessSeq
        {
            get
            {
                return _LastSerProcessSeq;
            }
        }

        public void Set_CliSeq(int Seq)
        {
            _CliSeq = Seq;
            //CliSeqFileStore.WriteData(Seq, 0);
        }

        public void Set_LastCliProcess(int Seq)
        {
            _LastCliProcessSeq = Seq;
            //CliSeqFileStore.WriteData(Seq, 8);
        }

        public void Set_SerSeq(int Seq)
        {
            _SerSeq = Seq;
            //SerSeqFileStore.WriteData(Seq, 0);
        }

        public void Set_LastSerProcess(int Seq)
        {
            _LastSerProcessSeq = Seq;
            //SerSeqFileStore.WriteData(Seq, 8);
        }

        public void Set_MaxSeqProcess(int Seq)
        {
            _MaxSeqProcess = Seq;
        }
        public void RecoverSeq()
        {
            //_CliSeq = (int)CliSeqFileStore.Readdata(0);
            //_LastCliProcessSeq = (int)CliSeqFileStore.Readdata(8);
            //_SerSeq = (int)SerSeqFileStore.Readdata(0);
            //_LastSerProcessSeq = (int)SerSeqFileStore.Readdata(8);
            
        }

        public void RealoadSeq(int p_CliSeq = 0, int p_SerSeq = 0, int p_LastCliProcessSeq = 0, int p_LastSerProcessSeq = 0)
        {

        }

        public void CLose()
        {
            //CliSeqFileStore.Dispose();
            //SerSeqFileStore.Dispose();
        }
    }
}

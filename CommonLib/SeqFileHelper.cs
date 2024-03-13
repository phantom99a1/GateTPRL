using Microsoft.AspNetCore.SignalR.Protocol;
namespace CommonLib
{
    public class SeqFileHelper:IDisposable
    {
        private readonly BinaryWriter _streamWriter;
        private readonly BinaryReader binaryReader;
        private string Name = "";

        public SeqFileHelper()
        {
        }


        public SeqFileHelper(string p_ObjName, string p_PathFile)
        {
            try
            {
                var _pathName = p_PathFile + Path.DirectorySeparatorChar + p_ObjName;
                Name = p_ObjName;
                if (!Directory.Exists(p_PathFile))
                    Directory.CreateDirectory(p_PathFile);
                var fileStream = File.Open(_pathName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                fileStream.Seek(0, SeekOrigin.Begin);
                _streamWriter = new BinaryWriter(fileStream);
                binaryReader = new BinaryReader(fileStream);
                if (fileStream.Length == 0)
                {
                    WriteData(0, 0);
                    WriteData(0, 8);
                    WriteData(0, 16);
                }    
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, "HNXTPRL|IO: SeqFileHelper name {0} error", p_ObjName, ex.Message);
            }
        }


        public void WriteData(long num1, int position)
        {
            try
            {
                _streamWriter.BaseStream.Seek(position * sizeof(int), SeekOrigin.Begin);
                _streamWriter.Write(num1);
                _streamWriter.Flush();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, "HNXTPRL|IO: SeqFileHelper name {0} error {1}", Name,ex.Message);
            }
        }

        public long Readdata(int position)
        {
            try
            {
                binaryReader.BaseStream.Seek(position * sizeof(int), SeekOrigin.Begin);
                return binaryReader.ReadInt64();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex,"HNXTPRL|IO: {0}", ex.Message);
                return 0;
            }
        }

        public void Dispose()
        {
            try
            {
                if (_streamWriter != null)
                {
                    _streamWriter.Dispose();                   
                    binaryReader.Dispose();
                }    
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, "HNXTPRL|IO: {0}", ex.Message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommonLib
{
    public class FileHelper : IDisposable
    {

        #region parameter

        private string c_PathName;
        private string c_FileName;
        private StreamWriter c_StreamWriter;
        private System.IO.FileStream c_FileStream;

        //
        #endregion

        #region Contructor

        public FileHelper(string p_ObjName, string p_PathFile)
        {
            try
            {
                c_FileName = p_ObjName;
                c_PathName = p_PathFile + Path.DirectorySeparatorChar + p_ObjName + ".dat";
                if (!Directory.Exists(p_PathFile))
                    Directory.CreateDirectory(p_PathFile);

                c_FileStream = System.IO.File.Open(c_PathName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite);
                c_FileStream.Seek(0, SeekOrigin.End);

                c_StreamWriter = new StreamWriter(c_FileStream);

                //c_StreamReader = new StreamReader(c_PathName, true);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }
        public long Position
        {
            get
            {
                return c_FileStream.Position;
            }
        }

        #endregion

        #region Public Method
        public void WriteData(string strData)
        {
            try
            {
                c_StreamWriter.WriteLine(strData);
                c_StreamWriter.Flush();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        public List<string> ReadAllData()
        {
            List<string> _result = new List<string>();
            try
            {
                if (System.IO.File.Exists(c_PathName))
                {
                    System.IO.FileStream _FileStream = System.IO.File.Open(c_PathName, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
                    StreamReader _StreamReader = new StreamReader(_FileStream);
                    while (!_StreamReader.EndOfStream)
                    {
                        string s = _StreamReader.ReadLine();
                        if (s.Length > 5) _result.Add(s);
                    }
                    _StreamReader.Close();
                    _FileStream.Close();

                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
            return _result;
        }

        public void Dispose()
        {
            try
            {
                if (c_FileStream != null)
                {
                    c_StreamWriter.Close();
                    c_FileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex);
            }
        }

        #endregion


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
namespace LocalMemory
{
    public class ShareMemoryData
    {
        public static HNXFileStore c_FileStore = new HNXFileStore();
        public static int c_UserStatus { get; set; } = -1;
        public static string c_UserStatusText { get; set; } = "";
        public static string c_LoginStatus { get; set; } = "";
    }
}

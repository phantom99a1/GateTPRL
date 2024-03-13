using BusinessProcessResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessProcessAPIReq
{
    public partial class ProcessRevBusiness: IProcessRevBussiness
    {
        private readonly IProcessRevEntity c_IProcessRevEntity;

        public ProcessRevBusiness(HNXInterface.iHNXClient p_iHNXEntity, IResponseInterface p_ResponseInterface)
        {
            //   c_IProcessRevEntity = _processRevEntity;
            c_IProcessRevEntity = new ProcessRevEntity(p_iHNXEntity, p_ResponseInterface);
        }

        public (long, long) ItemsInQueue()
        {
            return c_IProcessRevEntity.ItemsInQueue();
        }

        public void StopReceiveApi()
        {
            c_IProcessRevEntity.StopProducer();
        }

        public void RecoverData()
        {
            c_IProcessRevEntity.RecoverData();          
        }
    }
}

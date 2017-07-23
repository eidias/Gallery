using Gallery.Library.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Library.Services
{
    public class EntityService
    {
        readonly AppContext appContext;

        public EntityService(AppContext appContext)
        {
            this.appContext = appContext;
        }

    }
}

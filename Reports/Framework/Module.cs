using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    public abstract class Module
    {
        public abstract void RegisterServices(Registrator registrator);
    }
}

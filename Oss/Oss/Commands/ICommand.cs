using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Commands
{
    internal interface  ICommand<T>
    {
          Task<T> Execute();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oss.Deserial
{
    internal interface IDeserializer<TInput, TOutput>
    {
        TOutput Deserialize(TInput input);
    }
}

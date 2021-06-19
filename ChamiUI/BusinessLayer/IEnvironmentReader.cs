using System.Collections;
using System.Collections.Generic;

namespace ChamiUI.BusinessLayer
{
    public interface IEnvironmentReader<TOut>
    {
        TOut Process();
        ICollection<TOut> ProcessMultiple();
    }
}
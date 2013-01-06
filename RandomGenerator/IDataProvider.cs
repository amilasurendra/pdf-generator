using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomGenerator
{
    public interface IDataProvider
    {
       string GetSampleData(String featureName);
    }
}

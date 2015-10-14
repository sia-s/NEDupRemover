using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrgNameDupRemover
{
    public class JaroWinklerDistance : ITokenDistance
    {
        public double Score(Token source, Token target)
        {
            var d = new SimMetricsMetricUtilities.JaroWinkler();
            return d.GetSimilarity(source.Value, target.Value);            
        }
    }
}

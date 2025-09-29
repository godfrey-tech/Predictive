using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Modals
{
    public class MatchPrediction
    {
        [ColumnName("PredictedLabel")]
        public uint Outcome { get; set; } // Predicted outcome
    }
}

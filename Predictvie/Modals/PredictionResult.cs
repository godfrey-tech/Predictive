using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Predictvie.Modals
{
    public class PredictionResult
    {
        [ColumnName("Score")]
        public float PredictedCorners { get; set; }

        [ColumnName("PredictedLabel")]
        public float PredictedBookings { get; set; }
    }
}

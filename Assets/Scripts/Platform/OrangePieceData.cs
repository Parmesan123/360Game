using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public struct OrangePieceData
{
    public float StartAngle { get; private set; }
    public float EndAngle { get; private set; }

    public OrangePieceData(float startAngle, float endAngle)
    {
        StartAngle = startAngle;
        EndAngle = endAngle;
    }
}


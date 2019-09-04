using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorGraphicViewer.ServiceFoundation.Enum
{
    public enum  LineTypeEnum
    {
        [Description("Line is drawn solid type")]
        Solid = 1,

        [Description("Line is drawn dot type")]
        Dot = 2,

        [Description("Line is drawn dash type")]
        Dash = 3,

        [Description("Line is drawn DashDot type")]
        DashDot = 4,

        
    }
}


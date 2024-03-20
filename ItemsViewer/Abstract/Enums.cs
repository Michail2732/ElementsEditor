using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsViewer.Abstract
{
    public enum AccessRights
    {
        Read = 0b001,
        Write = 0b011,
        All = 0b111
    }

    public enum ElementState
    {
        None,        
        Modified,
        New,
        Removed
    }

    public enum ConditionOperation
    {
        Equals,
        NotEquals,
        Large,
        LargeOrEquals,
        Less,
        LessOrEquals,
        Contains,
        StartWith,
        EndWith,
    }

    public enum Logic
    {
        Or,
        And
    }

    public enum ValueType
    {
        String,
        Integer,
        Double,
        Decimal,
        Boolean,
        DateTime        
    }
}

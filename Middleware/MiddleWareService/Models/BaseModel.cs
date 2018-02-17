using PublicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiddleWareService.Models
{
    public class RangeMode
    {
        Enums.业务类型 _Btype = Enums.业务类型.热库_存活库;

        public Enums.业务类型 Btype
        {
            get { return _Btype; }
            set { _Btype = value; }
        }

        int _RangeEnd = 0;

        public int RangeEnd
        {
            get { return _RangeEnd; }
            set { _RangeEnd = value; }
        }

        int _RangeStart = 0;

        public int RangeStart
        {
            get { return _RangeStart; }
            set { _RangeStart = value; }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoWPF
{
    public class BaseVirtual
    {
        public virtual string F()
        {
            return string.Format("{0}:{1}", "BaseVirtural", "F()");
        }

        public virtual string G()
        {
            return string.Format("{0}:{1}", "BaseVirtural", "G()");
        }
    }

    public class Sansum : BaseVirtual
    {
    }

    public class Apple : BaseVirtual
    {
        public virtual string F()
        {
            return string.Format("{0}:{1}", "Apple", "F()");
        }

        public virtual string G()
        {
            return string.Format("{0}:{1}", "Apple", "G()");
        }
    }

    public class HTC : BaseVirtual
    {
        public override string F()
        {
            return string.Format("{0}:{1}", "HTC", "F()");
        }

        public override string G()
        {
            return string.Format("{0}:{1}", "HTC", "G()");
        }
    }
}

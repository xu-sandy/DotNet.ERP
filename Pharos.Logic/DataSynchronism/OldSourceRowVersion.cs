using Pharos.Logic.DataSynchronism;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.DataSynchronism
{
    public class OldSourceRowVersion : ITarget
    {
        public byte[] SourceRowVersion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.Entity
{
    public class BatchTranEntity
    {
        public CommandEnum CmdType { get; set; }
        public Type EntityType { get; set; }
        public IEnumerable<object> Entities { get; set; }

        public BatchTranEntity(CommandEnum cmdType,IEnumerable<object> entities)
        {
            CmdType = cmdType;
            Entities = entities;
        }
        public BatchTranEntity(CommandEnum cmdType, IEnumerable<object> entities, Type entityType)
        {
            CmdType = cmdType;
            Entities = entities;
            EntityType = entityType;
        }
    }
}

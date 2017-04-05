using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pharos.SyncService.Exceptions;

namespace Pharos.SyncService.SyncEntities
{
    [Serializable]
    public class SyncDataObject : ISyncDataObject, IComparable
    {
        public SyncDataObject()
        {
            EntityType = this.GetType().ToString();
        }
        public Guid SyncItemId { get; set; }

        public byte[] SyncItemVersion { get; set; }

        public string EntityType { get; set; }
        public int CompareTo(object obj)
        {
            var temp = obj as ISyncDataObject;
            return BitConverter.ToUInt64(this.SyncItemVersion, 0).CompareTo(BitConverter.ToUInt64(temp.SyncItemVersion, 0));
        }
    }
    public class SyncDataPackageItem : SyncDataObject, IComparable
    {
    }
    public class SyncDataPackage : ISyncDataObject
    {
        public SyncDataPackage()
        {
            Items = new List<SyncDataPackageItem>();
        }
        public List<SyncDataPackageItem> Items { get; set; }

        public Guid SyncItemId { get; set; }
        private byte[] syncItemVersion;
        public byte[] SyncItemVersion
        {
            get
            {
                if (Items.Count == 0)
                {
                    throw new SyncException("Items不能为空！");

                }
                if (syncItemVersion == null)
                {
                    syncItemVersion = Items.Max().SyncItemVersion;
                }
                return syncItemVersion;
            }
            set
            {
                // throw new SyncException("无法设置SyncItemVersion");
            }
        }


        public string EntityType { get; set; }
    }
}

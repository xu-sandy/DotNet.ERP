using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Events.Attributes
{
    /// <summary>
    /// Represents that the event handlers applied with this attribute
    /// will handle the events in a asynchronous process.
    /// [异步处理特性,某些处理过程可能需要花费较长的时间，并且业务逻辑的继续执行并不需要得知其处理结果，那么就可以在这个事件处理器应用这个特性]
    /// </summary>
    /// <remarks>This attribute is only applicable to the message handlers and will only
    /// be used by the message buses or message dispatchers. Applying this attribute to
    /// other types of classes will take no effect.</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class HandlesAsynchronouslyAttribute : Attribute
    {

    }
}

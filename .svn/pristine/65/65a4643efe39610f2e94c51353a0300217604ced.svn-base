using System;

namespace Pharos.SocketService.Retailing.Extensions
{
    public static class ArrayExtensions
    {
        public static byte[] CloneRange(this byte[] source, int offset, int length)
        {
            var result = new byte[length];
            Array.Copy(source, offset, result, 0, length);
            return result;
        }
    }
}

using SuperSocket.Common;
using SuperSocket.SocketBase.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.SocketService.Retailing.Protocol
{
    /// <summary>
    /// 根据开始结果分隔符组包
    /// </summary>
    /// <typeparam name="TRequestInfo">组包后转化成Request对象</typeparam>
    public abstract class BaseDataSyncReceiveFilter<TRequestInfo> : ReceiveFilterBase<TRequestInfo> where TRequestInfo : IRequestInfo
    {
        /// <summary>
        /// 数据包起始标识
        /// </summary>
        private readonly SearchMarkState<byte> m_BeginSearchState;
        /// <summary>
        /// 数据包结束标识
        /// </summary>
        private readonly SearchMarkState<byte> m_EndSearchState;

        /// <summary>
        /// TRequestInfo Null
        /// </summary>
        protected TRequestInfo NullRequestInfo = default(TRequestInfo);
        /// <summary>
        /// 是否已找到起始标识
        /// </summary>
        private bool m_FoundBegin = false;


        public BaseDataSyncReceiveFilter(byte[] beginMark, byte[] endMark)
        {
            m_BeginSearchState = new SearchMarkState<byte>(beginMark);
            m_EndSearchState = new SearchMarkState<byte>(endMark);
        }
        /// <summary>
        /// 该方法将会在 SuperSocket 收到一块二进制数据时被执行，接收到的数据在 readBuffer 中从 offset 开始， 长度为 length 的部分
        /// </summary>
        /// <param name="readBuffer">接收缓冲区, 接收到的数据存放在此数组里</param>
        /// <param name="offset">接收到的数据在接收缓冲区的起始位置</param>
        /// <param name="length">本轮接收到的数据的长度</param>
        /// <param name="toBeCopied">表示当你想缓存接收到的数据时，是否需要为接收到的数据重新创建一个备份而不是直接使用接收缓冲区</param>
        /// <param name="rest">这是一个输出参数, 它应该被设置为当解析到一个为政的请求后，接收缓冲区还剩余多少数据未被解析</param>
        /// <returns>组包后转化成Request对象</returns>
        public override TRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            int searchEndMarkOffset;
            int searchEndMarkLength;
            //上一个匹配结果
            int prevMatched = 0;
            if (!m_FoundBegin)
            {
                prevMatched = m_BeginSearchState.Matched;
                int pos = readBuffer.SearchMark(offset, length, m_BeginSearchState);

                if (pos < 0)
                {
                    //不用缓存无效数据
                    if (prevMatched > 0 || (m_BeginSearchState.Matched > 0 && length != m_BeginSearchState.Matched))
                    {
                        State = FilterState.Error;
                    }
                    return NullRequestInfo;
                }
                //起始标识符错位
                if (pos != offset)
                {
                    State = FilterState.Error;
                    return NullRequestInfo;
                }

                //标识已找到起始标识符
                m_FoundBegin = true;

                searchEndMarkOffset = pos + m_BeginSearchState.Mark.Length - prevMatched;

                //这个数据块只包含起始标识
                if (offset + length <= searchEndMarkOffset)
                {
                    AddArraySegment(m_BeginSearchState.Mark, 0, m_BeginSearchState.Mark.Length, false);
                    return NullRequestInfo;
                }

                searchEndMarkLength = offset + length - searchEndMarkOffset;
            }
            else//已经找到起始标识
            {
                searchEndMarkOffset = offset;
                searchEndMarkLength = length;
            }

            while (true)
            {
                var prevEndMarkMatched = m_EndSearchState.Matched;
                var endPos = readBuffer.SearchMark(searchEndMarkOffset, searchEndMarkLength, m_EndSearchState);

                //还没找到结束标识
                if (endPos < 0)
                {
                    rest = 0;
                    if (prevMatched > 0)//缓存上一个开始标识数据块
                        AddArraySegment(m_BeginSearchState.Mark, 0, prevMatched, false);
                    AddArraySegment(readBuffer, offset, length, toBeCopied);
                    return NullRequestInfo;
                }

                //找到结束标识
                int parsedLen = endPos - offset + m_EndSearchState.Mark.Length - prevEndMarkMatched;
                rest = length - parsedLen;

                byte[] commandData = new byte[BufferSegments.Count + prevMatched + parsedLen];

                if (BufferSegments.Count > 0)
                    BufferSegments.CopyTo(commandData, 0, 0, BufferSegments.Count);

                if (prevMatched > 0)
                    Array.Copy(m_BeginSearchState.Mark, 0, commandData, BufferSegments.Count, prevMatched);

                Array.Copy(readBuffer, offset, commandData, BufferSegments.Count + prevMatched, parsedLen);

                var requestInfo = ProcessMatchedRequest(commandData, 0, commandData.Length);

                if (!ReferenceEquals(requestInfo, NullRequestInfo))
                {
                    Reset();
                    return requestInfo;
                }

                if (rest > 0)
                {
                    searchEndMarkOffset = endPos + m_EndSearchState.Mark.Length;
                    searchEndMarkLength = rest;
                    continue;
                }

                //没有匹配结果
                if (prevMatched > 0)//缓存上一个开始标识数据块
                    AddArraySegment(m_BeginSearchState.Mark, 0, prevMatched, false);
                AddArraySegment(readBuffer, offset, length, toBeCopied);
                return NullRequestInfo;
            }
        }

        /// <summary>
        /// 处理匹配的请求数据结果
        /// </summary>
        /// <param name="readBuffer">请求数据缓存</param>
        /// <param name="offset">起始位置</param>
        /// <param name="length">数据长度</param>
        /// <returns>组包后转化成Request对象</returns>
        protected abstract TRequestInfo ProcessMatchedRequest(byte[] readBuffer, int offset, int length);



        /// <summary>
        /// 重置请求初始化实例
        /// </summary>
        public override void Reset()
        {
            m_BeginSearchState.Matched = 0;
            m_EndSearchState.Matched = 0;
            m_FoundBegin = false;

            base.Reset();

        }

    }
}

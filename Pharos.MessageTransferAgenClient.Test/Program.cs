﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pharos.MessageTransferAgenClient.Test
{
    class Program
    {
        static string showText;
        static int showTextcount;
        static DateTime date1 = default(DateTime);
        static DateTime date2 = default(DateTime);
        static void Main(string[] args)
        {
            Action<TestEvent> action = (t) =>
            {
                Task.Factory.StartNew((o) =>
                {
                    if (date1 == default(DateTime))
                        date1 = DateTime.Now;
                    date2 = DateTime.Now;
                    showTextcount++;
                    if (showTextcount > 10) return;
                    var e = (TestEvent)o;
                    showText += "接收到推送消息：" + e.Topic + "  " + e.ID + "   start:" + e.TimeStamp + "   end:" + DateTime.Now + "   " + e.Msg + Environment.NewLine;
                }, t);
            };
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (!string.IsNullOrEmpty(showText))
                    {
                        var ss = showText;
                        showText = string.Empty;
                        Console.WriteLine(date1 + "|" + date2 + ss);
                        showTextcount = 0;

                    }
                    Thread.Sleep(100);
                }
            });
            while (true)
            {
                Console.WriteLine("请输入命令（-s/-p/-q）：");

                switch (Console.ReadLine())
                {
                    case "-s":
                        Console.WriteLine("请输入订阅主题：");
                        new Pharos.MessageTransferAgenClient.DomainEvent.EventAggregator().SubscribeAsync<TestEvent>(Console.ReadLine(), action);
                        break;
                    case "-p":
                        Console.WriteLine("请输入推送主题：");
                        var topic = Console.ReadLine();
                        Console.WriteLine("请输入推送内容：");
                        new Pharos.MessageTransferAgenClient.DomainEvent.EventAggregator().Publish(topic, new TestEvent() { Msg = Console.ReadLine(), Topic = topic });
                        break;
                    case "-l":
                        Console.WriteLine("请输入推送主题：");
                        topic = Console.ReadLine();
                        Console.WriteLine("请输入推送内容：");
                        var msg = Console.ReadLine();
                        Console.WriteLine("请输入负载量：");
                        var count = Convert.ToInt32(Console.ReadLine());
                        var dt = DateTime.Now;
                        for (var i = 0; i < count; i++)
                        {
                            new Pharos.MessageTransferAgenClient.DomainEvent.EventAggregator().Publish(topic, new TestEvent() { Msg = i + "  ：" + msg, Topic = topic });
                        }
                        Console.WriteLine("推送时长：" + (DateTime.Now - dt).ToString("G"));
                        Console.WriteLine("推送开始时间：" + dt.ToString("dd HH：mm:ss.ffffff"));
                        break;
                    case "-q":
                        return;
                }
            }

        }

    }

    public class TestEvent : Pharos.MessageTransferAgenClient.DomainEvent.IEvent
    {
        public TestEvent()
        {
            ID = Guid.NewGuid();
            TimeStamp = DateTime.Now;
        }

        public System.Guid ID { get; set; }

        public System.DateTime TimeStamp { get; set; }

        public string Msg { get; set; }
        public string Topic { get; set; }
    }
}
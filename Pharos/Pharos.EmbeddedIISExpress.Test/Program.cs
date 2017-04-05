﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Pharos.EmbeddedIISExpress.Test
{
    class Program
    {
        static IISExpressSeverManager m;
        static void Main(string[] args)
        {
            m = new IISExpressSeverManager();
            m.Run(8111, @"D:\api", "v4.0", "IIS");
            Console.ReadLine();
        }
    }
}

﻿using Pharos.Logic.Entity;
using Pharos.Logic.MemberDomain.Interfaces;
using Pharos.ObjectModels.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pharos.Logic.MemberDomain.QuanChengTaoProviders.Scenes
{
    public class BarcodeScene : IScene<Members>
    {
        public decimal Count { get; set; }

        public decimal Amount { get; set; }

        public Members Member { get; set; }
    }
}
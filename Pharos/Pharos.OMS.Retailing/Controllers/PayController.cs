using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Pharos.Logic.OMS;
using QCT.Pay.Common;
using QCT.Pay.Common.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Pharos.Utility.Helpers;

namespace Pharos.OMS.Retailing.Controllers
{
    public class PayController : Controller
    {
        //
        // GET: /Pay/

        #region Pay提供给OMS的接口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mch_id"></param>
        /// <param name="store_id"></param>
        /// <returns></returns>
        public ActionResult QRCode(int mch_id, string store_id)
        {
            QrEncoder encoder = new QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M);
            QrCode qrCode;
            encoder.TryEncode(PayRules.GetPayQRCode(mch_id, store_id), out qrCode);

            GraphicsRenderer gRenderer = new GraphicsRenderer(
                new FixedModuleSize(6, QuietZoneModules.Two),
                Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            gRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            Response.ClearContent();
            Response.ContentType = "image/Gif";
            Response.BinaryWrite(ms.ToArray());
            return View();
        }
        #endregion
    }
}

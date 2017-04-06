using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Net;

namespace Pharos.Utility.Helpers
{
    /// <summary>
    /// 图片处理公共方法 create by kouzp 2014-02-13
    /// </summary>
    public class ImageHelper
    {
        #region 上传图片
        /// <summary>
        /// 上传图片 create by kouzp on 2013-8-21
        /// </summary>
        /// <param name="postfile">上传图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="isReturnFullPath">是否返回完整路径</param>
        /// <returns>图片名称</returns>
        public static string UploadImage(HttpPostedFile postfile, string savePath, bool isReturnFullPath = false)
        {
            try
            {
                if (string.IsNullOrEmpty(postfile.FileName))
                {
                    return UploadError("上传图片不能为空");
                }
                string modifyFileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                string[] ArrFileName = postfile.FileName.Split(new Char[] { '.' });
                string fileType = ArrFileName[ArrFileName.Length - 1];
                if ("|asp|aspx|dll|exe|bat|".IndexOf(fileType.ToLower()) > 0) //"|jpg|jpeg|gif|bmp|png|flash|mp3|rar|doc|xls|".IndexOf(fileType.ToLower()) < 1 || 
                {
                    return UploadError("请上传合法文件");
                }
                if (savePath != string.Empty)
                {
                    System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(savePath);
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                }
                string savefilePath = savePath + "/" + modifyFileName + "." + fileType;
                if (savefilePath != string.Empty)
                {
                    postfile.SaveAs(savefilePath);
                }
                if (isReturnFullPath)
                {
                    return savefilePath;
                }
                else
                {
                    return modifyFileName + "." + fileType;
                }
            }
            catch (Exception ex)
            {
                return UploadError(ex.Message);
            }
        }
        #endregion

        #region 上传图片并生成缩略图
        /// <summary>
        /// 上传图片（可选是否生成缩略图） create by kouzp 2013-8-21
        /// </summary>
        /// <param name="postfile">上传文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="thumbWidth">缩略图宽度</param>
        /// <param name="thumbHeight">缩略图高度</param>
        /// <param name="thumbMode">缩略图模式(HW,H,W,Cut)</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <returns>图片名称(缩略图为_s.)</returns>
        public static string ThumbImage(HttpPostedFile postfile, string savePath, int thumbWidth = 100, int thumbHeight = 100, string thumbMode = "Cut", bool isDelOimage = false)
        {
            #region 上传图片
            string fileImg = "UploadFiles\\" + DateTime.Now.ToString("yyyy") + "\\" + DateTime.Now.ToString("MM") + "\\" + DateTime.Now.ToString("dd") + "\\";
            if (savePath == "")
            {
                savePath = System.Web.HttpContext.Current.Server.MapPath("~") + fileImg;
            }
            string uploadImageValue = UploadImage(postfile, savePath);
            if (!isUploadSuccess(uploadImageValue))
            {
                return uploadImageValue;
            }
            string imageSavePath = savePath + "/" + uploadImageValue;
            #endregion

            #region 生成缩略图
            string thumbSavePath = savePath + "/" + uploadImageValue.Replace(".", "_s.");
            MakeThumbnail(imageSavePath, thumbSavePath, thumbWidth, thumbHeight, thumbMode);
            #endregion

            #region 是否删除原文件
            if (isDelOimage)
            {
                File.Delete(imageSavePath);
            }
            #endregion

            return fileImg + "/" + uploadImageValue;
        }
        #endregion

        #region 上传图片并添加文字水印
        /// <summary>
        /// 上传图片并添加文字水印 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="postfile">上传图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="waterText">水印文字</param>
        /// <param name="waterSaveFolder">水印图片保存文件夹</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <returns>图片名称</returns>
        public static string WaterTextImage(HttpPostedFile postfile, string savePath, string waterText = "格男仕", string waterSaveFolder = "/WaterText", bool isDelOimage = false)
        {
            #region 上传图片
            string uploadImageValue = UploadImage(postfile, savePath);
            if (!isUploadSuccess(uploadImageValue))
            {
                return uploadImageValue;
            }
            string imageSavePath = savePath + "/" + uploadImageValue;
            #endregion

            #region 添加文字水印
            string waterTextSavePath = savePath + waterSaveFolder + "/" + uploadImageValue;
            AddWaterText(imageSavePath, waterTextSavePath, waterText);
            #endregion

            #region 是否删除原文件
            if (isDelOimage)
            {
                File.Delete(imageSavePath);
            }
            #endregion

            return uploadImageValue;
        }
        #endregion

        #region 上传图片并添加图片水印
        /// <summary>
        /// 上传图片并添加图片水印 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="postfile">上传图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="waterPicPath">水印图片路径</param>
        /// <param name="waterSaveFolder">水印图片保存文件夹</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <returns>图片名称</returns>
        public static string WaterPicImage(HttpPostedFile postfile, string savePath, string waterPicPath, string waterSaveFolder = "/WaterPic", bool isDelOimage = false)
        {
            #region 上传图片
            string uploadImageValue = UploadImage(postfile, savePath);
            if (!isUploadSuccess(uploadImageValue))
            {
                return uploadImageValue;
            }
            string imageSavePath = savePath + "/" + uploadImageValue;
            #endregion

            #region 添加文字水印
            string waterTextSavePath = savePath + waterSaveFolder + "/" + uploadImageValue;
            AddWaterPic(imageSavePath, waterTextSavePath, waterPicPath);
            #endregion

            #region 是否删除原文件
            if (isDelOimage)
            {
                File.Delete(imageSavePath);
            }
            #endregion

            return uploadImageValue;
        }
        #endregion

        #region 上传图片生成缩略图并添加文字水印
        /// <summary>
        /// 上传图片生成缩略图并添加文字水印 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="postfile">上传文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="thumbWidth">缩略图宽度</param>
        /// <param name="thumbHeight">缩略图高度</param>
        /// <param name="thumbMode">缩略图模式(HW,H,W,Cut)</param>
        /// <param name="thumbSaveFolder">缩略图保存文件夹</param>
        /// <param name="waterText">水印文字</param>
        /// <param name="waterSaveFolder">水印图片保存文件夹</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <returns>图片名称</returns>
        public static string ThumbWaterTextImage(HttpPostedFile postfile, string savePath, int thumbWidth = 100, int thumbHeight = 100, string thumbMode = "Cut", string thumbSaveFolder = "/Thumb", string waterText = "格男仕", string waterSaveFolder = "/WaterText", bool isDelOimage = true, bool isDelThumbImage = true)
        {
            #region 上传图片
            string uploadImageValue = UploadImage(postfile, savePath);
            if (!isUploadSuccess(uploadImageValue))
            {
                return uploadImageValue;
            }
            string imageSavePath = savePath + "/" + uploadImageValue;
            #endregion

            #region 生成缩略图
            string thumbSavePath = savePath + thumbSaveFolder + "/" + uploadImageValue;
            MakeThumbnail(imageSavePath, thumbSavePath, thumbWidth, thumbHeight, thumbMode);
            #endregion

            #region 添加文字水印
            string waterTextSavePath = savePath + waterSaveFolder + "/" + uploadImageValue;
            AddWaterText(imageSavePath, waterTextSavePath, waterText);//上传图片添加水印
            AddWaterText(thumbSavePath, waterTextSavePath, waterText);//缩略图添加水印
            #endregion

            #region 是否删除原文件
            if (isDelOimage)
            {
                File.Delete(imageSavePath);
            }
            #endregion

            #region 是否删除缩略图
            if (isDelThumbImage)
            {
                File.Delete(thumbSavePath);
            }
            #endregion

            return uploadImageValue;
        }
        #endregion

        #region 上传图片生成缩略图并添加文字水印
        /// <summary>
        /// 上传图片生成缩略图并添加文字水印 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="postfile">上传文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="waterPicPath">水印图片路径</param>
        /// <param name="waterSaveFolder">水印图片保存文件夹</param>
        /// <param name="thumbWidth">缩略图宽度</param>
        /// <param name="thumbHeight">缩略图高度</param>
        /// <param name="thumbMode">缩略图模式(HW,H,W,Cut)</param>
        /// <param name="thumbSaveFolder">缩略图保存文件夹</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <param name="isDelOimage">是否删除上传图片</param>
        /// <returns>图片名称</returns>
        public static string ThumbWaterPicImage(HttpPostedFile postfile, string savePath, string waterPicPath, string waterSaveFolder = "/WaterPic", int thumbWidth = 100, int thumbHeight = 100, string thumbMode = "Cut", string thumbSaveFolder = "/Thumb", bool isDelOimage = true, bool isDelThumbImage = true)
        {
            #region 上传图片
            string uploadImageValue = UploadImage(postfile, savePath);
            if (!isUploadSuccess(uploadImageValue))
            {
                return uploadImageValue;
            }
            string imageSavePath = savePath + "/" + uploadImageValue;
            #endregion

            #region 生成缩略图
            string thumbSavePath = savePath + thumbSaveFolder + "/" + uploadImageValue;
            MakeThumbnail(imageSavePath, thumbSavePath, thumbWidth, thumbHeight, thumbMode);
            #endregion

            #region 添加文字水印
            string waterTextSavePath = savePath + waterSaveFolder + "/" + uploadImageValue;
            AddWaterPic(imageSavePath, waterTextSavePath, waterPicPath);//上传图片添加水印
            AddWaterPic(thumbSavePath, waterTextSavePath, waterPicPath);//缩略图添加水印
            #endregion

            #region 是否删除原文件
            if (isDelOimage)
            {
                File.Delete(imageSavePath);
            }
            #endregion

            #region 是否删除缩略图
            if (isDelThumbImage)
            {
                File.Delete(thumbSavePath);
            }
            #endregion

            return uploadImageValue;
        }
        #endregion

        #region 生成缩略图
        /// <summary>
        /// 生成缩略图 create by kouzp 2014-02-13
        /// </summary>
        /// <param name="imagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图保存路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string imagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)image.Width / (double)image.Height > (double)towidth / (double)toheight)
                    {
                        oh = image.Height;
                        ow = image.Height * towidth / toheight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / towidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            if (image.Width <= width && image.Height <= height)//保持原样
            {
                towidth = image.Width;
                toheight = image.Height;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                int len = thumbnailPath.LastIndexOf('/');
                if (len <= 0) len = thumbnailPath.LastIndexOf('\\');
                string dirPath = thumbnailPath.Substring(0, len);
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                image.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="stream">源图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <returns>字节数组</returns>
        public static byte[] MakeThumbnail(Stream stream, int width, int height, string mode)
        {
            System.Drawing.Image image = System.Drawing.Image.FromStream(stream);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = image.Width;
            int oh = image.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = image.Height * width / image.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = image.Width * height / image.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)image.Width / (double)image.Height > (double)towidth / (double)toheight)
                    {
                        oh = image.Height;
                        ow = image.Height * towidth / toheight;
                        y = 0;
                        x = (image.Width - ow) / 2;
                    }
                    else
                    {
                        ow = image.Width;
                        oh = image.Width * height / towidth;
                        x = 0;
                        y = (image.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }
            if (image.Width <= width && image.Height <= height)//保持原样
            {
                towidth = image.Width;
                toheight = image.Height;
            }
            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(image, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                MemoryStream ms = new MemoryStream();
                //以jpg格式保存缩略图
                bitmap.Save(ms, image.RawFormat);
                return ms.GetBuffer();
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                image.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }
        #endregion

        #region 在图片上增加文字水印
        /// <summary>
        /// 在图片上增加文字水印 create by kouzp 2014-02-13
        /// </summary>
        /// <param name="imagePath">原图片路径</param>
        /// <param name="savePath">加水印后图片保存路径</param>
        /// <param name="waterText">水印文字</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">水印透明度</param>
        public static void AddWaterText(string imagePath, string savePath, string waterText = "格男仕", WaterPosition position = WaterPosition.RigthBottom, double alpha = 0.3)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.DrawImage(image, 0, 0, image.Width, image.Height);

            //根据图片的大小我们来确定添加上去的文字的大小  
            //在这里我们定义一个数组来确定  
            int[] sizes = new int[] { 16, 14, 12, 10, 8, 6, 4 };

            //字体  
            Font crFont = null;
            //矩形的宽度和高度，SizeF有三个属性，分别为Height高，width宽，IsEmpty是否为空  
            SizeF crSize = new SizeF();

            //利用一个循环语句来选择我们要添加文字的型号  
            //直到它的长度比图片的宽度小  
            for (int i = 0; i < 7; i++)
            {
                crFont = new Font("arial", sizes[i], FontStyle.Bold);

                //测量用指定的 Font 对象绘制并用指定的 StringFormat 对象格式化的指定字符串。  
                crSize = g.MeasureString(waterText, crFont);

                // ushort 关键字表示一种整数数据类型  
                if ((ushort)crSize.Width < (ushort)image.Width)
                    break;
            }

            //下面定义一个矩形区域，以后在这个矩形里画上白底黑字  
            float rectX = 0;
            float rectY = 0;
            float rectWidth = crSize.Width;
            float rectHeight = crSize.Height;

            #region 水印位置
            switch (position)
            {
                case WaterPosition.BottomMiddle:
                    rectX = image.Width / 2;
                    rectY = image.Height - rectHeight - 10;
                    break;
                case WaterPosition.Center:
                    rectX = image.Width / 2;
                    rectY = image.Height / 2;
                    break;
                case WaterPosition.LeftBottom:
                    rectX = rectWidth;
                    rectY = image.Height - rectHeight - 10;
                    break;
                case WaterPosition.LeftTop:
                    rectX = rectWidth / 2;
                    rectY = rectHeight / 2;
                    break;
                case WaterPosition.RightTop:
                    rectX = image.Width - rectWidth - 10;
                    rectY = rectHeight;
                    break;
                case WaterPosition.RigthBottom:
                    rectX = image.Width - rectWidth - 10;
                    rectY = image.Height - rectHeight - 10;
                    break;
                case WaterPosition.TopMiddle:
                    rectX = image.Width / 2;
                    rectY = rectWidth;
                    break;
                default:
                    rectX = rectWidth;
                    rectY = image.Height - rectHeight - 10;
                    break;
            }
            #endregion

            //声明矩形域  
            RectangleF textArea = new RectangleF(rectX, rectY, rectWidth, rectHeight);
            Font font = crFont; //定义字体  
            int m_alpha = Convert.ToInt32(256 * alpha);//这个画笔为描绘阴影的画笔，呈灰色
            Brush whiteBrush = new SolidBrush(Color.FromArgb(153, 255, 255, 255));//笔刷，画文字用  
            Brush blackBrush = new SolidBrush(Color.FromArgb(m_alpha, 0, 0, 0)); //笔刷，画背景用 

            //g.FillRectangle(blackBrush, rectX, rectY, rectWidth, rectHeight);//背景
            RectangleF backArea = new RectangleF(rectX + 1, rectY + 1, rectWidth, rectHeight);//背景
            g.DrawString(waterText, font, blackBrush, backArea);
            g.DrawString(waterText, font, whiteBrush, textArea);

            g.Dispose();
            string dirPath = savePath.Substring(0, savePath.LastIndexOf('/'));
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            image.Save(savePath);
            image.Dispose();
        }
        #endregion

        #region 在图片上生成图片水印
        /// <summary>
        /// 在图片上生成图片水印 create by kouzp 2014-02-13
        /// </summary>
        /// <param name="imagePath">原图片路径</param>
        /// <param name="savePath">加水印后图片保存路径</param>
        /// <param name="waterPicPath">水印图片路径</param>
        /// <param name="position">水印位置</param>
        /// <param name="alpha">水印透明度</param>
        public static void AddWaterPic(string imagePath, string savePath, string waterPicPath, WaterPosition position = WaterPosition.RigthBottom, double alpha = 0.3)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath);
            System.Drawing.Image copyImage = System.Drawing.Image.FromFile(waterPicPath);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.AntiAlias;


            //与底图一样，我们需要一个位图来装载水印图片。并设定其分辨率  
            Bitmap bmPhoto = new Bitmap(image.Width, image.Height, PixelFormat.Format24bppRgb);
            // 定义一个绘图画面用来装载位图  
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            //同样，由于水印是图片，我们也需要定义一个Image来装载它  
            System.Drawing.Image imgWatermark = new Bitmap(waterPicPath);
            grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
            // 第一次描绘，将我们的底图描绘在绘图画面上  
            grPhoto.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            Bitmap bmWatermark = new Bitmap(bmPhoto);
            bmWatermark.SetResolution(image.HorizontalResolution, image.VerticalResolution);


            // 继续，将水印图片装载到一个绘图画面grWatermark  
            Graphics grWatermark = Graphics.FromImage(bmWatermark);

            //ImageAttributes 对象包含有关在呈现时如何操作位图和图元文件颜色的信息。  
            ImageAttributes imageAttributes = new ImageAttributes();

            //Colormap: 定义转换颜色的映射  
            ColorMap colorMap = new ColorMap();

            //我的水印图被定义成拥有绿色背景色的图片被替换成透明  
            colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
            colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

            float[][] colorMatrixElements = {   
            new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f}, // red红色  
            new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f}, //green绿色  
            new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f}, //blue蓝色         
            new float[] {0.0f,  0.0f,  0.0f,  (float)alpha, 0.0f}, //透明度       
            new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}};//  

            //  ColorMatrix:定义包含 RGBA 空间坐标的 5 x 5 矩阵。  
            //  ImageAttributes 类的若干方法通过使用颜色矩阵调整图像颜色。  
            ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default,
             ColorAdjustType.Bitmap);

            //上面设置完颜色，下面开始设置位置  
            int rectX;
            int rectY;

            #region 水印位置
            switch (position)
            {
                case WaterPosition.BottomMiddle:
                    rectX = image.Width / 2;
                    rectY = image.Height - copyImage.Height - 10;
                    break;
                case WaterPosition.Center:
                    rectX = image.Width / 2;
                    rectY = image.Height / 2;
                    break;
                case WaterPosition.LeftBottom:
                    rectX = copyImage.Width;
                    rectY = image.Height - copyImage.Height - 10;
                    break;
                case WaterPosition.LeftTop:
                    rectX = copyImage.Width / 2;
                    rectY = copyImage.Height / 2;
                    break;
                case WaterPosition.RightTop:
                    rectX = image.Width - copyImage.Width - 10;
                    rectY = copyImage.Height;
                    break;
                case WaterPosition.RigthBottom:
                    rectX = image.Width - copyImage.Width - 10;
                    rectY = image.Height - copyImage.Height - 10;
                    break;
                case WaterPosition.TopMiddle:
                    rectX = image.Width / 2;
                    rectY = copyImage.Width;
                    break;
                default:
                    rectX = copyImage.Width;
                    rectY = image.Height - copyImage.Height - 10;
                    break;
            }
            #endregion

            //第二次绘图，把水印印上去  
            imgWatermark = new Bitmap(waterPicPath);
            grWatermark.DrawImage(imgWatermark, new Rectangle(rectX, rectY, copyImage.Width, copyImage.Height), 0, 0, copyImage.Width, copyImage.Height, GraphicsUnit.Pixel, imageAttributes);
            image = bmWatermark;
            grWatermark.Dispose();
            g.Dispose();
            string dirPath = savePath.Substring(0, savePath.LastIndexOf('/'));
            DirectoryInfo dir = new DirectoryInfo(dirPath);
            if (!dir.Exists)
            {
                dir.Create();
            }
            image.Save(savePath);
            image.Dispose();
        }
        #endregion

        #region 上传成功提示
        /// <summary>
        /// 成功提示 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <returns>成功信息</returns>
        private static string UploadSuccess(string msg)
        {
            return "success:" + msg;
        }
        #endregion

        #region 上传错误提示
        /// <summary>
        /// 错误提示 create by kouzp 2013-8-21
        /// </summary>
        /// <param name="msg">提示内容</param>
        /// <returns>错误信息</returns>
        private static string UploadError(string msg)
        {
            return "error:" + msg;
        }
        #endregion

        #region 是否上传成功
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uploadMsg"></param>
        /// <returns></returns>
        public static bool isUploadSuccess(string uploadMsg)
        {
            if (uploadMsg.Contains("error:"))
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 图片位置
        /// <summary>  
        /// 图片位置  
        /// </summary>  
        public enum WaterPosition
        {
            LeftTop,        //左上  
            LeftBottom,    //左下  
            RightTop,       //右上  
            RigthBottom,  //右下  
            TopMiddle,     //顶部居中  
            BottomMiddle, //底部居中  
            Center           //中心  
        }
        #endregion

        #region 图片转换为 base64 格式的 DataUri
        /// <summary>
        /// 取给定图片路径，将之转换为 base64 格式的 DataUri
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns>base64 格式的 DataUri</returns>
        public static string ToDataUri(string imgPath)
        {
            return "data:image/" + System.IO.Path.GetExtension(imgPath).Substring(1) + ";base64," + ToBase64(imgPath);
        }
        /// <summary>
        /// 取给定图片路径，将之转换为 base64 格式
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns>图片的 base64 编码数据</returns>
        public static string ToBase64(string imgPath)
        {
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.FileStream fs = System.IO.File.OpenRead(imgPath);
                int filelength = 0;
                filelength = (int)fs.Length;
                Byte[] imgArr = new Byte[filelength];
                fs.Read(imgArr, 0, filelength);
                fs.Close();
                return Convert.ToBase64String(imgArr);
            }
            else
            {
                //if (NetFileHelper.IsExistRemote(imgPath))
                //{
                    try
                    {
                        HttpWebRequest request = WebRequest.Create(imgPath) as HttpWebRequest;
                        request.Method = "GET";

                        using (HttpWebResponse resp = request.GetResponse() as HttpWebResponse)
                        {
                            Stream imgstream = resp.GetResponseStream();
                            System.Drawing.Image img = System.Drawing.Image.FromStream(imgstream);
                            MemoryStream ms = new MemoryStream();
                            img.Save(ms, img.RawFormat);
                            var imgByte = ms.ToArray();
                            ms.Close();
                            return Convert.ToBase64String(imgByte);
                        }
                    }
                    catch (Exception e)
                    {
                        throw;
                    }
                //}
                return string.Empty;
            }
        }
        #endregion


        #region 生成用户默认头像

        public static void MakeUserDefultPhoto(string userName, string color, string savePath)
        {
            if (string.IsNullOrEmpty(userName)) return;
            if (userName.Length > 2) userName = userName.Substring(userName.Length - 2, 2);
            System.Drawing.Bitmap image = new System.Drawing.Bitmap(100, 100);
            try
            {
                //裁剪成圆形
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(new Rectangle(0, 0, 100, 100));
                using (Graphics g = Graphics.FromImage(image))
                { //假设bm就是你要绘制的正方形位图，已创建好
                    g.SetClip(gp);

                    //清空图片背景色
                    Color c = System.Drawing.ColorTranslator.FromHtml(color);
                    g.Clear(c);

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    //Font(字体 大小 粗细
                    Font font = new System.Drawing.Font("Microsoft YaHei", 28, (System.Drawing.FontStyle.Bold));
                    //LinearGradientBrush的参数是画的图像,起始颜色,简便的终止颜色,粗细,true
                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.White, Color.White, 1.2f, true);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(userName, font, brush, new Rectangle(0, 0, image.Width, image.Height), sf);

                    image.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            finally
            {
                //g.Dispose();
                image.Dispose();
            }
        }
        #endregion

        #region 裁剪上传的图片
        /// <summary>  
        /// 剪切图片  
        /// </summary>  
        /// <param name="sourcePath"></param>  
        /// <param name="sx"></param>  
        /// <param name="sy"></param>  
        /// <param name="width"></param>  
        /// <param name="height"></param>  
        /// <returns></returns>  
        public static void MakeThumbnail(string sourcePath, int sx, int sy, int width, int height, string targetPath, string targetThumbnailPath = null)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(sourcePath);

            int x = sx;
            int y = sy;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //新建一个bmp图片   
            System.Drawing.Image bitmap = new Bitmap(width, height);

            //新建一个画板   
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法   
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度   
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充   
            g.Clear(Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分   
            g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(targetPath);
                if (!string.IsNullOrEmpty(targetThumbnailPath))
                {
                    //bitmap.Save(targetThumbnailPath);
                    ImageHelper.MakeThumbnail(targetPath, targetThumbnailPath, 200, 200, "W");
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }



        #endregion
    }
}

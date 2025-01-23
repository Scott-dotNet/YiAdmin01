using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace YiAdmin01.Common.Utils
{
    /// <summary>
    /// 验证码生成类
    /// </summary>
    public  class CaptchaHelper
    {
        #region 得到验证码
        /// <summary>
        /// Tuple第一个值是表达式，第二个值是表达式结果
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, string> GetCaptchaCode()
        {
            int result = 0;
            char[] operators = { '+', '-', '*' };
            StringBuilder expression = new StringBuilder();

            Random random = new Random();
            int firstNum = random.Next(10);
            int secondNum = random.Next(10);
            char op = operators[random.Next(0, operators.Length)];

            switch (op)
            {
                case '+': result = firstNum + secondNum; break;
                case '*': result = firstNum * secondNum; break;
                case '-':
                    if (firstNum < secondNum)
                    {
                        int temp = firstNum;
                        firstNum = secondNum;
                        secondNum = temp;
                    }
                    result = firstNum - secondNum; 
                    break;
            }

            expression.Append((char)('0' + firstNum))
                .Append(op)
                .Append((char)('0' + secondNum))
                .Append("=?");
            return new Tuple<string, string>(expression.ToString(), result.ToString());
        }

        #endregion

        #region 生成验证码图片
        /// <summary>
        /// 生成验证码图片
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static byte[] CreateCaptchaImage(string captchaCode)
        {
            Random random = new Random();
            //颜色
            var colors = new[] {SKColors.Red, SKColors.DarkBlue, SKColors.Green ,SKColors.Black, SKColors.Orange, SKColors.Brown, SKColors.DarkCyan, SKColors.Purple };
            //字体
            var fonts = new[] { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            // 画布设置
            using var image2d = new SKBitmap(100, 30, SKColorType.Bgra8888, SKAlphaType.Premul);
            using var canvas = new SKCanvas(image2d);

            canvas.DrawColor(SKColors.AntiqueWhite);
            using var drawStyle = new SKPaint();
            using var drawFont = new SKFont();
            //填充验证码到图片
            for (int i = 0; i < captchaCode.Length; i++) 
            {
                drawStyle.IsAntialias = true;
                drawFont.Size = 30;
                drawFont.Typeface = SKTypeface.FromFamilyName(fonts[random.Next(fonts.Length)], SKFontStyleWeight.SemiBold, SKFontStyleWidth.ExtraCondensed, SKFontStyleSlant.Upright);
                drawStyle.Color = colors[random.Next(colors.Length)];

                string text = captchaCode[i].ToString();
                int width = 16 * (i + 1);
                int height = 28;
                canvas.DrawText(text, width, height, SKTextAlign.Center, drawFont, drawStyle);
            }
            //生成干扰线
            for (int i = 0; i <= captchaCode.Length; i++) 
            {
                drawStyle.Color = colors[random.Next(colors.Length)];
                drawStyle.StrokeWidth = 1;
                canvas.DrawLine(random.Next(0, captchaCode.Length * 15), random.Next(0, 60), random.Next(0, captchaCode.Length * 16), random.Next(0, 30), drawStyle);
            }
            //创建对象信息
            using var img = SKImage.FromBitmap(image2d);
            using var pic = img.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = new MemoryStream();
            //保存到流
            pic.SaveTo(stream);
            var captchaBytes = stream.GetBuffer();
            return captchaBytes;
        }

        /// <summary>
        /// Tuple 的第一个值是表达式结果
        /// Tuple 的第二个值是表达式图片
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, byte[]> GetImgData()
        {
            Tuple<string, string> tuple = GetCaptchaCode();
            var result = tuple.Item2.ToString();
            var img = CreateCaptchaImage(tuple.Item1);
            return new Tuple<string, byte[]>(result, img);
        }


        #endregion
    }
}

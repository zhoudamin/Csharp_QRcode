using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using com.google.zxing;
using ByteMatrix = com.google.zxing.common.ByteMatrix;

namespace WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        //生成条形码
        private void button1_Click(object sender, EventArgs e)
        {
            lbshow.Text = "";
            Regex rg = new Regex("^[0-9]{13}$");
            if (!rg.IsMatch(txtMsg.Text))
            {
                MessageBox.Show("本例子采用EAN_13编码，需要输入13位数字");
                return;
            }
           
            try
            {
                MultiFormatWriter mutiWriter = new com.google.zxing.MultiFormatWriter();
                ByteMatrix bm = mutiWriter.encode(txtMsg.Text, com.google.zxing.BarcodeFormat.EAN_13, 363, 150);
                Bitmap img= bm.ToBitmap();
                pictureBox1.Image =img;

                //自动保存图片到当前目录
                string filename = System.Environment.CurrentDirectory + "\\EAN_13" + DateTime.Now.Ticks.ToString() + ".jpg";
                img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                lbshow.Text = "图片已保存到：" + filename;
            }
            catch(Exception ee)
            { MessageBox.Show(ee.Message); }
        }

        //生成二维码
        private void button2_Click(object sender, EventArgs e)
        {
            lbshow.Text = "";
            try
            {


//***************************************主程序***********************************************************************************************//、

                MultiFormatWriter mutiWriter = new com.google.zxing.MultiFormatWriter();
                ByteMatrix bm = mutiWriter.encode(txtMsg.Text, com.google.zxing.BarcodeFormat.QR_CODE, 300, 300);
                Bitmap img = bm.ToBitmap();
                pictureBox1.Image = img;

                //自动保存图片到当前目录
                string filename = System.Environment.CurrentDirectory + "\\QR" + DateTime.Now.Ticks.ToString() + ".jpg";
                img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
                lbshow.Text = "图片已保存到：" + filename;
            }
            catch (Exception ee)
            { MessageBox.Show(ee.Message); }
        }

        string opFilePath = "";
        //打开文件
        private void button4_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Filter = "*.jpg|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                opFilePath = openFileDialog1.FileName;
                pictureBox1.ImageLocation = opFilePath;
            }
        }

        //解码操作
        private void button3_Click(object sender, EventArgs e)
        {
            MultiFormatReader mutiReader = new com.google.zxing.MultiFormatReader();
            Bitmap img = (Bitmap)Bitmap.FromFile(opFilePath);
            if (img == null)
                return;
            LuminanceSource ls = new RGBLuminanceSource(img, img.Width, img.Height);
            BinaryBitmap bb = new BinaryBitmap(new com.google.zxing.common.HybridBinarizer(ls));

            Hashtable hints = new Hashtable();
            hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
            Result r = mutiReader.decode(bb, hints);
            txtMsg.Text = r.Text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pansonic;
using System.IO;
using System.IO.Ports;

namespace pansonicPLC_Test
{
    public partial class Form1 : Form
    {
        PanasonicPLC PLC = new PanasonicPLC();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            PLC.MewtocolSet("COM1", 115200, 8, 1, Parity.Odd);    //设置串口参数
                                                                  // PLC.MewtocolClose();                                //先关闭相应串口
            bool  s = PLC.MewtocolOpen();                            //再打开PLC串口通讯
            if (s)
            {
                device_plc_status.Text = "PLC串口成功打开";
            }
            else
            {
                device_plc_status.Text = "PLC串口打开失败";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PLC.WriteSinglePoint("R80",true);
            sendStr.Text += PLC.GetSendStr();
            receiveStr.Text += PLC.GetReceiveStr();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            PLC.ReadSinglePoint("Y30F");
            sendStr.Text += PLC.GetSendStr();
            receiveStr.Text += PLC.GetReceiveStr();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PLC.WriteSinglePoint("R80", false);
            sendStr.Text += PLC.GetSendStr();
            receiveStr.Text += PLC.GetReceiveStr();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PLC.ReadWordPoint("Y30");
            sendStr.Text += PLC.GetSendStr();
            receiveStr.Text += PLC.GetReceiveStr();
        }
    }
}

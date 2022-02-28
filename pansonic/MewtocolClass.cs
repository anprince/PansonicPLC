using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pansonic
{
    /// <summary>
    /// 生产Mewtocol协议的各种命令的命文类 结束符：CR（\r）
    /// </summary>
    public class MewtocolClass
    {

        SoftBasic SoftBasic = new SoftBasic();
        /// <summary>
        /// 默认构造函数 PLC站号默认为1
        /// </summary>
        public MewtocolClass()
        {
            _station = 1;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="station">站号</param>
        public MewtocolClass(short station)
        {
            _station = station;
        }
        private short _station ;         // 站号默认为1
        /// <summary>
        /// PLC站号
        /// </summary>
        public short Station
        {
            get { return _station; }
            set { _station = value; }
        }

        /// <summary>
        /// 读取单触点状态协议文
        /// </summary>
        /// <param name="d">触点地址</param>
        /// <returns></returns>
        public string RCS(string d)         //读单触点
        {
            string sendData;
            string s;
            //命令： % 站号 # 读写命令          

            string f = d.Substring(1, d.Length - 1);
            f = f.ToString().PadLeft(4, '0');
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "RCS" + d[0] + f;
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码

            return s;
        }

        /// <summary>
        /// 写入单触点状态协议文
        /// </summary>
        ///  <param name="d">触点地址</param>
        ///   <param name="b">开关量true/false</param>
        /// <returns></returns>
        public string WCS(string d, bool b)         //写单触点
        {
            string sendData;
            string s;
            string g;
            //命令：% 站号 # 读写命令          
            string f = d.Substring(1, d.Length - 1);
            f = f.ToString().PadLeft(4, '0');
            if (b) g = "1";
            else g = "0";
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "WCS" + d[0] + f + g;
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }
        /// <summary>
        /// 读取多触点状态协议文
        /// </summary>
        /// <param name="d">起始触点地址</param>
        /// <param name="n">读取触点个数 n=1-8</param>
        /// <returns></returns>
        public string RCP(string d, int n)
        {
            string sendData;
            string s;
            //命令 % 站号 # 读写命令          

            string f = d.Substring(1, d.Length - 1);    //取出触点编号数据
            string f1 = f.PadLeft(4, '0');              //不足四位左边补0    
            string fBCD = f1.Substring(0, 3);           //取出前BCD格式的3位          
            string fHEX = f1.Substring(3, 1);           //取出HEX格式的最后一位
            fHEX = fHEX.PadLeft(2, '0');                //补0 改为hex字符串形式
            //int i = int.Parse(s);
            //string show = i.ToString();

            byte[] bs = new byte[1];
            bs = SoftBasic.HexStringToBytes(fHEX);       //把最后一位Hex转为byte[]
            byte address = bs[0];                        //把最后一位Hex转为byte

            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "RCP" + n.ToString();
            for (byte by = address; by < (address + n); by++)
            {
                byte[] bs1 = new byte[1];
                bs1[0] = by;                                   //by为最后一位的值                                          
                string sn = SoftBasic.ByteToHexString(bs1);    //将最后一位转为HEX字符串              
                string sn0 = sn.Substring(0, 1);               //取hex字符串第一位
                string sn1 = sn.Substring(1, 1);               //取hex字符串第二位
                int i = int.Parse(fBCD) + int.Parse((sn0));      //（将触点编号前3位BCD值转为INT）+（HEX字符串第一位转为int）
                string ss = i.ToString().PadLeft(3, '0');      //将前3位BCD补足3位
                string st = ss + sn1;                             //得到新的触点编号数据
                sendData += d[0] + st;
            }
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }

        /// <summary>
        /// 写入多触点状态协议文
        /// </summary>
        /// <param name="d">起始触点地址</param>
        /// <param name="n">写入触点个数 n=1-8</param>
        /// <param name="staus">写入值， "0"或者"1"</param>
        /// <returns></returns>
        public string WCP(string d, int n, string staus)
        {
            string sendData;
            string s;
            //命令 % 站号 # 读写命令          

            string f = d.Substring(1, d.Length - 1);    //取出触点编号数据
            string f1 = f.PadLeft(4, '0');              //不足四位左边补0    
            string fBCD = f1.Substring(0, 3);           //取出前BCD格式的3位          
            string fHEX = f1.Substring(3, 1);           //取出HEX格式的最后一位
            fHEX = fHEX.PadLeft(2, '0');                //补0 改为hex字符串形式
            //int i = int.Parse(s);
            //string show = i.ToString();

            byte[] bs = new byte[1];
            bs = SoftBasic.HexStringToBytes(fHEX);       //把最后一位Hex转为byte[]
            byte address = bs[0];                        //把最后一位Hex转为byte

            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "WCP" + n.ToString();
            for (byte by = address; by < (address + n); by++)
            {
                byte[] bs1 = new byte[1];
                bs1[0] = by;                                   //by为最后一位的值                                          
                string sn = SoftBasic.ByteToHexString(bs1);    //将最后一位转为HEX字符串              
                string sn0 = sn.Substring(0, 1);               //取hex字符串第一位
                string sn1 = sn.Substring(1, 1);               //取hex字符串第二位
                int i = int.Parse(fBCD) + int.Parse((sn0));      //（将触点编号前3位BCD值转为INT）+（HEX字符串第一位转为int）
                string ss = i.ToString().PadLeft(3, '0');      //将前3位BCD补足3位
                string st = ss + sn1;                             //得到新的触点编号数据
                sendData += d[0] + st + staus;
            }
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }


        /*************************读/写触点状态的辅助说明****************************************    
           
        触点代码  组编号     应 答 信 息
         T(C)       0       T(C): 0 ～ 15
                    1       T(C): 16 ～ 31
                    2       T(C): 32 ～ 47
                    3       T(C): 48 ～ 63
                    :            :
                    :            :
                   15 T(C):240 ～ 255
        **************************************************************************************/
        /// <summary>
        /// 按字单位读取触点状态协议文
        /// </summary>
        /// <param name="d">触点地址</param>
        /// <param name="n">读取字个数</param>
        /// <returns></returns>
        public string RCC(string d, int n)
        {
            string sendData;
            string s;
            //命令 % 站号 # 读写命令          

            string f = d.Substring(1, d.Length - 1);
            string f1 = f.PadLeft(4, '0');
            string f2 = Convert.ToString((Convert.ToInt32(f1) + n - 1)).PadLeft(4, '0');
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "RCC" + d[0] + f1 + f2;
            //s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData);                 //带校验码
            return s;
        }

        /// <summary>
        /// 按字单位写入触点状态协议文
        /// </summary>
        /// <param name="d">触点地址</param>
        /// <param name="n">读取字个数</param>
        /// <param name="inData">short型16位数据数组</param>
        /// <returns></returns>
        public string WCC(string d, int n, short[] inData)
        {
            string sendData;
            string s;
            string f = d.Substring(1, d.Length - 1);
            string f1 = f.PadLeft(4, '0');
            string f2 = Convert.ToString((Convert.ToInt32(f1) + n - 1)).PadLeft(4, '0');

            byte[] wdata = new byte[(int)(n * 2)];
            //转换写入值到wdata数组
            for (int i = 0; i < n; i++)
            {
                byte[] temp = BitConverter.GetBytes(inData[i]);
                wdata[i * 2] = temp[1];//转换为PLC的高位在前储存方式
                wdata[i * 2 + 1] = temp[0];
            }
            string e = SoftBasic.ByteToHexString(wdata);
            //拼接写入字符串
            //命令 % 站号 # 读写命令                   
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "WCC" + d[0] + f1 + f2 + e;
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }

        /// <summary>
        /// 读取多个数据寄存器值协议文
        /// </summary>
        /// <param name="d">数据寄存器地址</param>
        /// <param name="n">读取字个数 不大于25</param>
        /// <returns></returns>
        public string RD(string d, int n)
        {
            string sendData;
            string s;

            string f = d.Substring(1, d.Length - 1);
            string f1 = f.PadLeft(5, '0');
            string f2 = Convert.ToString((Convert.ToInt32(f1) + n - 1)).PadLeft(5, '0');
            if (n <= 25)
            {
                //% # R D 数据代码（1字符）起始数据编码(5 字符) 结束数据编码(5 字符)   BCC(H) BCC(L) CR               
                sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "RD" + d[0] + f1 + f2;
            }
            else
            {
                sendData = "<" + _station.ToString().PadLeft(2, '0') + "#" + "RD" + d[0] + f1 + f2;        //读取字超过25，协议头改为"<"    
            }
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }

        /****************************读写数据长度的辅助说明**********************************************
         注意：使用MEWTOCOL协议时，如果通信帧长度超过118字节就会报错。此时可以使用协议的另一种格式，将的第一个标志符ASCII码“%”变成“<”。

         以上 即最多返回100字节的数据，读一个字需要2字节，那么标准协议格式最多可以读取50个字的数据。
         
         ************************************************************************************************/

        /// <summary>
        /// 写入多个数据寄存器值协议文
        /// </summary>
        /// <param name="d">首个数据寄存器地址</param>
        /// <param name="n">写入字个数</param>
        /// <param name="inData">short型16位数据数组</param>
        /// <returns></returns>
        public string WD(string d, int n, short[] inData)
        {
            string sendData;
            string s;

            string f = d.Substring(1, d.Length - 1);
            string f1 = f.PadLeft(5, '0');
            string f2 = Convert.ToString((Convert.ToInt32(f1) + n - 1)).PadLeft(5, '0');

            byte[] wdata = new byte[(int)(n * 2)];
            //  转换写入值到wdata数组
            for (int i = 0; i < n; i++)
            {
                byte[] temp = BitConverter.GetBytes(inData[i]);
                wdata[i * 2] = temp[1];                         //转换为PLC的高位在前储存方式
                wdata[i * 2 + 1] = temp[0];
            }
            string e = SoftBasic.ByteToHexString(wdata);       //拼接写入字符串

            //% # R D 数据代码（1字符）起始数据编码(5 字符) 结束数据编码(5 字符)   BCC(H) BCC(L) CR               
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "WD" + d[0] + f1 + f2 + e;
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData);                 //带校验码
            return s;
        }
        /// <summary>
        /// 写入单个16位寄存器值
        /// </summary>
        /// <param name="d">数据寄存器地址</param>
        /// <param name="value">数据寄存器的值</param>
        /// <returns></returns>
        public string WD(string d, short value)
        {
            string sendData;
            string s;
            string f = d.Substring(1, d.Length - 1);
            string f1 = f.PadLeft(5, '0');
            string f2 = f1;


            value = (short)((value >> 8) + (value << 8));     //将short值前后八位字节对换

            string e = value.ToString();      //转换为字符串

            //% # R D 数据代码（1字符）起始数据编码(5 字符) 结束数据编码(5 字符)   BCC(H) BCC(L) CR               
            sendData = "%" + _station.ToString().PadLeft(2, '0') + "#" + "WD" + d[0] + f1 + f2 + e;
            // s = sendData + "**\r";                             //不带校验码
            s = sendData + BCC(sendData) ;                 //带校验码
            return s;
        }

        /// <summary>
        /// mewtocol协议BCC校验码计算
        /// </summary>
        /// <param name="sendData">要计算校验码的数据</param>
        /// <returns> </returns>
        private string BCC(string sendData)                              //mewtocol协议BCC校验码计算
        {

            char[] CharList = sendData.ToArray();
            int BCC = CharList[0];
            for (int i = 0; i < CharList.Length - 1; i++)
            {
                BCC ^= CharList[i + 1];
            }

            string sendBCC = Convert.ToString(BCC, 16).ToUpper().PadLeft(2, '0');  //BCC必须是大写的十六进制，小写的会报BCC错误  必须补足两位校验码
            return sendBCC;
        }
        /// <summary>
        /// byte[]数组转为short[]数组
        /// </summary>
        /// <param name="src">需要转换的byte数组</param>
        /// <returns></returns>
        public static short[] toShortArray(byte[] src)
        {

            int count = src.Length >> 1;
            short[] dest = new short[count];
            for (int i = 0; i < count; i++)
            {
                dest[i] = (short)(src[i * 2 + 1] << 8 | src[2 * i] & 0xff);
            }
            return dest;
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Pansonic
{
    /// <summary>
    /// 发送串口命令，返回相应值的松下PLC类
    /// </summary>
    public class PanasonicPLC 
    {
        SerialPort MewtocolSP;
        MewtocolClass MC;

       
        object _lockObj = new object();
        bool serialReceiework;
        /// <summary>
        /// 默认构造函数 要使用需先设置串口连接函数并打开串口 有需要的话需设置PLC站号
        /// </summary>
        public PanasonicPLC()
        {
            MewtocolSP = new SerialPort();
            MewtocolSP.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            MC = new MewtocolClass();
            string s = MewtocolSP.NewLine;
            MewtocolSP.NewLine = "\r";
            s = MewtocolSP.NewLine;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="COMx"></param>
        /// <param name="baudRate"></param>
        /// <param name="dataBits"></param>
        /// <param name="stopBits"></param>
        /// <param name="parity"></param>
        public PanasonicPLC(string COMx, int baudRate,int dataBits, int stopBits, Parity parity)
        {
            MewtocolSP = new SerialPort();
            MewtocolSP.DataReceived += new SerialDataReceivedEventHandler(DataReceived);
            MewtocolSet(COMx, baudRate, dataBits, stopBits, parity);
            MC = new MewtocolClass();
            string s = MewtocolSP.NewLine;
            MewtocolSP.NewLine = "\r";
            s = MewtocolSP.NewLine;

        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (MewtocolSP.IsOpen&&serialReceiework)     //此处可能没有必要判断是否打开串口，但为了严谨性，我还是加上了
            {
                // Thread.Sleep(15);
                _receive = MewtocolSP.ReadLine().Trim();
                string s = MewtocolSP.NewLine;              
                MewtocolSP.DiscardInBuffer();                      //清空SerialPort控件的Buffer 
                serialReceiework = false;
            }
            else
            {
                Thread.Sleep(10);
                _receive = MewtocolSP.ReadExisting().Trim();
                MewtocolSP.DiscardInBuffer();
                //  MessageBox.Show("请打开某个串口", "错误提示");
            }
        }
    



    /// <summary>
    /// 设置串口连接的各个参数
    /// </summary>
    ///  <param name="COMx">串口号 "COM0-COM16"</param>
    /// <param name="baudRate">波特率 9600，19200，38400，57600，115200</param>
    /// <param name="dataBits">数据位 7（7位），8（8位）</param>
    /// <param name="stopBits">停止位 1（1位），2（2位）</param>
    /// <param name="parity">校验 Partity.ODD(奇校验)，Partity.EVEN(偶校验)，Partity.NONE(无校验) ）</param>
    /// <returns></returns>
    public void MewtocolSet(string COMx, int baudRate, int dataBits, int stopBits, Parity parity)
        {
            MewtocolSP.PortName = COMx;            //串口号
            MewtocolSP.BaudRate = baudRate;       //波特率
            MewtocolSP.DataBits = dataBits;       //数据位 8位
            if (stopBits == 2)
            {
                MewtocolSP.StopBits = StopBits.Two;  //停止位 1位
            }
            //else if (stopBits == 1.5)
            //{
            //    BasicClass.MewtocolSP.StopBits = StopBits.OnePointFive;  //停止位 1.5位
            //}
            else
            {
                MewtocolSP.StopBits = StopBits.One;       //所有其他设置停止位都为1位
            }
            MewtocolSP.Parity = parity;      //校验位 无校验 Parity.Odd
        }

        /// <summary>
        /// 获取一个值，指示串口是否处于打开状态
        /// </summary>
        /// <returns>是或否</returns>
        public bool IsOpen()
        {
            return MewtocolSP.IsOpen;
        }
        /// <summary>
        /// 松下PLC站号
        /// </summary>
        public short Station
        {
            get { return MC.Station; }
            set { MC.Station = value; }
        }
        /// <summary>
        /// 关闭PLC操作对象的串口连接
        /// </summary>
        /// <returns></returns>
        public bool MewtocolOpen()
        {
            if (!MewtocolSP.IsOpen)
            {
                try
                {
                    MewtocolSP.Open();     //打开串口
                    return true;
                }
                catch (System.Exception ex)
                {
                    //MessageBox.Show("错误:" + "串口设置不正确,请重新确认", "串口设置错误");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 关闭PLC操作对象的串口连接
        /// </summary>
        /// <returns></returns>
        public bool MewtocolClose()
        {
            try
            {
                MewtocolSP.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        private string _send, _receive;
        /// <summary>
        /// 读取上一条通讯指令发出的字符串
        /// </summary>
        /// <returns></returns>
        public string GetSendStr()
        {
            return _send;
        }
        /// <summary>
        /// 读取上一条通讯指令收到的字符串
        /// </summary>
        /// <returns></returns>
        public string GetReceiveStr()
        {
            return _receive;

        }
        /// <summary>
        /// 异步发送指令 返回结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public  async Task<string> CmdAsync(string str)
        {
            _receive = await Task.Run(() => Cmd(str));
            return _receive;
        }
        /// <summary>
        /// 发送控制指令，返回结果
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Cmd(string str)
        {
            serialReceiework = true;
            MewtocolSP.WriteLine(str);
            _send = str;
            DateTime dt = DateTime.Now;
            Thread.Sleep(5);
            while (serialReceiework)
            {
                Thread.Sleep(5);
                if (DateTime.Now.Subtract(dt).TotalMilliseconds > 3000) //如果3秒后仍然无数据返回，则视为超时
                {
                    //throw new Exception("主版无响应");
                    // MessageBox.Show("通讯连接断开");
                    serialReceiework = false;
                    _receive = "";                 
                }
            }
          
            //serialReceie =new string() string.Empty;
            return _receive;
        }

        /// <summary>
        /// 发送控制指令，返回结果 重写 2022.2.26
        /// 不需要事件 直接通过串口
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string TCmd(string str) {

            lock (_lockObj) {
                MewtocolSP.DiscardInBuffer();
                MewtocolSP.WriteLine(str);
                _send = str;
                DateTime dt = DateTime.Now;

                while (MewtocolSP.BytesToRead == 0) {
                    Task.Delay(10).Wait();
                    if (DateTime.Now.Subtract(dt).TotalMilliseconds > 1000) //如果1秒后仍然无数据返回，则视为超时
                    {
                        _receive = "";
                    }
                }
                while (MewtocolSP.BytesToRead > 0) {
                    _receive = MewtocolSP.ReadLine().Trim();
                    MewtocolSP.DiscardInBuffer();
                }

            }

            //serialReceie =new string() string.Empty;
            return _receive;
        }
        /// <summary>
        ///  读取单个触点
        /// </summary>
        /// <param name="address">触点地址</param>
        public bool ReadSinglePoint(string address)
        {
           
            string receiveData = Cmd(MC.RCS(address));
            string s = receiveData.Substring(6, 1);
            if (s == "1") return true;
            else if (s == "0") return false;
            else
            {
                // MessageBox.Show("通讯返回错误");
            }

            return false;
        }
        /// <summary>
        /// 写入单个触点
        /// </summary>
        /// <param name="address">触点地址</param>
        /// <param name="value">要写入的值true/false</param>
        /// <returns></returns>
        public void WriteSinglePoint(string address, bool value)
        {
            string receiveData = Cmd(MC.WCS(address, value));
        }
        /// <summary>
        /// 按字单位读取触点值
        /// </summary>
        /// <param name="address">要读取的字单位地址</param>
        /// <returns></returns>
        public short ReadWordPoint(string address)
        {
            int n = 1;
            short[] returnValue = new short[n];
          
            string receiveData = Cmd(MC.RCC(address, n));
            if (receiveData[3].ToString() == "$")
            {
                string registerData = receiveData.Substring(6, 4 * n);
                byte[] byteData = SoftBasic.HexStringToBytes(registerData);

                for (int i = 0; i < n; i++)
                {
                    returnValue[i] = (short)(byteData[i * 2 + 1] << 8 | byteData[2 * i] & 0xff);  //得到short数组 ：由byte数组转为PC低位在前的存储方式               
                }
            }
            else
            {
                //MessageBox.Show("通讯返回出错");
            }
            short returnData = returnValue[0];
            return returnData;
        }
        /// <summary>
        /// 按字单位读取多个字单位的触点值
        /// </summary>
        /// <param name="address">字单位 起始点</param>
        /// <param name="n">读取字数</param>
        /// <returns></returns>
        public short[] ReadWordsPoint(string address, int n)
        {
            short[] returnValue = new short[n];
            
            string receiveData = Cmd(MC.RCC(address, n));
            if (receiveData[3].ToString() == "$")
            {
                string registerData = receiveData.Substring(6, 4 * n);
                byte[] byteData = SoftBasic.HexStringToBytes(registerData);

                for (int i = 0; i < n; i++)
                {
                    returnValue[i] = (short)(byteData[i * 2 + 1] << 8 | byteData[2 * i] & 0xff);  //得到short数组 ：由byte数组转为PC低位在前的存储方式               
                }
            }
            else
            {
                //MessageBox.Show("通讯返回出错");
            }
            return returnValue;

        }
        /// <summary>
        /// 按字单位写入多字的触点值
        /// </summary>
        /// <param name="address">字单位 起始点</param>
        /// <param name="n">写入字数</param>
        /// <param name="inData">写入字数组</param>
        public void WriteWordsPoint(string address, int n, short[] inData)
        {
            string receiveData = Cmd(MC.WCC(address, n, inData));
        }
        /// <summary>
        /// 按字单位写入单字的触点值
        /// </summary>
        /// <param name="address">写入点字单位地址</param>
        /// <param name="inValue">写入值（short型）</param>
        public void WriteSingleWordPoint(string address, short inValue)
        {
            short[] inData = new short[1];
            inData[0] = inValue;
            string receiveData = Cmd(MC.WCC(address, 1, inData));

        }
        /// <summary>
        /// 读取连续多个数据寄存器值
        /// </summary>
        /// <param name="address">数据寄存器地址</param>
        /// <param name="n">读取字个数</param>
        /// <returns></returns>
        public short[] ReadWordDatas(string address, int n)
        {
            short[] returnValue = new short[n];
            
            string receiveData = Cmd(MC.RD(address, n));
            if (receiveData[3].ToString() == "$")
            {
                string registerData = receiveData.Substring(6, 4 * n);
                byte[] byteData = SoftBasic.HexStringToBytes(registerData);

                for (int i = 0; i < n; i++)
                {
                    returnValue[i] = (short)(byteData[i * 2 + 1] << 8 | byteData[2 * i] & 0xff);  //得到short数组 ：由byte数组转为PC低位在前的存储方式               
                }
            }
            else
            {
                // MessageBox.Show("通讯返回出错");
            }
            return returnValue;
        }
        /// <summary>
        /// 读取单个数据寄存器（16位）
        /// </summary>
        /// <param name="address">数据寄存器地址</param>
        /// <returns></returns>
        public short ReadSingleData(string address)
        {
            int n = 1;
            short[] returnValue = new short[n];
           

            string receiveData = Cmd(MC.RD(address, n));
            if (receiveData[3].ToString() == "$")
            {
                string registerData = receiveData.Substring(6, 4 * n);
                byte[] byteData = SoftBasic.HexStringToBytes(registerData);

                for (int i = 0; i < n; i++)
                {
                    returnValue[i] = (short)(byteData[i * 2 + 1] << 8 | byteData[2 * i] & 0xff);  //得到short数组 ：由byte数组转为PC低位在前的存储方式               
                }
            }
            else
            {
                // MessageBox.Show("通讯返回出错");
            }
            short returnData = returnValue[0];
            return returnData;
        }
        /// <summary>
        /// 读取双字数据寄存器
        /// </summary>
        /// <param name="address">寄存器地址</param>
        /// <returns></returns>
        public int ReadDoubleData(string address)
        {
            int n = 2;
            short[] returnValue = new short[n];
           

            string receiveData = Cmd(MC.RD(address, n));
            if (receiveData[3].ToString() == "$")
            {
                string registerData = receiveData.Substring(6, 4 * n);
                byte[] byteData = SoftBasic.HexStringToBytes(registerData);

                for (int i = 0; i < n; i++)
                {
                    returnValue[i] = (short)(byteData[i * 2 + 1] << 8 | byteData[2 * i] & 0xff);  //得到short数组 ：由byte数组转为PC低位在前的存储方式               
                }
            }
            else
            {
                // MessageBox.Show("通讯返回出错");
            }
            int returnDoubleData = returnValue[1] * 0x100 + returnValue[0];
            return returnDoubleData;
        }

        /*  析构函数  应该可以不用
                bool disposed = false;
                public void Dispose()
                {
                    Dispose(true);
                    GC.SuppressFinalize(this);
                }
                /// <summary>
                /// 析构函数
                /// </summary>
                ~PanasonicPLC()
                {
                    Dispose(false);
                }
                protected virtual void Dispose(bool disposing)
                {
                    if (disposed == false)
                    {
                        if (disposing == true)
                        {
                            MewtocolClose();
                        }
                        disposing = true;
                    }
                }
                */
    }
}

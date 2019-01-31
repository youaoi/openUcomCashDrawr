using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace openUcomCashDrawr
{
    class Program
    {
        static int Main(string[] args)
        {
            string name;
            string[] names = SerialPort.GetPortNames();
            if (args.Length == 0)
            {
                var s = new Properties.Settings();
                name = s.comName;
                if (name == "")
                {
                    WritePortNames(names);
                    Console.WriteLine("[ERROR] Please specify COM port name as argument.");
                    return 0x1;
                }
            }
            else
            {
                name = args[0];
            }

            // 接続されたCOMポート名一覧に引数がなければエラー
            if (! names.Contains(name))
            {
                Console.WriteLine("[ERROR] The specified COM port does not exist.");
                Console.WriteLine($"        Port: {name}");
                return 0x2;
            }

            if (! SendPacket(name))
            {
                Console.WriteLine("[ERROR] Failed to send packet.");
                return 0x3;
            }

            Console.WriteLine("[INFO] Done.");
            return 0;
        }

        static void WritePortNames(string[] names)
        {
            Console.WriteLine("[INFO] Displays the name of the connected COM port.");
            
            foreach (string n in names)
            {
                Console.WriteLine($"       Port: {n}");
            }

            if (names.Length == 0)
            {
                Console.WriteLine("[ERROR] COM port list is empty.");
            }
        }

        /// <returns></returns>
        /// <summary>
        /// シリアルポートを開き、ドロワを開放するパケットを送信する。
        /// </summary>
        /// <param name="name">COMポート名</param>
        /// <returns></returns>
        static bool SendPacket(string name)
        {

            char c7 = (char)7;
            SerialPort sp = new SerialPort(name, 9600, Parity.None, 8, StopBits.Two);

            try
            {
                Console.WriteLine("[INFO] Open the COM port connection.");
                Console.WriteLine($"      Port: {name}");
                sp.Open();
                Console.WriteLine("[INFO] Send to open drawer packet.");
                sp.Write(c7.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"[ERROR] {e.Message}");
                return false;
            }
            finally
            {
                Console.WriteLine("[INFO] Close the COM port connection.");
                sp.Close();
            }
            return true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PROJECT
{
    class Notifier
    {
        private static SerialPort port = new SerialPort();
        private static clsSMS objclsSMS = new clsSMS();


        public static string comPort { get; set; }
        private static int boundRate = 921600;
        private static int dataBits = 8;
        private static int readTimeout = 300;
        private static int writeTimeOut = 300;
        private static bool isObjCreated = false;

        public Notifier()
        {
            if (!isObjCreated)
            {
                
                port = objclsSMS.OpenPort(comPort, boundRate, dataBits, readTimeout, writeTimeOut);
                isObjCreated = true;
            }

        }


        public bool sendSMS(string number, string message)
        {
            try
            {
                if (port != null)
                    if (objclsSMS.sendMsg(port, number, message))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                else
                    return false;
            }
            catch  
            { 
                return false;
            }
        }
    }
}

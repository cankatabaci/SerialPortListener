using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;

namespace Console_SerialPortListener
{

    public class Program 
    {
        internal static string portName = "COM1";
        internal static int baudRate = 9600;
        internal static int dataBits = 8;
        internal static Parity parity = Parity.None;
        internal static StopBits stopBits = StopBits.One;

        static void Main(string[] args)
        {
            //Default Port Settings
            SerialPort mySerialPort = new SerialPort(portName)
            {
                BaudRate = baudRate,
                Parity = parity,
                StopBits = stopBits,
                DataBits = dataBits
            };

            

            int menuChoice = 0;
            while(menuChoice != 3)
            {
                Console.WriteLine("Press 1 if you want to continue with the default settings");
                Console.WriteLine("Press 2 if you want to enter your own settings ");
                Console.WriteLine("Press 3 Exit!");
                menuChoice = int.Parse(Console.ReadLine());

                switch (menuChoice)
                {
                    case 1:
                        WriteMySerialPortSettings(menuChoice);
                        SerialPortListenDefault(menuChoice, mySerialPort);
                        break;
                    case 2:
                        SerialPortListenDefault(menuChoice, CustomSerialPortMenu(menuChoice));
                        break;
                    case 3:
                        Environment.Exit(-1);
                        break;
                    default:
                        Console.WriteLine("Sorry, invalid selection\n\n");
                        break;
                }
            }
        }

        private static void WriteMySerialPortSettings(int type)
        {
            string name = "";
            if (type == 1)
                name = "Default";
            else if (type == 2)
                name = "Custom";

            Console.WriteLine("\n**INFO**");
            Console.WriteLine("Your " + name +" serial port settings:");
            Console.WriteLine("Port Name: " + portName);
            Console.WriteLine("Baud Rate: " + baudRate);
            Console.WriteLine("Data Bits: " + dataBits);
            Console.WriteLine("Parity: " + parity.ToString());
            Console.WriteLine("Stop Bits: " + stopBits.ToString());
            Console.WriteLine("");
        }

        private static SerialPort  CustomSerialPortMenu(int type)
        {
            int tempParity = 0;
            int tempStopBits = 0;

            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }

            Console.WriteLine("Enter port name: ");
            portName = Console.ReadLine();
            Console.WriteLine("Enter Baud Rate: (for example: 9600)");
            baudRate = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Data Bits: (for example: 8)");
            dataBits = int.Parse(Console.ReadLine());
            Console.WriteLine("Select your parity:");
            Console.WriteLine("1. None");
            Console.WriteLine("2. Odd");
            Console.WriteLine("3. Even");
            Console.WriteLine("4. Mark");
            Console.WriteLine("5. Space");
            tempParity = int.Parse(Console.ReadLine());

            if (tempParity == 1)
                parity = Parity.None;
            else if (tempParity == 2)
                parity = Parity.Odd;
            else if (tempParity == 3)
                parity = Parity.Even;
            else if (tempParity == 4)
                parity = Parity.Mark;
            else if (tempParity == 5)
                parity = Parity.Space;
            else
                throw new Exception("Wrong Parity Type");

            Console.WriteLine("Select your Stop Bits");
            Console.WriteLine("1. None");
            Console.WriteLine("2. One");
            Console.WriteLine("3. Two");
            Console.WriteLine("4. OnePointFive");
            tempStopBits = int.Parse(Console.ReadLine());

            if (tempStopBits == 1)
                stopBits = System.IO.Ports.StopBits.One;
            else if (tempStopBits == 2)
                stopBits = System.IO.Ports.StopBits.One;
            else if (tempStopBits == 3)
                stopBits = System.IO.Ports.StopBits.Two;
            else if (tempStopBits == 4)
                stopBits = System.IO.Ports.StopBits.OnePointFive;
            else
                throw new Exception("Wrong Stop Bits Type");

            SerialPort mySerialPort = new SerialPort(portName)
            {
                BaudRate = baudRate,
                Parity = parity,
                StopBits = stopBits,
                DataBits = dataBits
            };
            WriteMySerialPortSettings(type);

            return mySerialPort;

        }

        private static void SerialPortListenDefault(int choice, SerialPort sP)
        {

            sP.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            sP.Open();

            Console.WriteLine("Press any key to return menu");
            Console.WriteLine("listening now..");
            Console.ReadKey();
            sP.Close();
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = (SerialPort)sender;
            string data = serialPort.ReadExisting();
            Console.WriteLine("Data: " + data);
        }

    }
}

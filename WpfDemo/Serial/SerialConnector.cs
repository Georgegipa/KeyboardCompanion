using System;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;

namespace Serial
{
    public class SerialConnector
    {
        private const int ResponseTime = 50;
        private SerialPort _comPort;
        private string _portName;
        public string BoardName { get; private set; }
        public string Protocol { get; private set; }
        public string ProtocolVersion { get; private set; }
        public int Buttons { get; private set; }

        public enum Request
        {
            Name,
            Buttons,
            Protocol,
            ProtocolVersion
        }

        public SerialConnector(string portName)
        {
            if (!SerialScanner.PortExists(portName) && !SerialScanner.PortOpen(portName))
            {
                throw new Exception("Port not found or open");
            }

            _portName = portName;
            _comPort = new SerialPort(portName);
        }

        public void OpenPort(bool verbose = false)
        {
            _comPort.Open();
            if (verbose)
                Console.WriteLine("Port " + _portName + " opened!");
        }

        public void ClosePort(bool verbose = false)
        {
            _comPort.Close();
            if (verbose)
                Console.WriteLine("Port " + _portName + " closed!");
        }

        public bool CheckProtocol(int timeout = 2)
        {
            Stopwatch watch = new Stopwatch();
            var progressFlag = 1;
            bool done = false;
            watch.Start();
            while (!done)
            {
                switch (progressFlag)
                {
                    case 1:
                        MakeRequest(Request.Protocol);
                        break;
                    case 2:
                        MakeRequest(Request.ProtocolVersion);
                        break;
                    case 3:
                        MakeRequest(Request.Name);
                        break;
                    case 4:
                        MakeRequest(Request.Buttons);
                        break;
                    case 5:
                        done = true;
                        break;
                }

                string incoming = _comPort.ReadExisting();
                if (!string.IsNullOrEmpty(incoming))
                {
                    if (incoming.Contains("Protocol"))
                    {
                        Protocol = incoming.Split(':')[1];
                        progressFlag++;
                    }
                    else if (incoming.Contains("Version"))
                    {
                        ProtocolVersion = incoming.Split(':')[1];
                        progressFlag++;
                    }
                    else if (incoming.Contains("Board"))
                    {
                        BoardName = incoming.Split(':')[1];
                        progressFlag++;
                    }
                    else if (incoming.Contains(("Buttons")))
                    {
                        Buttons = int.Parse(incoming.Split(':')[1]);
                        progressFlag++;
                    }

                    Console.WriteLine(incoming);
                }

                //timeout in seconds
                if (watch.ElapsedMilliseconds > 7 * 1000)
                    return false;
            }

            double timeTaken = watch.ElapsedMilliseconds / 1000.00;
            Console.WriteLine("Connection Established! Took: " + timeTaken + "sec");
            Console.WriteLine(Protocol + ":" + ProtocolVersion + " running on board:" + BoardName + " with:" + Buttons +
                              " buttons");
            return !string.IsNullOrEmpty(BoardName) && !string.IsNullOrEmpty(Protocol) &&
                   !string.IsNullOrEmpty(ProtocolVersion); //if name is filled then connection is established
        }

        public string ReadPort()
        {
            Thread.Sleep(1);
            string incoming = _comPort.ReadExisting();
            return !string.IsNullOrEmpty(incoming) ? incoming : null;
        }


        private void WriteChar(int send)
        {
            _comPort.Write("" + send);
            Thread.Sleep(ResponseTime);
        }

        private void MakeRequest(Request r)
        {
            switch (r)
            {
                case Request.Protocol:
                    WriteChar(1);
                    break;
                case Request.ProtocolVersion:
                    WriteChar(2);
                    break;
                case Request.Name:
                    WriteChar(3);
                    break;
                case Request.Buttons:
                    WriteChar(4);
                    break;
            }
        }
    }
}
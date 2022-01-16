using System;
using System.IO.Ports;
using System.Linq;


namespace Serial
{
    public static class SerialScanner
    {
        private static string[] _comPorts;

        public static void RefreshPorts(bool verbose = false)
        {
            _comPorts = SerialPort.GetPortNames();

            if (!verbose) return;
            Console.WriteLine("Found:" + _comPorts.Length + " ports.");
            foreach (var port in _comPorts)
            {
                Console.WriteLine(port);
            }
        }

        public static bool PortExists(string portName)
        {
            RefreshPorts();
            return _comPorts.Contains(portName);
        }

        public static int PortSum()
        {
            RefreshPorts();
            return _comPorts.Length;
        }

        public static bool PortOpen(string portName)
        {
            return new SerialPort(portName).IsOpen;
        }

        public static bool ExistsOpen(string portName)
        {
            return PortOpen(portName) && PortExists(portName);
        }
    }

}
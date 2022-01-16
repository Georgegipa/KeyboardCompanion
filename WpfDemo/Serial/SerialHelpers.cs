namespace Serial
{
    public static class SerialHelpers
    {
        private const string protocolPrefix = "->";

        /**
         * Checks if the given string is coming from a supported protocol.
         * If not returns null.
         */
        public static string RemoveProtocolPrefix(string incoming)
        {
            return FromProtocol(ref incoming) ? incoming.Substring(2) : null;
        }

        public static bool FromProtocol(ref string incoming)
        {
            return incoming.Substring(0, 2).Equals(protocolPrefix);
        }
    }
}
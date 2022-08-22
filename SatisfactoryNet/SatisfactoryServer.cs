using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;

namespace SatisfactoryNet
{
    public class SatisfactoryServer : IDisposable
    {
        private static readonly byte[] BeaconBytes = new byte[] { 0x42, 0x43, 0x4f, 0x4e, 0x02 };

        private UdpClient UdpClient { get; }

        public IPEndPoint Endpoint { get; }

        public SatisfactoryServer(string hostname, int port)
        {
            if (!IPAddress.TryParse(hostname, out IPAddress? address))
            {
                address = Dns.GetHostEntry(hostname)
                    .AddressList
                    .First();
            }

            Endpoint = new IPEndPoint(address, port);
            UdpClient = new UdpClient(address.AddressFamily);
            UdpClient.Connect(Endpoint);
        }

        public void Ping()
        {

        }

        public void Query()
        {

        }

        public void UnknownNetworkFunction1()
        {
            var packetData = new byte[10];

            var unknownData = new byte[8];
            RandomNumberGenerator.Fill(unknownData);
            Array.Copy(unknownData, 0, packetData, 2, unknownData.Length);

            Console.WriteLine(FormatPacketPayload(packetData, true));
            UdpClient.Send(packetData);

            // -----

            IPEndPoint sourceEndpoint = Endpoint;
            packetData = UdpClient.Receive(ref sourceEndpoint);
            Console.WriteLine(FormatPacketPayload(packetData, false));
        }

        public string FormatPacketPayload(byte[] payload, bool outbound)
        {
            var localEndpoint = UdpClient.Client.LocalEndPoint;
            var remoteEndpoint = UdpClient.Client.RemoteEndPoint;
            string packetDirectionIndicator = outbound ? "->" : "<-";

            string payloadHexString = Convert.ToHexString(payload).ToLower();

            return $"[{localEndpoint} {packetDirectionIndicator} {remoteEndpoint}]: {payloadHexString}";
        }

        public void Dispose()
        {
            if (UdpClient != null)
            {
                UdpClient.Dispose();
            }
        }
    }
}
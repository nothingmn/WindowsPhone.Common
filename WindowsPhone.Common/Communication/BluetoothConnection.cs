using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Networking.Proximity;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using WindowsPhone.Common.Communication.Channels;
using WindowsPhone.Contracts.Communication;

namespace WindowsPhone.Common.Communication
{
    public class BluetoothConnection : IConnection
    {
        public BluetoothConnection(IChannel channel = null)
        {
            if(channel!=null) _channel = channel;
        }

        public event Received OnReceived;

        private string _peer;
        private StreamSocket _socket = null;
        private DataWriter _dataWriter = null;
        private DataReader _dataReader = null;
        private IChannel _channel = new ByteArrayChannel();

        public async void Open(string peer)
        {
            _peer = peer;
            await SetupDeviceConn();
        }
        private async void ReadData()
        {
            while (true)
            {
                List<Byte> bigBuffer = new List<byte>();
                while (_dataReader.UnconsumedBufferLength > 0)
                {
                    var buffer = new byte[_dataReader.UnconsumedBufferLength];
                    _dataReader.ReadBytes(buffer);
                    bigBuffer.AddRange(buffer);
                }
                if (bigBuffer.Count > 0)
                {
                    object data = _channel.ConvertTo(bigBuffer.ToArray());
                    if (OnReceived != null) OnReceived(data, _socket, null, DateTime.Now);
                }
                else
                {
                    await Task.Delay(100);
                }
            }
        }
        public bool IsOpen { get { return _connected; } }
        private bool _connected = false;


        public void Close()
        {
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Send(object data)
        {
            if (_connected)
            {
                var bytes = _channel.ConvertFrom(data);
                _dataWriter.WriteBytes(bytes);
            }
        }


        private async Task<bool> SetupDeviceConn()
        {
            //Connect to your paired host PC using BT + StreamSocket (over RFCOMM)
            PeerFinder.AlternateIdentities["Bluetooth:PAIRED"] = "";

            var devices = await PeerFinder.FindAllPeersAsync();

            if (devices.Count == 0)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            var peerInfo = devices.FirstOrDefault(c => c.DisplayName.Contains(_peer));
            if (peerInfo == null)
            {
                MessageBox.Show("No paired device");
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings-bluetooth:"));
                return false;
            }

            _socket = new StreamSocket();

            //"{00001101-0000-1000-8000-00805f9b34fb}" - is the GUID for the serial port service.
            await _socket.ConnectAsync(peerInfo.HostName, "{00001101-0000-1000-8000-00805f9b34fb}");

            _dataWriter = new DataWriter(_socket.OutputStream);
            _dataReader = new DataReader(_socket.InputStream);
            _dataReader.InputStreamOptions = InputStreamOptions.Partial;

            ReadData();

            _connected = true;
            return true;
        }

    }
}

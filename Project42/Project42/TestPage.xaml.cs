using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WindowsBluetooth;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Project42
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {

        private ObservableCollection<DeviceInformation> _Devices
        {
            get { return BTWorker?._device; }
        }

        private StreamSocket _socket;
        BlueToothBackgroundWorker BTWorker;
        private RfcommDeviceService _service;

        DataWriter dw;
        DataReader dr;

        public TestPage()
        {
            this.InitializeComponent();

            BTWorker = new BlueToothBackgroundWorker();
        }

        private async void btnSend_Click(object sender,
                                         RoutedEventArgs e)
        {
            /*DataReader reader = null;

            var aqsFilter = SerialDevice.GetDeviceSelector("COM3");
            var devices = await DeviceInformation.FindAllAsync(aqsFilter);
            if (devices.Any())
            {
                var deviceId = devices.First().Id;
                var device = await SerialDevice.FromIdAsync(deviceId);

                if (device != null)
                {
                    device.BaudRate = 57600;
                    device.StopBits = SerialStopBitCount.One;
                    device.DataBits = 8;
                    device.Parity = SerialParity.None;
                    device.Handshake = SerialHandshake.None;

                    reader = new DataReader(device.InputStream);
                }
            }

            Debug.WriteLine(dr.ReadString(dr.UnconsumedBufferLength));*/

            int dummy;

            if (!int.TryParse(tbInput.Text, out dummy))
            {
                tbError.Text = "Invalid input";
            }

            var noOfCharsSent = await Send(tbInput.Text);

            if (noOfCharsSent != 0)
            {
                tbError.Text = noOfCharsSent.ToString();
            }
        }
        private async Task<uint> Send(string msg)
        {
            tbError.Text = string.Empty;

            try
            {
                dw.WriteString("1");

                var store = dw.StoreAsync().AsTask();

                Debug.WriteLine(dr.ReadString(dr.UnconsumedBufferLength));
                
                return await store;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;

                return 0;
            }
        }

        private async void btnConnect_Click(object sender,
                                            RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                var devices = await DeviceInformation.FindAllAsync(RfcommDeviceService.GetDeviceSelector(RfcommServiceId.SerialPort));

                DeviceInformation temp = ((DeviceInformation)t.SelectedItem) ?? _Devices[0];

                if (temp == null)
                    throw new Exception("nefunguje toooooooooooooooooooo");

                var device = devices.FirstOrDefault(x => x.Id.Split('-')[1].Split('#')[0] == temp.Id.Split('-')[1]);

                if (device == null)
                    throw new Exception("nefunguje toooooooooooooooooooo");

                _service = await RfcommDeviceService.FromIdAsync(device.Id);
                _socket = new StreamSocket();
                
                await _socket.ConnectAsync(_service.ConnectionHostName,
                                           _service.ConnectionServiceName,
                                           SocketProtectionLevel.BluetoothEncryptionAllowNullAuthentication);


                dw = new DataWriter(_socket.OutputStream);
               dr = new DataReader(_socket.InputStream);

                

                //string result = dr.ReadString(4);

                
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }

        private async void btnDisconnect_Click(object sender,
                                             RoutedEventArgs e)
        {
            tbError.Text = string.Empty;

            try
            {
                await _socket.CancelIOAsync();
                _socket.Dispose();
                _socket = null;
                _service.Dispose();
                _service = null;
            }
            catch (Exception ex)
            {
                tbError.Text = ex.Message;
            }
        }
    }
}

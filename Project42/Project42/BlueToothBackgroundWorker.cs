using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Bluetooth.Rfcomm;
using Windows.Devices.Enumeration;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Project42
{
    class BlueToothBackgroundWorker
    {
        public ObservableCollection<DeviceInformation> _device = new ObservableCollection<DeviceInformation>();

        private byte[] DEMO_DESTINATION_ONE = { 0x20, 0x16, 0x04, 0x11, 0x48, 0x82 } ; //0x201604114882;
        private byte[] DEMO_DESTINATION_TWO = { 0x20, 0x16, 0x04, 0x11, 0x48, 0x82 } ; //0x201604114882;

        Timer forceUpdate;

        public BlueToothBackgroundWorker()
        {
            forceUpdate = new Timer(Scan, null, 0, 60000);
        }

        private async void Scan(object state)
        {
            List<DeviceInformation> temp = new List<DeviceInformation>();

            temp.AddAll((await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelector())).ToList());
            temp.AddAll((await DeviceInformation.FindAllAsync(BluetoothDevice.GetDeviceSelectorFromPairingState(false))).ToList());
            
            foreach(var item in temp.ToList())
            {
                if (!Compare(item))
                    temp.Remove(item);
            }

            lock (_device)
            {
                _device = new ObservableCollection<DeviceInformation>(temp); //.AddAll(temp_2);
            }

            foreach(var i in _device)
            {
                if (!i.Pairing.IsPaired && i.Pairing.CanPair)
                {
                    DevicePairingKinds pairingKind = DevicePairingKinds.ConfirmOnly;

                    DeviceInformationCustomPairing customPairing = i.Pairing.Custom;

                    customPairing.PairingRequested += cancer;

                    var pairingResult = await customPairing.PairAsync(pairingKind, DevicePairingProtectionLevel.EncryptionAndAuthentication);

                    Debug.WriteLine(pairingResult.Status);
                }
            }
        }

        private void cancer(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args)
        {
            args.Accept();
        }

        private bool Compare(DeviceInformation hledanejCancer)
        {
            Debug.WriteLine(string.Format($"{hledanejCancer.Id} {hledanejCancer.Name} {hledanejCancer.Kind}"));

            string[] temp = hledanejCancer.Id.Split('-')[1].ToUpper().Split(':');

            byte[] MAC = new byte[temp.Length]; //temp.Select().ToArray();

            for (int i = 0; i < MAC.Length; i++)
                MAC[i] = temp[i].StringToByte();

            return MACComparator(MAC, DEMO_DESTINATION_ONE);
            /*if(MACComparator(MAC, DEMO_DESTINATION_TWO))
            {
                try
                {
                    var service = await GattDeviceService.FromIdAsync(device.Id);
                    Debug.WriteLine("Opened Service!!");
                }
                catch
                {
                    Debug.WriteLine("Failed to open service.");
                }
            }*/
        }
        

        private bool MACComparator(byte[] MAC_1, byte[] MAC_2)
        {
            if (MAC_1.Length != 6 || MAC_2.Length != 6)
                throw new Exception("One of arrays is not MAC");

            for (int i = 0; i < MAC_1.Length; i++)
                if (MAC_1[i] != MAC_2[i])
                    return false;

            return true;
        }
    }
}

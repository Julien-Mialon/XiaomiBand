using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.ViewManagement;

namespace XiaomiBand
{
	public class MiBand
	{
		BluetoothLEDevice _device;

		public MiBand(BluetoothLEDevice device)
		{
			_device = device;

			Debug.WriteLine($"Name : {device.Name}");
			Debug.WriteLine($"Device connection status : {device.ConnectionStatus}");


			_device.ConnectionStatusChanged += OnConnectionStatusChanged;
			_device.GattServicesChanged += OnGattServicesChanged;
			_device.NameChanged += OnNameChanged;
		}

		private void OnNameChanged(BluetoothLEDevice sender, object args)
		{
			Debug.WriteLine("Name changed : " + _device.Name);
		}

		private void OnConnectionStatusChanged(BluetoothLEDevice sender, object args)
		{
			Debug.WriteLine("Connection status changed : " + _device.ConnectionStatus);
		}

		private void OnGattServicesChanged(BluetoothLEDevice sender, object args)
		{
			Debug.WriteLine("Gatt services changed");
		}

		public async Task PairAsync()
		{
			GattDeviceService service = GetMiliService();

			GattCharacteristic chr = service.GetCharacteristics(Constants.CHAR_PAIR).SingleOrDefault();
			
			Debug.WriteLine("Pair protection level : " + chr.ProtectionLevel);
			await chr.WriteValueAsync(new byte[] {2}.AsBuffer());
		}

		public async Task<string> ReadNameAsync()
		{
			GattDeviceService service = GetMiliService();

			GattCharacteristic chr = service.GetCharacteristics(Constants.CHAR_DEVICE_NAME).SingleOrDefault();

			GattReadResult read = await chr.ReadValueAsync(BluetoothCacheMode.Uncached);
			if (read.Status == GattCommunicationStatus.Success)
			{
				byte[] name = read.Value.ToArray();

				Debug.WriteLine($"Name length : {name.Length}");
				foreach (byte b in name)
				{
					Debug.WriteLine((char) b);
				}


				string result = Encoding.UTF8.GetString(name, 0, name.Length);
				Debug.WriteLine($"name : {result}");

				return result;
			}
			else
			{
				Debug.WriteLine("Unable to read name");
			}
			return null;
		}

		public async Task<Battery> ReadBatteryAsync()
		{
			GattDeviceService service = GetMiliService();

			GattCharacteristic chr = service.GetCharacteristics(Constants.CHAR_BATTERY).SingleOrDefault();

			GattReadResult read = await chr.ReadValueAsync(BluetoothCacheMode.Uncached);
			if (read.Status == GattCommunicationStatus.Success)
			{
				byte[] batteryResult = read.Value.ToArray();

				Debug.WriteLine($"battery length : {batteryResult.Length}");
				
				Battery b = new Battery(batteryResult);

				Debug.WriteLine(b);

				return b;
			}
			Debug.WriteLine("Unable to read battery");
			return null;
		}

		public async Task<int> ReadStepCountAsync()
		{
			GattDeviceService service = GetMiliService();

			GattCharacteristic chr = service.GetCharacteristics(Constants.CHAR_REALTIME_STEPS).SingleOrDefault();

			GattReadResult read = await chr.ReadValueAsync(BluetoothCacheMode.Uncached);
			if (read.Status == GattCommunicationStatus.Success)
			{
				byte[] steps = read.Value.ToArray();
				return 0xff & steps[0] | (0xff & steps[1]) << 8;
			}
			Debug.WriteLine("Unable to read step count");
			return -1;
		}

		public async Task<BLEParams> ReadBLEParamsAsync()
		{
			GattDeviceService service = GetMiliService();
			GattCharacteristic chr = service.GetCharacteristics(Constants.CHAR_LE_PARAMS).SingleOrDefault();
			GattReadResult read = await chr.ReadValueAsync(BluetoothCacheMode.Uncached);
			if (read.Status == GattCommunicationStatus.Success)
			{
				byte[] result = read.Value.ToArray();

				Debug.WriteLine($"bleparams length : {result.Length}");

				BLEParams p = new BLEParams(result);

				Debug.WriteLine(p);

				return p;
			}
			Debug.WriteLine("Unable to read ble params");
			return null;
		}

		private GattDeviceService GetMiliService()
		{
			return _device.GetGattService(Constants.ROOT_SERVICE_GUID);
		}
	}
}

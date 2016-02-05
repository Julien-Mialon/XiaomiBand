using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.UI;
using XiaomiBand.Sdk.Model;
using XiaomiBand.Sdk.Protocol;

namespace XiaomiBand.Sdk
{
	public class MiBand
	{
		readonly BluetoothIO _io;
		readonly BluetoothLEDevice _device;

		public MiBand(BluetoothLEDevice device)
		{
			_device = device;
			_io = new BluetoothIO(device);

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

		public async Task<bool> PairAsync()
		{
			if (await _io.WriteValueAsync(Features.Pair, ProtocolData.Pair))
			{
				Debug.WriteLine("Pairing OK");
				return true;
			}

			Debug.WriteLine("Error while pairing");
			return false;
		}

		public async Task<string> ReadNameAsync()
		{
			byte[] value = await _io.ReadValueAsync(Features.DeviceName);
			if (value == null)
			{
				Debug.WriteLine("Can not read name");
				return null;
			}

			string result = Encoding.UTF8.GetString(value, 3, value.Length - 3);
			Debug.WriteLine($"DeviceName={result}");

			return result;
		}

		public async Task<Battery> ReadBatteryAsync()
		{
			byte[] value = await _io.ReadValueAsync(Features.Battery);
			if (value == null)
			{
				Debug.WriteLine("Can not read battery info");
				return null;
			}

			Battery battery = new Battery(value);
			Debug.WriteLine($"Battery : {battery}");
			return battery;
		}

		public async Task<int> ReadStepCountAsync()
		{
			byte[] value = await _io.ReadValueAsync(Features.RealtimeSteps);
			if (value == null)
			{
				Debug.WriteLine("Can not read step count");
				return -1;
			}

			int count = 0xff & value[0] | (0xff & value[1]) << 8;
			Debug.WriteLine($"Step count {count}");
			return count;
		}

		public async Task<BLEParams> ReadBLEParamsAsync()
		{
			byte[] value = await _io.ReadValueAsync(Features.LeParams);
			if (value == null)
			{
				Debug.WriteLine("Can not read ble params");
				return null;
			}

			BLEParams param = new BLEParams(value);
			Debug.WriteLine($"BLEParams : {param}");
			return param;
		}

		public async Task StartVibrateAsync(bool withLed = false)
		{
			byte[] data = withLed ? ProtocolData.VibrationWithLed : ProtocolData.VibrationWithoutLed;

			if (await _io.WriteValueAsync(Features.ControlPoint, data))
			{
				Debug.WriteLine("Start vibrate Ok");
			}
			else
			{
				Debug.WriteLine("Start vibrate KO");
			}
		}

		public async Task StopVibrateAsync()
		{
			if (await _io.WriteValueAsync(Features.ControlPoint, ProtocolData.StopVibration))
			{
				Debug.WriteLine("Stop vibrate Ok");
			}
			else
			{
				Debug.WriteLine("Stop vibrate KO");
			}
		}
		
		public async Task SetLedAsync(Color color, bool flashing)
		{
			int red = color.R / 42;
			int green = color.G / 42;
			int blue = color.B / 42;

			byte[] data = { ProtocolData.LedStartByte, (byte)red, (byte)green, (byte)blue, flashing ? ProtocolData.LedEndByteTurnFlashOn : ProtocolData.LedEndByteTurnFlashOff };

			if (await _io.WriteValueAsync(Features.ControlPoint, data))
			{
				Debug.WriteLine("Led started Ok");
			}
			else
			{
				Debug.WriteLine("Led started KO");
			}
		}
	}
}

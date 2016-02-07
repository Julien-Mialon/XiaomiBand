using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using XiaomiBand.Sdk.Protocol;

namespace XiaomiBand.Sdk
{
	class BluetoothIO
	{
		private readonly BluetoothLEDevice _device;

		public BluetoothIO(BluetoothLEDevice device)
		{
			_device = device;
		}

		public Task<bool> WriteValueAsync(Guid featureId, byte[] data)
		{
			return WriteValueAsync(Services.Mili, featureId, data);
		}

		public async Task<bool> WriteValueAsync(Guid serviceId, Guid featureId, byte[] data)
		{
			try
			{
				GattDeviceService service = _device.GetGattService(serviceId);
				if (service == null)
				{
					Debug.WriteLine($"Unable to find service {serviceId}");
					return false;
				}

				GattCharacteristic feature = service.GetCharacteristics(featureId).SingleOrDefault();
				if (feature == null)
				{
					Debug.WriteLine($"Unable to find feature {featureId}");
					return false;
				}

				await feature.WriteValueAsync(data.AsBuffer());

				return true;
			}
			catch (Exception e)
			{
				Debug.WriteLine($"Exception : {e.Message} {e.StackTrace}");
			}
			return false;
		}

		public Task<byte[]> ReadValueAsync(Guid featureId)
		{
			return ReadValueAsync(Services.Mili, featureId);
		}

		public async Task<byte[]> ReadValueAsync(Guid serviceId, Guid featureId)
		{
			try
			{
				GattDeviceService service = _device.GetGattService(serviceId);
				if (service == null)
				{
					Debug.WriteLine($"Unable to find service {serviceId}");
					return null;
				}

				GattCharacteristic feature = service.GetCharacteristics(featureId).SingleOrDefault();
				if (feature == null)
				{
					Debug.WriteLine($"Unable to find feature {featureId}");
					return null;
				}

				GattReadResult readResult = await feature.ReadValueAsync();
				if (readResult.Status == GattCommunicationStatus.Unreachable)
				{
					Debug.WriteLine("Unable to read");
					return null;
				}

				return readResult.Value.ToArray();
			}
			catch (Exception e)
			{
				Debug.WriteLine($"Exception : {e.Message} {e.StackTrace}");
			}
			return null;
		}
	}
}

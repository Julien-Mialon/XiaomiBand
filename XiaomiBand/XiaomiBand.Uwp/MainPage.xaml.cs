using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XiaomiBand.Sdk;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace XiaomiBand.Uwp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
	    private MiBand _band;

        public MainPage()
        {
            this.InitializeComponent();

	        Initialize();
        }

	    public async void Initialize()
	    {
			try
			{
				DeviceInformationCollection devices = await DeviceInformation.FindAllAsync();
				DeviceInformation device = devices.FirstOrDefault(x => x.Name == "MI1S");

				if (device == null)
				{
					return;
				}

				Output(device);
				
				Debug.WriteLine("Connecting to device");
				GattDeviceService service = await GattDeviceService.FromIdAsync(device.Id);

				if (service == null)
				{
					return;
				}
				Debug.WriteLine("Connected");

			    IReadOnlyList<GattCharacteristic> features = service.GetAllCharacteristics();
			    Output(features);
				IReadOnlyList<GattDeviceService> includedServices = service.GetAllIncludedServices();
				Output(includedServices);
				

				BluetoothLEDevice bleDevice = service.Device;

				_band = new MiBand(bleDevice);

				await _band.ReadBatteryAsync();
				await _band.ReadBLEParamsAsync();
				await _band.ReadStepCountAsync();
				await _band.ReadNameAsync();

				await _band.SetLedAsync(Colors.Red, true);
				await _band.StartVibrateAsync(true);
				await Task.Delay(1000);
				await _band.StopVibrateAsync();

				//await band.PairAsync();

				//string name = await band.ReadNameAsync();
				//Debug.WriteLine("Got a name ! : " + name);

				/*
				GattDeviceService batteryService = bleDevice.GetGattService(new Guid("0000fee0-0000-1000-8000-00805f9b34fb"));
				Debug.WriteLine("Battery service");
				Output(batteryService);
				*/
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Exception : {ex.Message}");
				Debug.WriteLine($"Trace : {ex.StackTrace}");
			}
		}


		private void Output(IReadOnlyList<GattDeviceService> services)
		{
			Debug.WriteLine("Services : ");
			if (services == null || services.Count == 0)
			{
				Debug.WriteLine("\tNone");
				return;
			}

			foreach (GattDeviceService service in services)
			{
				Output(service);
			}
		}

		private void Output(GattDeviceService service, string tab = "\t")
		{
			if (service == null)
			{
				return;
			}

			Debug.WriteLine($"{tab}DeviceId={service.DeviceId}");
			Debug.WriteLine($"{tab}\tUuid={service.Uuid}");
			Debug.WriteLine($"{tab}\tAttributeHandle={service.AttributeHandle}");
		}

		private void Output(IReadOnlyList<GattCharacteristic> features)
		{
			Debug.WriteLine("Features : ");
			if (features == null || features.Count == 0)
			{
				Debug.WriteLine("\tNone");
				return;
			}

			foreach (GattCharacteristic feature in features)
			{
				Debug.WriteLine($"\tDescription : {feature.UserDescription}");
				Debug.WriteLine($"\t\tProtection : {feature.ProtectionLevel}");
				Debug.WriteLine($"\t\tCharacteristic : {feature.CharacteristicProperties}");
				Debug.WriteLine($"\t\tUuid : {feature.Uuid}");
				Debug.WriteLine($"\t\tAttributeHandle : {feature.AttributeHandle}");

				Debug.WriteLine("\t\tPresentation formats : ");

				/*
				try
				{
					if (feature.PresentationFormats != null)
					{
						foreach (GattPresentationFormat format in feature.PresentationFormats)
						{
							Debug.WriteLine($"\t\t\tDescription={format.Description}");
							Debug.WriteLine($"\t\t\t\tExponent={format.Exponent}");
							Debug.WriteLine($"\t\t\t\tFormatType={format.FormatType}");
							Debug.WriteLine($"\t\t\t\tNamespace={format.Namespace}");
							Debug.WriteLine($"\t\t\t\tUnit={format.Unit}");
						}
					}
				}
				catch (Exception)
				{
					//presentation formats could raise an invalid memory error
				}
				*/

				GattDeviceService service = feature.Service;
				Debug.WriteLine("\t\tService : ");
				if (service == null)
				{
					Debug.WriteLine("\t\t\tNo service");
				}
				else
				{
					Output(service, "\t\t\t");
				}
			}
		}

		private void Output(DeviceInformation device)
		{
			Debug.WriteLine($"Device : \n\tName={device.Name}\n" +
			                $"\tId={device.Id}\n" +
			                $"\tIsDefault={device.IsDefault}\n" +
			                $"\tIsEnabled={device.IsEnabled}\n" +
			                $"\tKind={device.Kind}\n" +
			                $"\tCanPair={device.Pairing.CanPair}\n" +
			                $"\tIsPaired={device.Pairing.IsPaired}\n");

			Debug.WriteLine($"Location : \n\tInDock={device.EnclosureLocation?.InDock}\n\tInLid={device.EnclosureLocation?.InLid}\n\tPanel={device.EnclosureLocation?.Panel}");
			Debug.WriteLine("Properties :");
			foreach (KeyValuePair<string, object> prop in device.Properties)
			{
				Debug.WriteLine($"\t{prop.Key}={prop.Value}");
			}
		}
	}
}

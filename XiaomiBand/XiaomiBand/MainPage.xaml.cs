using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using XiaomiBand.Sdk;

namespace XiaomiBand
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

	    private async void OnConnectClicked(object sender, RoutedEventArgs e)
	    {
			//var serviceUIID = new Guid("F000AA20-0451-4000-B000-000000000000");

			//Find the devices that expose the service 
		    try
		    {
			    DeviceInformationCollection devices = await DeviceInformation.FindAllAsync();
			    DeviceInformation device = devices.FirstOrDefault(x => x.Name == "MI1S");

			    if (device == null)
			    {
				    return;
			    }

			    Output(device);

			    GattDeviceService service = await GattDeviceService.FromIdAsync(device.Id);

			    if (service == null)
			    {
				    return;
			    }

				/*
			    IReadOnlyList<GattCharacteristic> features = service.GetAllCharacteristics();
			    Output(features);
				IReadOnlyList<GattDeviceService> includedServices = service.GetAllIncludedServices();
				Output(includedServices);
				// */

			    BluetoothLEDevice bleDevice = service.Device;

			    _band = new MiBand(bleDevice);
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
		    /*
			var device = devices.FirstOrDefault(d => d.Name == "TI BLE Sensor Tag");

			if (device != null)
			{
				//Connect to the service
				var service = await GattDeviceService.FromIdAsync(device.Id);
			}
			*/
		}

	    private MiBand _band;


	    private async void OnPairClicked(object sender, RoutedEventArgs e)
	    {
		    await _band.PairAsync();
	    }

	    private async void OnNameClicked(object sender, RoutedEventArgs e)
	    {
		    string name = await _band.ReadNameAsync();
			Debug.WriteLine($"Got name #{name}#");
        }

		private async void OnBatteryClicked(object sender, RoutedEventArgs e)
		{
			await _band.ReadBatteryAsync();
		}

		private async void OnStepsClicked(object sender, RoutedEventArgs e)
		{
			int step = await _band.ReadStepCountAsync();
			Debug.WriteLine("Steps : " + step);
		}

		private async void OnParamsClicked(object sender, RoutedEventArgs e)
		{
			await _band.ReadBLEParamsAsync();
		}

		private async void OnReadLedClicked(object sender, RoutedEventArgs e)
		{
			await _band.ReadLedAsync();
		}

		private async void OnStartLedClicked(object sender, RoutedEventArgs e)
		{
			await _band.StartLedAsync(Colors.Red);
		}

		private async void OnStopLedClicked(object sender, RoutedEventArgs e)
		{
			await _band.StopLedAsync(Colors.White);
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
			    foreach (GattPresentationFormat format in feature.PresentationFormats)
			    {
				    Debug.WriteLine($"\t\t\tDescription={format.Description}");
					Debug.WriteLine($"\t\t\t\tExponent={format.Exponent}");
					Debug.WriteLine($"\t\t\t\tFormatType={format.FormatType}");
					Debug.WriteLine($"\t\t\t\tNamespace={format.Namespace}");
					Debug.WriteLine($"\t\t\t\tUnit={format.Unit}");
				}

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
			Debug.WriteLine($"Device : \n\tName={device.Name}\n\tId={device.Id}\n\tIsDefault={device.IsDefault}\n\tIsEnabled={device.IsEnabled}");
			Debug.WriteLine($"Location : \n\tInDock={device.EnclosureLocation?.InDock}\n\tInLid={device.EnclosureLocation?.InLid}\n\tPanel={device.EnclosureLocation?.Panel}");
			Debug.WriteLine("Properties :");
			foreach (KeyValuePair<string, object> prop in device.Properties)
			{
				Debug.WriteLine($"\t{prop.Key}={prop.Value}");
			}
		}
    }
}

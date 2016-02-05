using System;

namespace XiaomiBand.Sdk.Protocol
{
	class Features
	{
		public static readonly Guid DeviceInfo	= new Guid("0000ff01-0000-1000-8000-00805f9b34fb");
		public static readonly Guid DeviceName	= new Guid("0000ff02-0000-1000-8000-00805f9b34fb");

		public static readonly Guid Notification = new Guid("0000ff03-0000-1000-8000-00805f9b34fb");
		public static readonly Guid UserInfo = new Guid("0000ff04-0000-1000-8000-00805f9b34fb");

		public static readonly Guid ControlPoint = new Guid("0000ff05-0000-1000-8000-00805f9b34fb");

		public static readonly Guid RealtimeSteps = new Guid("0000ff06-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Activity = new Guid("0000ff07-0000-1000-8000-00805f9b34fb");

		public static readonly Guid FirmwareData = new Guid("0000ff08-0000-1000-8000-00805f9b34fb");
		public static readonly Guid LeParams = new Guid("0000ff09-0000-1000-8000-00805f9b34fb");
		public static readonly Guid DataTime = new Guid("0000ff0a-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Statistics = new Guid("0000ff0b-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Battery = new Guid("0000ff0c-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Test = new Guid("0000ff0d-0000-1000-8000-00805f9b34fb");
		public static readonly Guid SensorData = new Guid("0000ff0e-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Pair = new Guid("0000ff0f-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Vibration = new Guid("00002a06-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Heartrate = new Guid("00002a39-0000-1000-8000-00805f9b34fb");
	}
}

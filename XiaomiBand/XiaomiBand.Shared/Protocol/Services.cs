using System;

namespace XiaomiBand.Sdk.Protocol
{
	class Services
	{
		public static readonly Guid Mili = new Guid("0000fee0-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Vibration = new Guid("00001802-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Heartrate = new Guid("0000180d-0000-1000-8000-00805f9b34fb");

		//unknown services, do not use them unless you know exactly what you're doing
		public static readonly Guid Unknown1 = new Guid("00001800-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Unknown2 = new Guid("00001801-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Unknown4 = new Guid("0000fee1-0000-1000-8000-00805f9b34fb");
		public static readonly Guid Unknown5 = new Guid("0000fee7-0000-1000-8000-00805f9b34fb");
	}
}

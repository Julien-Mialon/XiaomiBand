namespace XiaomiBand.Sdk.Protocol
{
	class ProtocolData
	{
		public static readonly byte[] Pair = { 2 };
		
		public static readonly byte[] EnableRealtimeStepsNotify = { 3, 1 };
		public static readonly byte[] DisableRealtimeStepsNotify = { 3, 0 };
		public static readonly byte[] EnableSensorDataNotify = { 18, 1 };
		public static readonly byte[] DisableSensorDataNotify = { 18, 0 };

		/*
		public static readonly byte[] VibrationWithLed = { 1 };
		public static readonly byte[] Vibration10TimesWithLed = { 2 };
		public static readonly byte[] VibrationWithoutLed = { 4 };
		public static readonly byte[] StopVibration = { 0 };

		public static readonly byte[] SetColorRed = { 14, 6, 1, 2, 1 };
		public static readonly byte[] SetColorBlue = { 14, 0, 6, 6, 1 };
		public static readonly byte[] SetColorOrange = { 14, 6, 2, 0, 1 };
		public static readonly byte[] SetColorGreen = { 14, 4, 5, 0, 1 };
		*/

		public static readonly byte[] StartHeartRateScan = { 21, 2, 1 };

		public static readonly byte[] Reboot = { 12 };
		public static readonly byte[] RemoteDisconnect = { 1 };
		public static readonly byte[] FactoryReset = { 9 };
		public static readonly byte[] SelfTest = { 2 };

		public static readonly byte LedStartByte = 14;
		public static readonly byte LedEndByteTurnFlashOn = 1;
		public static readonly byte LedEndByteTurnFlashOff = 0;

		public static readonly byte[] VibrationWithLed = { 8, 2 };
		public static readonly byte[] VibrationWithoutLed = { 8, 2 };
		public static readonly byte[] StopVibration = { 19 };
	}
}

namespace XiaomiBand
{
	public class BLEParams
	{
		public int ConnectionIntervalMin { get; set; }

		public int ConnectionIntervalMax { get; set; }

		public int ConnectionInterval { get; set; }

		public int AdvertisingInterval { get; set; }

		public int Latency { get; set; }

		public int Timeout { get; set; }

		public BLEParams(byte[] bytes)
		{
			ConnectionIntervalMin = 0xffff & (0xff & bytes[0] | (0xff & bytes[1]) << 8);
			ConnectionIntervalMax = 0xffff & (0xff & bytes[2] | (0xff & bytes[3]) << 8);
			Latency = 0xffff & (0xff & bytes[4] | (0xff & bytes[5]) << 8);
			Timeout = 0xffff & (0xff & bytes[6] | (0xff & bytes[7]) << 8);
			ConnectionInterval = 0xffff & (0xff & bytes[8] | (0xff & bytes[9]) << 8);
			AdvertisingInterval = 0xffff & (0xff & bytes[10] | (0xff & bytes[11]) << 8);

			ConnectionIntervalMin = (int)(1.25 * ConnectionIntervalMin);
			ConnectionIntervalMax = (int)(1.25 * ConnectionIntervalMax);
			AdvertisingInterval = (int)(0.625 * AdvertisingInterval);
			Timeout *= 10;
		}

		public override string ToString()
		{
			return $"ConnectionInterval={ConnectionInterval}, ConnectionIntervalMin={ConnectionIntervalMin}, ConnectionIntervalMax={ConnectionIntervalMax}, AdvertisingInterval={AdvertisingInterval}, Latency={Latency}, Timeout={Timeout}";
		}
	}
}

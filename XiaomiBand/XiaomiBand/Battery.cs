using System;

namespace XiaomiBand
{
	public enum BatteryStatus
	{
		Undefined = 0,
		Low = 1,
		Charging = 2,
		Full = 3,
		NotCharging = 4
	}

	public class Battery
	{
		public int BatteryLevel { get; set; }

		public int Cycles { get; set; }

		public DateTime LastCharged { get; set; }

		public BatteryStatus Status { get; set; }

		public Battery(byte[] bytes)
		{
			BatteryLevel = bytes[0];
			Status = (BatteryStatus)bytes[9];
			LastCharged = new DateTime(bytes[1] + 2000, bytes[2] + 1, bytes[3], bytes[4], bytes[5], bytes[6]);
			
			Cycles = 0xffff & (0xff & bytes[7] | (0xff & bytes[8]) << 8);
		}

		public override string ToString()
		{
			return $"BatteryLevel={BatteryLevel}%, ChargingCycles={Cycles}, LastCharged={LastCharged.ToString("yyyy-MM-dd hh:mm:ss")}, Status={ToString(Status)}";
		}

		private string ToString(BatteryStatus status)
		{
			switch(status)
			{
				case BatteryStatus.Undefined:
					return "Undefined";
				case BatteryStatus.Low:
					return "Low";
				case BatteryStatus.Charging:
					return "Charging";
				case BatteryStatus.Full:
					return "Full";
				case BatteryStatus.NotCharging:
					return "NotCharging";
				default:
					throw new ArgumentOutOfRangeException(nameof(status), status, null);
			}
		}
	}
}

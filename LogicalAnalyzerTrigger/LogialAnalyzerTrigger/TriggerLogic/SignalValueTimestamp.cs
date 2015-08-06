using ABB.Robotics.Paint.RobView.Database.SignalLog;

namespace LogialAnalyzerTrigger
{
	public class SignalValueTimestamp
	{
		private Signal _signal;
		private double _value;
		private long _timeStamp;

		public SignalValueTimestamp(Signal signal, double value, long timeStamp)
		{
			this._signal = signal;
			this._value = value;
			this._timeStamp = timeStamp;
		}

		public SignalValueTimestamp(Signal signal)
		{
			this._signal = signal;
		}

		internal Signal Signal
		{
			get { return this._signal; }
			set
			{
				this._signal = value;

			}
		}

		public double Value
		{
			get { return this._value; }
			set { this._value = value; }
		}

		public long TimeStamp
		{
			get { return this._timeStamp; }
			set { this._timeStamp = value; }
		}
	}
}
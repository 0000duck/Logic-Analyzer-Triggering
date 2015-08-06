using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogialAnalyzerTrigger
{
    interface ISignalMetaData
    {
        string DisplayName { get; set; }

        double? MaxValue { get; set; }

        double? MinValue { get; set; }

        Signal Signal { get; set; }

        int SignalId { get; set; }

        string Unit { get; set; }

    }

    public enum SignalType
    {
        Analog,

        Digital
    }

    interface ISignal
    {
        string Name { get; }

        int SignalId { get; }

        SignalMetaData SignalMetaData { get; }

        SignalType SignalType { get; }
    }

    public class Signal : ISignal
    {
            public static Signal Create(string displayName, string signalName, double? maxValue, double? minValue, int signaID, string unit, SignalType signalType)
        {
            var meta = new SignalMetaData(displayName, maxValue, minValue , signaID, unit);
            var sig = new Signal(signalName, signaID, meta, signalType);
            return sig;
        }

        private string name;
        private int signalId;
        private SignalMetaData signalMetaData;
        private SignalType signalType;

        public Signal(string name, int signalId, SignalMetaData signalMetaData, SignalType signalType)
        {
            signalMetaData.Signal = this;
            this.name = name;
            this.signalId = signalId;
            this.signalMetaData = signalMetaData;
            this.signalType = signalType;
        }
        public string Name
        {
            get { return this.name; }
        }

        public int SignalId
        {
            get { return this.signalId; }
        }

        public SignalMetaData SignalMetaData
        {
            get { return this.signalMetaData; }
        }

        public SignalType SignalType
        {
            get { return this.signalType; }
        }
    }

    public class SignalMetaData : ISignalMetaData
    {
        private string displayName;
        private double? maxValue;
        private double? minValue;
        private Signal signal;
        private int signalId;
        private string unit;

        public SignalMetaData(string displayName, double? maxValue, double? minValue,  int signalId, string unit)
        {
            this.displayName = displayName;
            this.maxValue = maxValue;
            this.minValue = minValue;
            this.signal = signal;
            this.signalId = signalId;
            this.unit = unit;
        }

        public string DisplayName
        {
            get
            {
                return this.displayName; 
            }
            set
            {
                this.displayName = value;
            }
        }

        public double? MaxValue
        {
            get
            {
                return this.maxValue;
            }
            set
            {
                this.maxValue = value;
            }
        }

        public double? MinValue
        {
            get
            {
                return this.minValue;
            }
            set
            {
                this.minValue = value;
            }
        }

        public Signal Signal
        {
            get
            {
                return this.signal;
            }
            set
            {
                this.signal = value;
            }
        }

        public int SignalId
        {
            get
            {
                return this.signalId;
            }
            set
            {
                this.signalId = value;
            }
        }

        public string Unit
        {
            get
            {
                return this.unit;
            }
            set
            {
                this.unit = value;
            }
        }
    }
}

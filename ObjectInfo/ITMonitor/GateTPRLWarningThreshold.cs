﻿namespace ObjectInfo
{
    public class GateTPRLWarningThreshold
    {

        public GateTPRLWarningThreshold()
        {
            MaxSeqBusinessSendDay = 0;
            MaxSeqBusinessSendSession = 0;
            SeqBusinessSendMorning = 0;
            SeqBusinessAchieve = 0;
        }
        public double MaxSeqBusinessSendDay { get; set; }

        public double MaxSeqBusinessSendSession { get; set; }

        public double SeqBusinessSendMorning { get; set; }

        public double SeqBusinessAchieve { get; set; }

        public string ThresholdSession { get; set; } = string.Empty;

        public string ThresholdDay { get; set; } = string.Empty;

        public string DescriptionSession { get; set; } = string.Empty;

        public string DescriptionDay { get; set; } = string.Empty;

        public string ProcessingTime { get; set; } = string.Empty;

        public string Session { get; set; } = string.Empty;
    }
}

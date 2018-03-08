using System;

namespace Ribe.Infrustructure
{
    /// <summary>
    /// An IdGenerator using "Snowflake" algorithm
    /// </summary>
    public class SnowflakeIdGenerator : IIdGenerator
    {
        private static object SyncRoot = new object();

        const long Twepoch = 1420041600000L;

        const long MachineIdBits = 10;

        const long MaxMachineId = -1 ^ -1 << (int)MachineIdBits;

        const long SequenceBits = 12L;

        const long SequenceMask = -1L ^ -1L << (int)SequenceBits;

        private long _sequence = 0L;

        private long _machineIdShift = SequenceBits;

        private long _timestampLeftShift = SequenceBits + MachineIdBits;

        private long _lastTimestamp = -1L;

        private long _machineId = -1;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="machineId">machineId</param>
        public SnowflakeIdGenerator(int machineId)
        {
            if (machineId > MaxMachineId || machineId < 0)
            {
                throw new ArgumentOutOfRangeException(String.Format("machineId Id can't be greater than {0} or less than 0", MaxMachineId));
            }

            _machineId = machineId;
        }

        /// <summary>
        /// Create an Id
        /// </summary>
        /// <returns>id</returns>
        public long CreateId()
        {
            lock (SyncRoot)
            {
                var timestamp = GetTimeMilliseconds();

                if (_lastTimestamp == timestamp)
                {
                    _sequence = _sequence + 1 & SequenceMask;

                    if (_sequence == 0)
                    {
                        timestamp = GetNextMilliMillisecond(_lastTimestamp);
                    }
                }
                else
                {
                    //if the id was used to mod,more evenly
                    _sequence = new Random(DateTime.UtcNow.Millisecond).Next(0, 9);
                }

                if (timestamp < _lastTimestamp)
                {
                    throw new Exception(String.Format("clock moved backwards.Refusing to generate id for {0} milliseconds", (_lastTimestamp - timestamp)));
                }

                _lastTimestamp = timestamp;

                return timestamp - Twepoch << (int)_timestampLeftShift | +_machineId << (int)_machineIdShift | _sequence;
            }
        }

        private long GetNextMilliMillisecond(long lastTimestamp)
        {
            var timestamp = GetTimeMilliseconds();

            while (timestamp <= lastTimestamp)
            {
                timestamp = GetTimeMilliseconds();
            }

            return timestamp;
        }

        private long GetTimeMilliseconds()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}

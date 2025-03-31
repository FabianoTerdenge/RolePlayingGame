using RPG.Models;
using System;
using System.Collections.Generic;

namespace RPG.Library.Models
{
    public sealed class FightLog : Subject
    {
        public static List<FightRecord> records = new List<FightRecord>();
        private static readonly Lazy<FightLog> _instance =
       new Lazy<FightLog>(() => new FightLog());

        public static FightLog Instance => _instance.Value;
        private readonly object _lock = new();
        private FightLog() { } // Private constructor
        public void AddFightRecord(FightRecord fightRecords)
        {
            lock (_lock)
            {
                records.Add(fightRecords);
                Notify();
            }

        }
        public IReadOnlyList<FightRecord> GetLogs()
        {
            lock (_lock)
            {
                return records.AsReadOnly();
            }
        }
    }
}

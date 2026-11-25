using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace YMH219_KeyEventLogger
{
    public class KeyLoggerManager
    {
        private readonly List<KeyLogEntry> _entries = new List<KeyLogEntry>();
        public bool IsRecording { get; private set; } = false;

        public int TotalCount => _entries.Count;

        public void Start() => IsRecording = true;
        public void Stop() => IsRecording = false;

        public void AddEntry(KeyLogEntry entry)
        {
            if (!IsRecording) return;
            _entries.Add(entry);
        }

        public void Clear()
        {
            _entries.Clear();
        }

        public List<KeyLogEntry> GetFilteredEntries(DateTime? startTime, DateTime? endTime)
        {
            return _entries
                .Where(e => (!startTime.HasValue || e.Timestamp >= startTime.Value) &&
                            (!endTime.HasValue || e.Timestamp <= endTime.Value))
                .ToList();
        }

        public string GetTopKeysSummary(int topN = 3)
        {
            if (_entries.Count == 0) return "Henüz kayıt yok";

            var topKeys = _entries
                .GroupBy(e => e.KeyDisplay)
                .OrderByDescending(g => g.Count())
                .Take(topN)
                .Select(g => $"{g.Key} ({g.Count()})");

            return string.Join("  |  ", topKeys);
        }

        public void SaveToFile(string filePath, IEnumerable<KeyLogEntry> entriesToSave)
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== YMH219 EĞİTSEL KEY EVENT LOGGER RAPORU ===");
            sb.AppendLine($"Oluşturulma Tarihi : {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            sb.AppendLine($"Toplam Kayıt Sayısı: {entriesToSave.Count()}");
            sb.AppendLine($"En Sık Tuşlar      : {GetTopKeysSummary()}");
            sb.AppendLine(new string('-', 60));

            foreach (var entry in entriesToSave)
            {
                sb.AppendLine(entry.ToString());
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }
    }
}
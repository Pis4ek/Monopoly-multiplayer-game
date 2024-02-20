using Playmode.View;
using System;
using System.Collections.Generic;

namespace Playmode.PlayData.ClientsData
{
    public class ClientsLogData
    {
        public event Action OnNewLogAdded;

        public Log LastLog {  get; private set; }
        public PlayerID LastPlayer => LastLog.Author;
        public int LastIndex => LastLog.Index;

        private Dictionary<int, Log> _logs = new(64);
        private TagConverter _tagConverter = new();

        public void Update(List<Log> logs)
        {
            for (int i = 0; i < logs.Count; i++)
            {
                var newText = _tagConverter.ConvertTagsInString(logs[i].Text);
                var log = new Log(logs[i].Author, newText, logs[i].Index);
                LastLog = log;
                _logs.Add(i, log);
                OnNewLogAdded?.Invoke();
            }
        }
    }
}

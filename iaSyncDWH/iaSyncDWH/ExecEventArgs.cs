using System;

namespace iaSyncDWH
{
    public class ExecEventArgs : EventArgs
    {
        public ExecEventArgs(DBEvents result, string message, Exception exception = null)
        {
            Result = result;
            Message = message;
            Exception = exception;
        }
        public DBEvents Result { get; private set; }
        public string Message { get; private set; }
        public Exception Exception { get; private set; }
    }


    public enum DBEvents
    {
        Done,
        Warning,
        Error,
        Notice
    }

}
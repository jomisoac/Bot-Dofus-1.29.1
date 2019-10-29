using System;
using System.Text;

namespace Bot_Dofus_1._29._1.Utilities.Logs
{
    public class LogMessage
    {
        public string reference { get; }
        public string message { get; }
        public Exception exception { get; }
        public bool es_Exception => exception != null;

        public LogMessage(string _reference, string _message, Exception _exception)
        {
            reference = _reference;
            message = _message;
            exception = _exception;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            string _reference = string.IsNullOrEmpty(reference) ? "" : $"[{reference}]";
            sb.Append($"[{DateTime.Now.ToString("HH:mm:ss")}] {_reference} {message}");

            if (es_Exception)
                sb.Append($"{Environment.NewLine}- An exception has occured : {exception}");

            return sb.ToString();
        }
    }
}

using System;
using System.Text;

namespace Bot_Dofus_1._29._1.Utilidades.Logs
{
    public class LogMensajes
    {
        public string referencia { get; }
        public string mensaje { get; }
        public Exception exception { get; }
        public bool es_Exception => exception != null;

        public LogMensajes(string _referencia, string _mensaje, Exception _exception)
        {
            referencia = _referencia;
            mensaje = _mensaje;
            exception = _exception;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            string _referencia = string.IsNullOrEmpty(referencia) ? "" : $"[{referencia}]";
            sb.Append($"[{DateTime.Now.ToString("HH:mm:ss")}] {_referencia} {mensaje}");

            if (es_Exception)
                sb.Append($"{Environment.NewLine}- Exception ocurrida: {exception}");

            return sb.ToString();
        }
    }
}

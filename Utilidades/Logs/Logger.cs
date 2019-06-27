using System;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;

namespace Bot_Dofus_1._29._1.Utilidades.Logs
{
    public class Logger
    {
        public event Action<LogMensajes, string> log_evento;

        private void log_Final(string referencia, string mensaje, string color, Exception ex = null)
        {
            try
            {
                LogMensajes log_mensaje = new LogMensajes(referencia, mensaje, ex);
                log_evento?.Invoke(log_mensaje, color);
            }
            catch (Exception e)
            {
                log_Final("LOGGER", "Se produjo una excepción al activar el evento registrado.", LogTipos.ERROR, e);
            }
        }

        private void log_Final(string referencia, string mensaje, LogTipos color, Exception ex = null)
        {
            if (color == LogTipos.DEBUG && !GlobalConf.mostrar_mensajes_debug)
                return;
            log_Final(referencia, mensaje, ((int)color).ToString("X"), ex);
        }

        public void log_Error(string referencia, string mensaje) => log_Final(referencia, mensaje, LogTipos.ERROR);
        public void log_Peligro(string referencia, string mensaje) => log_Final(referencia, mensaje, LogTipos.PELIGRO);
        public void log_informacion(string referencia, string mensaje) => log_Final(referencia, mensaje, LogTipos.INFORMACION);
        public void log_normal(string referencia, string mensaje) => log_Final(referencia, mensaje, LogTipos.NORMAL);
        public void log_privado(string referencia, string mensaje) => log_Final(referencia, mensaje, LogTipos.PRIVADO);
    }
}

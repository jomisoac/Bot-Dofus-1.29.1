using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class IMFrame : Frame
    {
        [PaqueteAtributo("Im189")]
        public void get_Mensaje_Bienvenida_Dofus(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.logger.log_Error("DOFUS", "¡Bienvenido(a) a DOFUS, el Mundo de los Doce! Atención: Está prohibido comunicar tu usuario de cuenta y tu contraseña.");
        }

        [PaqueteAtributo("Im039")]
        public void get_Pelea_Espectador_Desactivado(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("COMBATE", "El modo Espectador está desactivado.");
        }

        [PaqueteAtributo("Im040")]
        public void get_Pelea_Espectador_Activado(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("COMBATE", "El modo Espectador está activado.");
        }

        [PaqueteAtributo("Im0152")]
        public void get_Mensaje_Ultima_Conexion_IP(ClienteGame cliente, string paquete)
        {
            string mensaje = paquete.Substring(3).Split(';')[1];
            cliente.cuenta.logger.log_informacion("DOFUS", "Última conexión a tu cuenta realizada el " + mensaje.Split('~')[0] + "/" + mensaje.Split('~')[1] + "/" + mensaje.Split('~')[2] + " a las " + mensaje.Split('~')[3] + ":" + mensaje.Split('~')[4] + " mediante la dirección IP " + mensaje.Split('~')[5]);
        }

        [PaqueteAtributo("Im0153")]
        public void get_Mensaje_Nueva_Conexion_IP(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("DOFUS", "Tu dirección IP actual es " + paquete.Substring(3).Split(';')[1]);
            cliente.cuenta.personaje.evento_Personaje_Seleccionado();
        }
    }
}

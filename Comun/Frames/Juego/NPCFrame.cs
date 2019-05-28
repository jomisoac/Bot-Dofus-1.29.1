using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Protocolo.Enums;

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class NPCFrame : Frame
    {
        [PaqueteAtributo("DCK")]
        public void get_Dialogo_Creado(ClienteTcp cliente, string paquete) => cliente.cuenta.Estado_Cuenta = EstadoCuenta.HABLANDO;

        [PaqueteAtributo("DQ")]
        public void get_Lista_Respuestas(ClienteTcp cliente, string paquete)
        {
            if (cliente.cuenta.Estado_Cuenta != EstadoCuenta.HABLANDO)
                return;

            cliente.cuenta.juego.personaje.evento_Pregunta_Npc(paquete.Substring(2));
        }
    }
}

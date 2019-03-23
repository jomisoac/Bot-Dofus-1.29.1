using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class PeleaFrame : Frame
    {
        [PaqueteAtributo("GP")]
        public void get_Combate_Celdas_Posicion(ClienteGame cliente, string paquete) => new Peleas().onPositionStart(cliente.cuenta, paquete.Substring(2));

        [PaqueteAtributo("GTM")]
        public void get_Combate_Info_Stats(ClienteGame cliente, string paquete) => new Peleas().onTurnMiddle(cliente.cuenta, paquete.Substring(4));

        [PaqueteAtributo("GTR")]
        public void get_Combate_Turno_Listo(ClienteGame cliente, string paquete) => cliente.cuenta.conexion.enviar_Paquete("GT");

        [PaqueteAtributo("GTS")]
        public void get_Combate_Inicio_Turno(ClienteGame cliente, string paquete)
        {
            new Peleas().onTurnStart(cliente.cuenta, paquete.Substring(3));
            cliente.cuenta.conexion.enviar_Paquete("Gt");
        }
    }
}

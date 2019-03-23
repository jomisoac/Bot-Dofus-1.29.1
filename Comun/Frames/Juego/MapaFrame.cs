using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class MapaFrame : Frame
    {
        [PaqueteAtributo("al")]
        public void get_Lista_SubAreas_Alineamiento(ClienteGame cliente, string paquete) => cliente.enviar_Paquete("GC1");

        [PaqueteAtributo("fC")]
        public void get_Combates_Mapa(ClienteGame cliente, string paquete) => cliente.enviar_Paquete("BD");

        [PaqueteAtributo("GM")]
        public void get_Movimientos_Personajes(ClienteGame cliente, string paquete) => new GameActions(cliente.cuenta).get_En_Movimiento(paquete.Substring(3));
        
        [PaqueteAtributo("GA")]
        public void get_Iniciar_Accion(ClienteGame cliente, string paquete) => new GameActions(cliente.cuenta).get_On_GameAction(paquete.Substring(2));

        [PaqueteAtributo("GAF")]
        public void get_Finalizar_Accion(ClienteGame cliente, string paquete) => cliente.cuenta.conexion.enviar_Paquete("GKK" + paquete[3]);

        [PaqueteAtributo("GDM")]
        public void get_Nuevo_Mapa(ClienteGame cliente, string paquete)
        {
            cliente.cuenta.personaje.mapa = new Mapa(cliente.cuenta, paquete.Substring(4));
            cliente.cuenta.personaje.evento_Mapa_Actualizado();
        }
    }
}

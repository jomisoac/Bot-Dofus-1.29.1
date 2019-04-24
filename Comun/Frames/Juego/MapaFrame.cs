using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;
using System;
using System.Threading.Tasks;

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
        public async Task get_Lista_SubAreas_Alineamiento(ClienteAbstracto cliente, string paquete) => await cliente.enviar_Paquete("GC1");
        
        [PaqueteAtributo("GM")]
        public void get_Movimientos_Personajes(ClienteAbstracto cliente, string paquete) => new GameActions(cliente.cuenta).get_En_Movimiento(paquete.Substring(3));

        [PaqueteAtributo("GAF")]
        public async Task get_Finalizar_Accion(ClienteAbstracto cliente, string paquete)
        {
            string[] id_fin_accion = paquete.Substring(3).Split('|');

            await cliente.cuenta.conexion.enviar_Paquete("GKK" + id_fin_accion[0]);
        }

        [PaqueteAtributo("GA")]
        public async Task get_Iniciar_Accion(ClienteGame cliente, string paquete) => await new GameActions(cliente.cuenta).get_On_GameAction(paquete.Substring(2));

        [PaqueteAtributo("GDF")]
        public void get_Estado_Interactivo(ClienteAbstracto cliente, string paquete)
        {
            foreach(string interactivo in paquete.Substring(4).Split('|'))
            {
                string[] separador = interactivo.Split(';');
                Personaje personaje = cliente.cuenta.personaje;
                short celda_id = short.Parse(separador[0]);
                byte estado = byte.Parse(separador[1]);

                if (estado >= 2 && 4 >= estado)
                {
                    personaje.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;

                    if (personaje.celda_objetivo_recoleccion == celda_id && !cliente.cuenta.esta_recolectando())
                    {
                        cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                        personaje.evento_Recoleccion_Acabada();
                    }
                }
            }
        }

        [PaqueteAtributo("IQ")]
        public void get_Numero_Arriba_Pj(ClienteAbstracto cliente, string paquete)
        {
            int id = int.Parse(paquete.Substring(2).Split('|')[0]);
            Cuenta cuenta = cliente.cuenta;

            if (cuenta.esta_recolectando() && cuenta.personaje.id == id)
            {
                cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                cuenta.personaje.evento_Recoleccion_Acabada();
            }
        }

        [PaqueteAtributo("PBF")]
        public async Task get_Antibot_1(ClienteAbstracto cliente, string paquete) => await cliente.cuenta.conexion.enviar_Paquete(paquete.Substring(0, 2) + new Random().Next(120000, 140000));

        [PaqueteAtributo("GDM")]
        public void get_Nuevo_Mapa(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.personaje.mapa = new Mapa(cliente.cuenta, paquete.Substring(4));
            cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
            cliente.cuenta.personaje.evento_Mapa_Actualizado();
        }
    }
}

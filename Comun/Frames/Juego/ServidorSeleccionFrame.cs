using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
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
    class ServidorSeleccionFrame : Frame
    {
        [PaqueteAtributo("HG")]
        public async Task bienvenida_Juego(ClienteAbstracto cliente, string paquete) => await cliente.enviar_Paquete("AT" + cliente.cuenta.tiquet_game);

        [PaqueteAtributo("ATK0")]
        public async Task resultado_Servidor_Seleccion(ClienteAbstracto cliente, string paquete)
        {
            await cliente.enviar_Paquete("Ak" + Hash.HEX_CHARS[Convert.ToInt16(paquete.Substring(3, 1))]);
            await cliente.enviar_Paquete("AV");
        }

        [PaqueteAtributo("AV0")]
        public async Task lista_Personajes(ClienteAbstracto cliente, string paquete)
        {
            await cliente.enviar_Paquete("Ages");
            await cliente.enviar_Paquete("AL");
            await cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("ALK")]
        public async Task seleccionar_Personaje(ClienteAbstracto cliente, string paquete) => await cliente.enviar_Paquete(new PersonajeSeleccion(cliente.cuenta.cuenta_configuracion.id_personaje, paquete.Substring(3)).get_Mensaje());

        [PaqueteAtributo("BT")]
        public async Task get_Tiempo_Servidor(ClienteAbstracto cliente, string paquete)
        {
            await cliente.enviar_Paquete("GI");
            cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
        }

        [PaqueteAtributo("BD")]
        public async Task get_Fecha_Servidor(ClienteAbstracto cliente, string paquete)
        {
            if(cliente.cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO)
                await cliente.enviar_Paquete("GC1");
        }

        [PaqueteAtributo("ASK")]
        public async Task personaje_Seleccionado(ClienteAbstracto cliente, string paquete)
        {
            string[] _loc4 = paquete.Substring(4).Split('|');
            if (cliente.cuenta.personaje == null)
                cliente.cuenta.personaje = new Personaje(int.Parse(_loc4[0]), _loc4[1], byte.Parse(_loc4[2]), byte.Parse(_loc4[4]), int.Parse(_loc4[5]), cliente.cuenta);

            cliente.cuenta.Estado_Socket = EstadoSocket.PERSONAJE_SELECCIONADO;
            cliente.cuenta.personaje.inventario.agregar_Objetos(_loc4[9]);

            if (cliente.cuenta.personaje.nombre_personaje.Equals("Campesino-aidemu"))
                await cliente.enviar_Paquete("BYA");
        }
    }
}

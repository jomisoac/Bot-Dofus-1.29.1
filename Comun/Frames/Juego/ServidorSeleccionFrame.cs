using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Game.Paquetes;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;

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
        public void bienvenida_Juego(ClienteGame cliente, string paquete) => cliente.enviar_Paquete("AT" + cliente.cuenta.tiquet_game);

        [PaqueteAtributo("ATK0")]
        public void resultado_Servidor_Seleccion(ClienteGame cliente, string paquete)
        {
            cliente.enviar_Paquete("Ak" + Hash.HEX_CHARS[Convert.ToInt16(paquete.Substring(3, 1))]);
            cliente.enviar_Paquete("AV");
        }

        [PaqueteAtributo("AV0")]
        public void lista_Personajes(ClienteGame cliente, string paquete)
        {
            string[] idiomas = { "es", "fr", "en", "pt" };
            cliente.enviar_Paquete("Ag" + idiomas[new Random().Next(0, (idiomas.Length - 1))]);
            cliente.enviar_Paquete("AL");
            cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("ALK")]
        public void seleccionar_Personaje(ClienteGame cliente, string paquete) => cliente.enviar_Paquete(new PersonajeSeleccion(cliente.cuenta.cuenta_configuracion.id_personaje, paquete.Substring(3)).get_Mensaje());

        [PaqueteAtributo("BD")]
        public void get_Fecha_Servidor(ClienteGame cliente, string paquete) => cliente.enviar_Paquete("GI");

        [PaqueteAtributo("ASK")]
        public void personaje_Seleccionado(ClienteGame cliente, string paquete)
        {
            string[] _loc4 = paquete.Substring(4).Split('|');
            if (cliente.cuenta.personaje == null)
                cliente.cuenta.personaje = new Personaje(int.Parse(_loc4[0]), _loc4[1], byte.Parse(_loc4[2]), int.Parse(_loc4[3]), byte.Parse(_loc4[4]), int.Parse(_loc4[5]), int.Parse(_loc4[6]), int.Parse(_loc4[7]), int.Parse(_loc4[8]), _loc4[9], cliente.cuenta);

            cliente.cuenta.Estado_Socket = EstadoSocket.PERSONAJE_SELECCIONADO;
            cliente.cuenta.personaje.inventario.agregar_Objetos(_loc4[9]);
        }
    }
}

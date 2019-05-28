using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
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
    internal class ServidorSeleccionFrame : Frame
    {
        [PaqueteAtributo("HG")]
        public void bienvenida_Juego(ClienteTcp cliente, string paquete) => cliente.enviar_Paquete("AT" + cliente.cuenta.tiquet_game);

        [PaqueteAtributo("ATK0")]
        public void resultado_Servidor_Seleccion(ClienteTcp cliente, string paquete)
        {
            cliente.enviar_Paquete("Ak" + Hash.HEX_CHARS[Convert.ToInt16(paquete.Substring(3, 1))]);
            cliente.enviar_Paquete("AV");
        }

        [PaqueteAtributo("AV0")]
        public void lista_Personajes(ClienteTcp cliente, string paquete)
        {
            cliente.enviar_Paquete("Ages");
            cliente.enviar_Paquete("AL");
            cliente.enviar_Paquete("Af");
        }

        [PaqueteAtributo("ALK")]
        public void seleccionar_Personaje(ClienteTcp cliente, string paquete)
        {
            cliente.enviar_Paquete(new PersonajeSeleccion(cliente.cuenta.cuenta_configuracion.id_personaje, paquete.Substring(3)).get_Mensaje());
        }

        [PaqueteAtributo("BT")]
        public void get_Tiempo_Servidor(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;

            cliente.enviar_Paquete("GI");

            if(!cuenta.esta_luchando())
                cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
        }

        [PaqueteAtributo("ASK")]
        public void personaje_Seleccionado(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] _loc4 = paquete.Substring(4).Split('|');

            int id = int.Parse(_loc4[0]);
            string nombre = _loc4[1];
            byte nivel = byte.Parse(_loc4[2]); //maximo nivel en dofus 200, maximo valor de byte 255 2^8-1
            byte raza_id = byte.Parse(_loc4[3]);
            byte sexo = byte.Parse(_loc4[4]);

            cuenta.juego.personaje.set_Datos_Personaje(id, nombre, nivel, sexo, raza_id);
            cuenta.juego.personaje.inventario.agregar_Objetos(_loc4[9]);

            cliente.enviar_Paquete("GC1");
            cliente.enviar_Paquete("BYA");
            cuenta.juego.personaje.evento_Personaje_Seleccionado();
            cuenta.conexion.Estado_Socket = EstadoSocket.CONECTADO;
        }
    }
}

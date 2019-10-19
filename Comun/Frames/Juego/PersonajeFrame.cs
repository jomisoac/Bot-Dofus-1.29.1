using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Personaje;
using Bot_Dofus_1._29._1.Otros.Game.Personaje.Oficios;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class PersonajeFrame : Frame
    {
        [PaqueteAtributo("As")]
        public void get_Stats_Actualizados(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.actualizar_Caracteristicas(paquete);

        [PaqueteAtributo("PIK")]
        public void get_Peticion_Grupo(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("Groupe", $"Nouvelle invitation de groupe du personnage: {paquete.Substring(3).Split('|')[0]}");
            cliente.enviar_Paquete("PR");
            cliente.cuenta.logger.log_informacion("Groupe", "Rejêt de l'invitation");
        }

        [PaqueteAtributo("SL")]
        public void get_Lista_Hechizos(ClienteTcp cliente, string paquete)
        {
            if (!paquete[2].Equals('o'))
                cliente.cuenta.game.personaje.actualizar_Hechizos(paquete.Substring(2));
        }

        [PaqueteAtributo("Ow")]
        public void get_Actualizacion_Pods(ClienteTcp cliente, string paquete)
        {
            string[] pods = paquete.Substring(2).Split('|');
            short pods_actuales = short.Parse(pods[0]);
            short pods_maximos = short.Parse(pods[1]);
            PersonajeJuego personaje = cliente.cuenta.game.personaje;

            personaje.inventario.pods_actuales = pods_actuales;
            personaje.inventario.pods_maximos = pods_maximos;
            cliente.cuenta.game.personaje.evento_Pods_Actualizados();
        }

        [PaqueteAtributo("DV")]
        public void get_Cerrar_Dialogo(ClienteTcp cliente, string paquete)
        {
            Account cuenta = cliente.cuenta;

            switch (cuenta.Estado_Cuenta)
            {
                case AccountStates.STORAGE:
                    cuenta.game.personaje.inventario.evento_Almacenamiento_Abierto();
                    break;

                case AccountStates.DIALOG:
                    IEnumerable<Npcs> npcs = cuenta.game.mapa.lista_npcs();
                    Npcs npc = npcs.ElementAt((cuenta.game.personaje.hablando_npc_id * -1) - 1);
                    npc.respuestas.Clear();
                    npc.respuestas = null;

                    cuenta.Estado_Cuenta = AccountStates.CONNECTED_INACTIVE;
                    cuenta.game.personaje.evento_Dialogo_Acabado();
                break;
            }
        }

        [PaqueteAtributo("EV")]
        public void get_Ventana_Cerrada(ClienteTcp cliente, string paquete)
        {
            Account cuenta = cliente.cuenta;

            if (cuenta.Estado_Cuenta == AccountStates.STORAGE)
            {
                cuenta.Estado_Cuenta = AccountStates.CONNECTED_INACTIVE;
                cuenta.game.personaje.inventario.evento_Almacenamiento_Cerrado();
            }
        }

        [PaqueteAtributo("JS")]
        public void get_Skills_Oficio(ClienteTcp cliente, string paquete)
        {
            string[] separador_skill;
            PersonajeJuego personaje = cliente.cuenta.game.personaje;
            Oficio oficio;
            SkillsOficio skill = null;
            short id_oficio, id_skill;
            byte cantidad_minima, cantidad_maxima;
            float tiempo;

            foreach (string datos_oficio in paquete.Substring(3).Split('|'))
            {
                id_oficio = short.Parse(datos_oficio.Split(';')[0]);
                oficio = personaje.oficios.Find(x => x.id == id_oficio);

                if (oficio == null)
                {
                    oficio = new Oficio(id_oficio);
                    personaje.oficios.Add(oficio);
                }

                foreach (string datos_skill in datos_oficio.Split(';')[1].Split(','))
                {
                    separador_skill = datos_skill.Split('~');
                    id_skill = short.Parse(separador_skill[0]);
                    cantidad_minima = byte.Parse(separador_skill[1]);
                    cantidad_maxima = byte.Parse(separador_skill[2]);
                    tiempo = float.Parse(separador_skill[4]);
                    skill = oficio.skills.Find(actividad => actividad.id == id_skill);

                    if (skill != null)
                        skill.set_Actualizar(id_skill, cantidad_minima, cantidad_maxima, tiempo);
                    else
                        oficio.skills.Add(new SkillsOficio(id_skill, cantidad_minima, cantidad_maxima, tiempo));
                }
            }

            personaje.evento_Oficios_Actualizados();
        }

        [PaqueteAtributo("JX")]
        public void get_Experiencia_Oficio(ClienteTcp cliente, string paquete)
        {
            string[] separador_oficio_experiencia = paquete.Substring(3).Split('|');
            PersonajeJuego personaje = cliente.cuenta.game.personaje;
            uint experiencia_actual, experiencia_base, experiencia_siguiente_nivel;
            short id;
            byte nivel;

            foreach (string oficio in separador_oficio_experiencia)
            {
                id = short.Parse(oficio.Split(';')[0]);
                nivel = byte.Parse(oficio.Split(';')[1]);
                experiencia_base = uint.Parse(oficio.Split(';')[2]);
                experiencia_actual = uint.Parse(oficio.Split(';')[3]);

                if (nivel < 100)
                    experiencia_siguiente_nivel = uint.Parse(oficio.Split(';')[4]);
                else
                    experiencia_siguiente_nivel = 0;

                personaje.oficios.Find(x => x.id == id).set_Actualizar_Oficio(nivel, experiencia_base, experiencia_actual, experiencia_siguiente_nivel);
            }
            personaje.evento_Oficios_Actualizados();
        }

        [PaqueteAtributo("Re")]
        public void get_Datos_Montura(ClienteTcp cliente, string paquete) => cliente.cuenta.canUseMount = true;

        [PaqueteAtributo("OAKO")]
        public void get_Aparecer_Objeto(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.inventario.agregar_Objetos(paquete.Substring(4));

        [PaqueteAtributo("OR")]
        public void get_Eliminar_Objeto(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.inventario.eliminar_Objeto(uint.Parse(paquete.Substring(2)), 1, false);

        [PaqueteAtributo("OQ")]
        public void get_Modificar_Cantidad_Objeto(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.inventario.modificar_Objetos(paquete.Substring(2));

        [PaqueteAtributo("ECK")]
        public void get_Intercambio_Ventana_Abierta(ClienteTcp cliente, string paquete) => cliente.cuenta.Estado_Cuenta = AccountStates.STORAGE;

        [PaqueteAtributo("PCK")]
        public void get_Grupo_Aceptado(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.en_grupo = true;

        [PaqueteAtributo("PV")]
        public void get_Grupo_Abandonado(ClienteTcp cliente, string paquete) => cliente.cuenta.game.personaje.en_grupo = true;

        [PaqueteAtributo("ERK")]
        public void get_Peticion_Intercambio(ClienteTcp cliente, string paquete)
        {
            cliente.cuenta.logger.log_informacion("INFORMATION", "L'invitation à l'échange est rejetée");
            cliente.enviar_Paquete("EV", true);
        }

        [PaqueteAtributo("ILS")]
        public void get_Tiempo_Regenerado(ClienteTcp cliente, string paquete)
        {
            paquete = paquete.Substring(3);
            int tiempo = int.Parse(paquete);
            Account cuenta = cliente.cuenta;
            PersonajeJuego personaje = cuenta.game.personaje;

            personaje.timer_regeneracion.Change(Timeout.Infinite, Timeout.Infinite);
            personaje.timer_regeneracion.Change(tiempo, tiempo);

            cuenta.logger.log_informacion("DOFUS", $"Votre personnage récupère 1 pdv chaque {tiempo / 1000} secondes");
        }

        [PaqueteAtributo("ILF")]
        public void get_Cantidad_Vida_Regenerada(ClienteTcp cliente, string paquete)
        {
            paquete = paquete.Substring(3);
            int vida = int.Parse(paquete);
            Account cuenta = cliente.cuenta;
            PersonajeJuego personaje = cuenta.game.personaje;

            personaje.caracteristicas.vitalidad_actual += vida;
            cuenta.logger.log_informacion("DOFUS", $"Vous avez récupéré {vida} points de vie");
        }

        [PaqueteAtributo("eUK")]
        public void get_Emote_Recibido(ClienteTcp cliente, string paquete)
        {
            string[] separador = paquete.Substring(3).Split('|');
            int id = int.Parse(separador[0]), emote_id = int.Parse(separador[1]);
            Account cuenta = cliente.cuenta;

            if (cuenta.game.personaje.id != id)
                return;

            if (emote_id == 1 && cuenta.Estado_Cuenta != AccountStates.REGENERATION)
                cuenta.Estado_Cuenta = AccountStates.REGENERATION;
            else if (emote_id == 0 && cuenta.Estado_Cuenta == AccountStates.REGENERATION)
                cuenta.Estado_Cuenta = AccountStates.CONNECTED_INACTIVE;
        }

        [PaqueteAtributo("Bp")]
        public void get_Ping_Promedio(ClienteTcp cliente, string paquete) => cliente.enviar_Paquete($"Bp{cliente.get_Promedio_Pings()}|{cliente.get_Total_Pings()}|50");

        [PaqueteAtributo("pong")]
        public void get_Ping_Pong(ClienteTcp cliente, string paquete) => cliente.cuenta.logger.log_informacion("DOFUS", $"Ping: {cliente.get_Actual_Ping()} ms");
    }
}

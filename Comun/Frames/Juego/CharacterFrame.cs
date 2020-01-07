using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Character;
using Bot_Dofus_1._29._1.Otros.Game.Character.Jobs;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    class CharacterFrame : Frame
    {
        [PaqueteAtributo("As")]
        public void get_Stats_Actualizados(TcpClient cliente, string paquete) => cliente.account.game.character.actualizar_Caracteristicas(paquete);

        [PaqueteAtributo("PIK")]
        public void get_Peticion_Grupo(TcpClient cliente, string paquete)
        {

            if (cliente.account.hasGroup == false)
            {
                Task.Delay(1250);
                cliente.SendPacket("PR");
                cliente.account.Logger.LogInfo("Groupe", "Rejêt de l'invitation");
            }
            else if ( cliente.account.isGroupLeader == false)
            {
                string PseudoInviter = paquete.Substring(3).Split('|')[0];
                Account leaderAccount = cliente.account.group.lider;
                string pseudoLeader = leaderAccount.game.character.nombre;
                if (PseudoInviter.ToLower() == pseudoLeader.ToLower())
                {
                    Task.Delay(550);
                    cliente.account.connexion.SendPacket("PA");
                    cliente.account.Logger.LogInfo("Groupe", "Je suis maintenant dans le groupe de : " + pseudoLeader);
                }            
                else
                {
                    cliente.SendPacket("PR");
                    cliente.account.Logger.LogInfo("Groupe", "Rejêt de l'invitation");
                }
            }
            else if(paquete.Substring(3).Split('|').Length ==1)
            {
                Task.Delay(1250);
                cliente.SendPacket("PR");
                cliente.account.Logger.LogInfo("Groupe", "Rejêt de l'invitation");
            }

        }

        [PaqueteAtributo("SL")]
        public void get_Lista_Hechizos(TcpClient cliente, string paquete)
        {
            if (!paquete[2].Equals('o'))
                cliente.account.game.character.actualizar_Hechizos(paquete.Substring(2));
        }

        [PaqueteAtributo("Ow")]
        public void get_Actualizacion_Pods(TcpClient cliente, string paquete)
        {
            string[] pods = paquete.Substring(2).Split('|');
            short pods_actuales = short.Parse(pods[0]);
            short pods_maximos = short.Parse(pods[1]);
            CharacterClass personaje = cliente.account.game.character;

            personaje.inventario.pods_actuales = pods_actuales;
            personaje.inventario.pods_maximos = pods_maximos;
            cliente.account.game.character.evento_Pods_Actualizados();
        }

        [PaqueteAtributo("DV")]
        public void get_Cerrar_Dialogo(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            switch (cuenta.AccountState)
            {
                case AccountStates.STORAGE:
                    cuenta.game.character.inventario.evento_Almacenamiento_Abierto();
                    break;

                case AccountStates.DIALOG:
                    IEnumerable<Npcs> npcs = cuenta.game.map.lista_npcs();
                    Npcs npc = npcs.ElementAt((cuenta.game.character.hablando_npc_id * -1) - 1);
                    npc.respuestas.Clear();
                    npc.respuestas = null;

                    cuenta.AccountState = AccountStates.CONNECTED_INACTIVE;
                    cuenta.game.character.evento_Dialogo_Acabado();
                    break;
            }
        }

        [PaqueteAtributo("EV")]
        public void get_Ventana_Cerrada(TcpClient cliente, string paquete)
        {
            Account cuenta = cliente.account;

            if (cuenta.AccountState == AccountStates.STORAGE)
            {
                cuenta.AccountState = AccountStates.CONNECTED_INACTIVE;
                cuenta.game.character.inventario.evento_Almacenamiento_Cerrado();
            }
        }

        [PaqueteAtributo("JS")]
        public void get_Skills_Oficio(TcpClient cliente, string paquete)
        {
            string[] separador_skill;
            CharacterClass personaje = cliente.account.game.character;
            Job oficio;
            JobSkills skill = null;
            short id_oficio, id_skill;
            byte cantidad_minima, cantidad_maxima;
            float tiempo;

            foreach (string datos_oficio in paquete.Substring(3).Split('|'))
            {
                id_oficio = short.Parse(datos_oficio.Split(';')[0]);
                oficio = personaje.oficios.Find(x => x.id == id_oficio);

                if (oficio == null)
                {
                    oficio = new Job(id_oficio);
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
                        oficio.skills.Add(new JobSkills(id_skill, cantidad_minima, cantidad_maxima, tiempo));
                }
            }

            personaje.evento_Oficios_Actualizados();
        }

        [PaqueteAtributo("JX")]
        public void get_Experiencia_Oficio(TcpClient cliente, string paquete)
        {
            string[] separador_oficio_experiencia = paquete.Substring(3).Split('|');
            CharacterClass personaje = cliente.account.game.character;
            uint experiencia_actual, experiencia_base, experiencia_siguiente_nivel;
            short id;
            byte nivel;

            foreach (string oficio in separador_oficio_experiencia)
            {
                var payload = oficio.Split(';');
                if (payload.Length < 4)
                    continue;
                id = short.Parse(payload[0]);
                nivel = byte.Parse(payload[1]);
                experiencia_base = uint.Parse(payload[2]);
                experiencia_actual = uint.Parse(payload[3]);

                if (nivel < 100 && payload.Length >= 4)
                    experiencia_siguiente_nivel = uint.Parse(oficio.Split(';')[4]);
                else
                    experiencia_siguiente_nivel = 0;

                personaje.oficios.Find(x => x.id == id).set_Actualizar_Oficio(nivel, experiencia_base, experiencia_actual, experiencia_siguiente_nivel);
            }
            personaje.evento_Oficios_Actualizados();
        }

        [PaqueteAtributo("Re")]
        public void get_Datos_Montura(TcpClient cliente, string paquete) => cliente.account.canUseMount = true;

        [PaqueteAtributo("OAKO")]
        public void get_Aparecer_Objeto(TcpClient cliente, string paquete) => cliente.account.game.character.inventario.agregar_Objetos(paquete.Substring(4));

        [PaqueteAtributo("OR")]
        public void get_Eliminar_Objeto(TcpClient cliente, string paquete) => cliente.account.game.character.inventario.eliminar_Objeto(uint.Parse(paquete.Substring(2)), 1, false);

        [PaqueteAtributo("OQ")]
        public void get_Modificar_Cantidad_Objeto(TcpClient cliente, string paquete) => cliente.account.game.character.inventario.modificar_Objetos(paquete.Substring(2));

        [PaqueteAtributo("ECK")]
        public void get_Intercambio_Ventana_Abierta(TcpClient cliente, string paquete) => cliente.account.AccountState = AccountStates.STORAGE;

        [PaqueteAtributo("PCK")]
        public void get_Grupo_Aceptado(TcpClient cliente, string paquete) => cliente.account.game.character.en_grupo = true;

        [PaqueteAtributo("PV")]
        public void get_Grupo_Abandonado(TcpClient cliente, string paquete) => cliente.account.game.character.en_grupo = true;

        [PaqueteAtributo("ERK")]
        public async void get_Peticion_Intercambio(TcpClient cliente, string paquete)
        {
            var t = new Random().Next(600, 1200);

            await Task.Delay(t);

            cliente.account.Logger.LogInfo("INFORMATION", "L'invitation à l'échange est rejetée");
            cliente.SendPacket("EV", true);
        }

        [PaqueteAtributo("ILS")]
        public void get_Tiempo_Regenerado(TcpClient cliente, string paquete)
        {
            paquete = paquete.Substring(3);
            int tiempo = int.Parse(paquete);
            Account cuenta = cliente.account;
            CharacterClass personaje = cuenta.game.character;

            personaje.timer_regeneracion.Change(Timeout.Infinite, Timeout.Infinite);
            personaje.timer_regeneracion.Change(tiempo, tiempo);

            cuenta.Logger.LogInfo("DOFUS", $"Votre personnage récupère 1 pdv chaque {tiempo / 1000} secondes");
        }

        [PaqueteAtributo("ILF")]
        public void get_Cantidad_Vida_Regenerada(TcpClient cliente, string paquete)
        {
            paquete = paquete.Substring(3);
            int vida = int.Parse(paquete);
            Account cuenta = cliente.account;
            CharacterClass personaje = cuenta.game.character;

            personaje.caracteristicas.vitalidad_actual += vida;
            cuenta.Logger.LogInfo("DOFUS", $"Vous avez récupéré {vida} points de vie");
        }

        [PaqueteAtributo("eUK")]
        public void get_Emote_Recibido(TcpClient cliente, string paquete)
        {
            string[] separador = paquete.Substring(3).Split('|');
            int id = int.Parse(separador[0]), emote_id = int.Parse(separador[1]);
            Account cuenta = cliente.account;

            if (cuenta.game.character.id != id)
                return;

            if (emote_id == 1 && cuenta.AccountState != AccountStates.REGENERATION)
                cuenta.AccountState = AccountStates.REGENERATION;
            else if (emote_id == 0 && cuenta.AccountState == AccountStates.REGENERATION)
                cuenta.AccountState = AccountStates.CONNECTED_INACTIVE;
        }

        [PaqueteAtributo("Bp")]
        public void get_Ping_Promedio(TcpClient cliente, string paquete) => cliente.SendPacket($"Bp{cliente.GetPingAverage()}|{cliente.GetTotalPings()}|50");

        [PaqueteAtributo("pong")]
        public void get_Ping_Pong(TcpClient cliente, string paquete) => cliente.account.Logger.LogInfo("DOFUS", $"Ping: {cliente.GetPing()} ms");
    }
}

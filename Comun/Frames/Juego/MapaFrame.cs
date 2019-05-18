using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Npcs;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Juego
{
    internal class MapaFrame : Frame
    {
        [PaqueteAtributo("al")]
        public void get_Lista_SubAreas_Alineamiento(ClienteAbstracto cliente, string paquete) => cliente.enviar_Paquete("GC1");

        [PaqueteAtributo("GM")]
        public async Task get_Movimientos_Personajes(ClienteAbstracto cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            string[] separador_jugadores = paquete.Substring(3).Split('|'), informaciones;
            string _loc6, nombre_template, tipo;

            for (int i = 0; i < separador_jugadores.Length; ++i)
            {
                _loc6 = separador_jugadores[i];
                if (_loc6.Length != 0)
                {
                    informaciones = _loc6.Substring(1).Split(';');
                    if (_loc6[0].Equals('+'))
                    {
                        Celda celda = cuenta.personaje.mapa.celdas[short.Parse(informaciones[0])];
                        int id = int.Parse(informaciones[3]);
                        nombre_template = informaciones[4];
                        tipo = informaciones[5];
                        if (tipo.Contains(","))
                            tipo = tipo.Split(',')[0];

                        switch (int.Parse(tipo))
                        {
                            case -1:
                            case -2:
                                if (cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO)
                                {
                                    int vida = int.Parse(informaciones[12]);
                                    byte pa = byte.Parse(informaciones[13]);
                                    byte pm = byte.Parse(informaciones[14]);
                                    byte equipo = byte.Parse(informaciones[15]);

                                    cuenta.pelea.get_Agregar_Luchador(new Luchadores(id, true, vida, pa, pm, celda.id, vida, equipo));
                                }
                                break;

                            case -3://monstruos
                                string[] templates = nombre_template.Split(',');
                                string[] niveles = informaciones[7].Split(',');


                                Monstruo monstruo = new Monstruo(id, int.Parse(templates[0]), celda, int.Parse(niveles[0]));
                                monstruo.lider_grupo = monstruo;

                                for (int m = 1; m < templates.Length; ++m)
                                    monstruo.moobs_dentro_grupo.Add(new Monstruo(id, int.Parse(templates[m]), celda, int.Parse(niveles[m])));

                                cuenta.personaje.mapa.agregar_Monstruo(monstruo);
                                break;

                            case -4://NPC
                                cuenta.personaje.mapa.agregar_Npc(new Npcs(id, int.Parse(nombre_template), celda));
                                break;

                            case -5:
                                break;

                            case -6:
                                break;

                            case -7:
                            case -8:
                                break;

                            case -9:
                                break;

                            case -10:
                                break;

                            default:
                                if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                                {
                                    if (cuenta.personaje.id != id)
                                    {
                                        if (nombre_template.StartsWith("[") || Extensiones.lista_mods.Contains(nombre_template))
                                            cuenta.conexion.get_Desconectar_Socket();

                                        cuenta.personaje.mapa.agregar_Personaje(new Personaje(id, nombre_template, byte.Parse(informaciones[7].ToString()), celda));
                                    }
                                    else
                                        cuenta.personaje.celda = celda;
                                }
                                else
                                {
                                    int vida = int.Parse(informaciones[14]);
                                    byte pa = byte.Parse(informaciones[15]);
                                    byte pm = byte.Parse(informaciones[16]);
                                    byte equipo = byte.Parse(informaciones[24]);

                                    cuenta.pelea.get_Agregar_Luchador(new Luchadores(id, true, vida, pa, pm, celda.id, vida, equipo));

                                    if (cuenta.personaje.id == id && cuenta.pelea_extension.configuracion.posicionamiento != PosicionamientoInicioPelea.INMOVIL)
                                    {
                                        await Task.Delay(300);

                                        /** la posicion es aleatoria pero el paquete GP siempre aparecera primero el team donde esta el pj **/
                                        short celda_posicion = cuenta.pelea.get_Celda_Mas_Cercana_O_Lejana(cuenta.pelea_extension.configuracion.posicionamiento == PosicionamientoInicioPelea.CERCA_DE_ENEMIGOS, cuenta.pelea.celdas_preparacion, cuenta.personaje.mapa);

                                        if (celda_posicion != celda.id)
                                            cuenta.conexion.enviar_Paquete("Gp" + celda_posicion);
                                        else
                                            cuenta.conexion.enviar_Paquete("GR1");
                                    }
                                    else if (cuenta.personaje.id == id)
                                    {
                                        await Task.Delay(300);
                                        cuenta.conexion.enviar_Paquete("GR1");//boton listo
                                    }
                                }
                                break;
                        }
                    }
                    else if (_loc6[0].Equals('-'))
                    {
                        if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                        {
                            int id = int.Parse(_loc6.Substring(1));
                            cuenta.personaje.mapa.eliminar_Personaje(id);
                            cuenta.personaje.mapa.eliminar_Monstruo(id);
                        }
                    }
                }
            }
        }

        [PaqueteAtributo("GAF")]
        public void get_Finalizar_Accion(ClienteAbstracto cliente, string paquete)
        {
            string[] id_fin_accion = paquete.Substring(3).Split('|');

            cliente.cuenta.conexion.enviar_Paquete("GKK" + id_fin_accion[0]);
        }

        [PaqueteAtributo("GAS")]
        public void get_Inicio_Accion(ClienteGame cliente, string paquete) { }

        [PaqueteAtributo("GA")]
        public async Task get_Iniciar_Accion(ClienteGame cliente, string paquete)
        {
            string[] separador = paquete.Substring(2).Split(';');
            int id_accion = int.Parse(separador[1]);

            if (id_accion > 0)//Error: GA;0
            {
                int id_jugador = int.Parse(separador[2]);
                Cuenta cuenta = cliente.cuenta;
                Personaje personaje = cuenta.personaje;
                Luchadores luchador = null;

                switch (id_accion)
                {
                    case 1:
                        Mapa mapa = personaje.mapa;
                        short celda_destino = Hash.get_Celda_Id_Desde_hash(separador[3].Substring(separador[3].Length - 2));

                        if (id_jugador == personaje.id && celda_destino > 0 && personaje.celda.id != celda_destino)
                        {
                            if (!cuenta.esta_luchando())
                                await Task.Delay(Pathfinding.tiempo_desplazamiento);
                            else
                                await Task.Delay(300 * personaje.celda.get_Distancia_Entre_Dos_Casillas(celda_destino));

                            cuenta.conexion.enviar_Paquete("GKK" + byte.Parse(separador[0]));
                            personaje.celda = mapa.celdas[celda_destino];


                            if (!cuenta.esta_luchando())
                            {
                                personaje.evento_Movimiento_Celda(true);
                                cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                            }
                        }
                        else if (mapa.get_Personajes().ContainsKey(id_jugador) && !cuenta.esta_luchando())
                        {
                            mapa.get_Personajes()[id_jugador].celda = mapa.celdas[celda_destino];

                            if (GlobalConf.mostrar_mensajes_debug)
                                cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un personaje a la casilla: " + celda_destino);
                        }
                        else if (mapa.get_Monstruos().ContainsKey(id_jugador) && !cuenta.esta_luchando())
                        {
                            mapa.get_Monstruos()[id_jugador].celda = mapa.celdas[celda_destino];

                            if (GlobalConf.mostrar_mensajes_debug)
                                cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un grupo de monstruo a la casilla: " + celda_destino);
                        }

                        if (cuenta.Estado_Cuenta == EstadoCuenta.LUCHANDO)
                        {
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_jugador);
                            if (luchador != null)
                                luchador.celda_id = celda_destino;
                        }
                        break;

                    case 2: //Cargando el mapa
                        await Task.Delay(200);
                        break;

                    case 102:
                        if (cuenta.esta_luchando())
                        {
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_jugador);
                            byte pa_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                            if (luchador != null)
                                luchador.pa -= pa_utilizados;
                        }
                        break;

                    case 103:
                        if (cuenta.esta_luchando())
                        {
                            int id_muerto = int.Parse(separador[3]);

                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_muerto);
                            if (luchador != null)
                                luchador.esta_vivo = false;
                        }
                        break;

                    case 129: //movimiento en pelea con exito
                        if (cuenta.esta_luchando())
                        {
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_jugador);
                            byte pm_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                            if (luchador != null)
                                luchador.pm -= pm_utilizados;

                            if (id_jugador == personaje.id)
                                cuenta.pelea.get_Movimiento_Exito();
                        }
                        break;

                    case 302:
                    case 300: //hechizo lanzado con exito
                        if (id_jugador == cuenta.personaje.id)
                            cuenta.pelea.get_Hechizo_Lanzado();
                        break;

                    case 501:
                        int tiempo_recoleccion = int.Parse(separador[3].Split(',')[1]);
                        short celda_id = short.Parse(separador[3].Split(',')[0]);

                        if (id_jugador == personaje.id)
                        {
                            cuenta.Estado_Cuenta = EstadoCuenta.RECOLECTANDO;
                            personaje.evento_Recoleccion_Iniciada();
                            await Task.Delay(tiempo_recoleccion);
                            cuenta.conexion.enviar_Paquete("GKK" + byte.Parse(separador[0]));
                        }
                        break;

                    case 900:
                        cuenta.conexion.enviar_Paquete("GA902" + id_jugador);
                        cuenta.logger.log_informacion("INFORMACIÓN", "Desafio del personaje id: " + id_jugador + " cancelado");
                        break;
                }
            }
        }

        [PaqueteAtributo("GDF")]
        public void get_Estado_Interactivo(ClienteAbstracto cliente, string paquete)
        {
            foreach (string interactivo in paquete.Substring(4).Split('|'))
            {
                string[] separador = interactivo.Split(';');
                Cuenta cuenta = cliente.cuenta;
                Personaje personaje = cuenta.personaje;
                short celda_id = short.Parse(separador[0]);
                byte estado = byte.Parse(separador[1]);

                switch (estado)
                {
                    case 2:
                        personaje.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;

                        if (personaje.celda_objetivo_recoleccion == celda_id && !cliente.cuenta.esta_recolectando())
                        {
                            cliente.cuenta.logger.log_informacion("INFORMACIÓN", "Un personaje te ha robado el recurso");
                            cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                            personaje.evento_Recoleccion_Acabada();
                        }
                        break;

                    case 3:
                        personaje.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;

                        //pescador no envia IQ porque aveces no pesca pero si manda GDF;3
                        if (cuenta.esta_recolectando() && personaje.celda_objetivo_recoleccion == celda_id)
                        {
                            cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
                            cuenta.personaje.evento_Recoleccion_Acabada();
                        }
                        break;

                    case 4:// reaparece
                        personaje.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;
                        break;
                }
            }
        }

        [PaqueteAtributo("PBF")]
        public void get_Antibot_1(ClienteAbstracto cliente, string paquete) => cliente.cuenta.conexion.enviar_Paquete(paquete.Substring(0, 2) + new Random().Next(120000, 140000));

        [PaqueteAtributo("GDM")]
        public void get_Nuevo_Mapa(ClienteAbstracto cliente, string paquete)
        {
            cliente.cuenta.personaje.mapa = new Mapa(cliente.cuenta, paquete.Substring(4));
            cliente.cuenta.Estado_Cuenta = EstadoCuenta.CONECTADO_INACTIVO;
            cliente.cuenta.personaje.evento_Mapa_Actualizado();
        }
    }
}

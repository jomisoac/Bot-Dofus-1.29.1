using Bot_Dofus_1._29._1.Comun.Frames.Transporte;
using Bot_Dofus_1._29._1.Comun.Network;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Monstruos;
using Bot_Dofus_1._29._1.Otros.Entidades.Npcs;
using Bot_Dofus_1._29._1.Otros.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Recolecciones;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Personajes;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;
using Bot_Dofus_1._29._1.Otros.Peleas.Peleadores;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using Bot_Dofus_1._29._1.Utilidades.Extensiones;
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
        [PaqueteAtributo("GM")]
        public async Task get_Movimientos_Personajes(ClienteTcp cliente, string paquete)
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
                        Celda celda = cuenta.juego.mapa.celdas[short.Parse(informaciones[0])];
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

                                cuenta.juego.mapa.agregar_Entidad(monstruo);
                            break;

                            case -4://NPC
                                cuenta.juego.mapa.agregar_Entidad(new Npcs(id, int.Parse(nombre_template), celda));
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

                            default:// jugador
                                if (cuenta.Estado_Cuenta != EstadoCuenta.LUCHANDO)
                                {
                                    if (cuenta.juego.personaje.id != id)
                                    {
                                        if (nombre_template.StartsWith("[") || Extensiones.lista_mods.Contains(nombre_template))
                                            cuenta.conexion.get_Desconectar_Socket();

                                        cuenta.juego.mapa.agregar_Entidad(new Personaje(id, nombre_template, byte.Parse(informaciones[7].ToString()), celda));
                                    }
                                    else
                                        cuenta.juego.personaje.celda = celda;
                                }
                                else
                                {
                                    int vida = int.Parse(informaciones[14]);
                                    byte pa = byte.Parse(informaciones[15]);
                                    byte pm = byte.Parse(informaciones[16]);
                                    byte equipo = byte.Parse(informaciones[24]);

                                    cuenta.pelea.get_Agregar_Luchador(new Luchadores(id, true, vida, pa, pm, celda.id, vida, equipo));

                                    if (cuenta.juego.personaje.id == id && cuenta.pelea_extension.configuracion.posicionamiento != PosicionamientoInicioPelea.INMOVIL)
                                    {
                                        await Task.Delay(300);

                                        /** la posicion es aleatoria pero el paquete GP siempre aparecera primero el team donde esta el pj **/
                                        short celda_posicion = cuenta.pelea.get_Celda_Mas_Cercana_O_Lejana(cuenta.pelea_extension.configuracion.posicionamiento == PosicionamientoInicioPelea.CERCA_DE_ENEMIGOS, cuenta.pelea.celdas_preparacion, cuenta.juego.mapa);

                                        if (celda_posicion != celda.id)
                                            cuenta.conexion.enviar_Paquete("Gp" + celda_posicion);
                                        else
                                            cuenta.conexion.enviar_Paquete("GR1");
                                    }
                                    else if (cuenta.juego.personaje.id == id)
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
                            cuenta.juego.mapa.eliminar_Entidad(id);
                        }
                    }
                }
            }
        }

        [PaqueteAtributo("GAF")]
        public void get_Finalizar_Accion(ClienteTcp cliente, string paquete)
        {
            string[] id_fin_accion = paquete.Substring(3).Split('|');

            cliente.cuenta.conexion.enviar_Paquete("GKK" + id_fin_accion[0]);
        }

        [PaqueteAtributo("GAS")]
        public async Task get_Inicio_Accion(ClienteTcp cliente, string paquete) => await Task.Delay(200);

        [PaqueteAtributo("GA")]
        public async Task get_Iniciar_Accion(ClienteTcp cliente, string paquete)
        {
            string[] separador = paquete.Substring(2).Split(';');
            int id_accion = int.Parse(separador[1]);
            Cuenta cuenta = cliente.cuenta;
            Personaje personaje = cuenta.juego.personaje;

            if (id_accion > 0)//Error: GA;0
            {
                int id_entidad = int.Parse(separador[2]);
                Luchadores luchador = null;

                switch (id_accion)
                {
                    case 1:
                        Mapa mapa = cuenta.juego.mapa;
                        Celda celda_destino = mapa.celdas[Hash.get_Celda_Id_Desde_hash(separador[3].Substring(separador[3].Length - 2))];
                        byte tipo_gkk_movimiento;

                        if (!cuenta.esta_luchando())
                        {
                            if (id_entidad == personaje.id && celda_destino.id > 0 && personaje.celda.id != celda_destino.id)
                            {
                                tipo_gkk_movimiento = byte.Parse(separador[0]);
                                
                                await cuenta.juego.manejador.movimientos.evento_Movimiento_Finalizado(celda_destino, tipo_gkk_movimiento, true);
                            }
                            else if (mapa.personajes.TryGetValue(id_entidad, out Personaje personaje_mapa))
                            {
                                personaje_mapa.celda = celda_destino;

                                if (GlobalConf.mostrar_mensajes_debug)
                                    cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un personaje a la casilla: " + celda_destino.id);
                            }
                            else if (mapa.monstruos.TryGetValue(id_entidad, out Monstruo monstruo_mapa))
                            {
                                monstruo_mapa.celda = celda_destino;
                                if (GlobalConf.mostrar_mensajes_debug)
                                    cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de un grupo de monstruo a la casilla: " + celda_destino.id);
                            }
                            else if (mapa.npcs.TryGetValue(id_entidad, out Npcs npc_mapa))
                            {
                                npc_mapa.celda = celda_destino;

                                if (GlobalConf.mostrar_mensajes_debug)
                                    cuenta.logger.log_informacion("DEBUG", "Detectado movimiento de npc a la casilla: " + celda_destino.id);
                            }
                            mapa.evento_Entidad_Actualizada();
                        }
                        else
                        {
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_entidad);
                            if (luchador != null)
                            {
                                luchador.celda_id = celda_destino.id;

                                if (luchador.id == personaje.id)
                                {
                                    tipo_gkk_movimiento = byte.Parse(separador[0]);

                                    await Task.Delay(300 * personaje.celda.get_Distancia_Entre_Dos_Casillas(celda_destino));
                                    cuenta.conexion.enviar_Paquete("GKK" + tipo_gkk_movimiento);
                                }
                            }
                        }
                    break;

                    case 2: //Cargando el mapa
                        await Task.Delay(200);
                    break;

                    case 102:
                        if (cuenta.esta_luchando())
                        {
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_entidad);
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
                            luchador = cuenta.pelea.get_Luchador_Por_Id(id_entidad);
                            byte pm_utilizados = byte.Parse(separador[3].Split(',')[1].Substring(1));

                            if (luchador != null)
                                luchador.pm -= pm_utilizados;

                            if (id_entidad == personaje.id)
                                cuenta.pelea.get_Movimiento_Exito();
                        }
                    break;

                    case 181: //efecto de invocacion (pelea)
                        short celda = short.Parse(separador[3].Substring(1));
                        short id_luchador = short.Parse(separador[6]);
                        short vida = short.Parse(separador[15]);
                        byte pa = byte.Parse(separador[16]);
                        byte pm = byte.Parse(separador[17]);
                        byte equipo = byte.Parse(separador[25]);

                        cuenta.pelea.get_Agregar_Luchador(new Luchadores(id_luchador, true, vida, pa, pm, celda, vida, equipo, id_entidad));
                    break;

                    case 302:
                    case 300: //hechizo lanzado con exito
                        if (id_entidad == cuenta.juego.personaje.id)
                            cuenta.pelea.get_Hechizo_Lanzado();
                        break;

                    case 501:
                        int tiempo_recoleccion = int.Parse(separador[3].Split(',')[1]);
                        short celda_id = short.Parse(separador[3].Split(',')[0]);
                        byte tipo_gkk_recoleccion = byte.Parse(separador[0]);

                        await cuenta.juego.manejador.recoleccion.evento_Recoleccion_Iniciada(id_entidad, tiempo_recoleccion, celda_id, tipo_gkk_recoleccion);
                    break;

                    case 900:
                        cuenta.conexion.enviar_Paquete("GA902" + id_entidad);
                        cuenta.logger.log_informacion("INFORMACIÓN", "Desafio del personaje id: " + id_entidad + " cancelado");
                    break;
                }
            }
            else
            {
                if(!cuenta.esta_luchando())
                await cuenta.juego.manejador.movimientos.evento_Movimiento_Finalizado(null, 0, false);
            }
        }

        [PaqueteAtributo("GDF")]
        public void get_Estado_Interactivo(ClienteTcp cliente, string paquete)
        {
            foreach (string interactivo in paquete.Substring(4).Split('|'))
            {
                string[] separador = interactivo.Split(';');
                Cuenta cuenta = cliente.cuenta;
                short celda_id = short.Parse(separador[0]);
                byte estado = byte.Parse(separador[1]);

                switch (estado)
                {
                    case 2:
                        cuenta.juego.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;
                    break;

                    case 3:
                        cuenta.juego.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;

                        if(cuenta.esta_recolectando())
                            cuenta.juego.manejador.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.RECOLECTADO, celda_id);
                        else
                            cuenta.juego.manejador.recoleccion.evento_Recoleccion_Acabada(RecoleccionResultado.ROBADO, celda_id);
                    break;

                    case 4:// reaparece asi se fuerza el cambio de mapa 
                        cuenta.juego.mapa.celdas[celda_id].objeto_interactivo.es_utilizable = false;
                    break;
                }
            }
        }

        [PaqueteAtributo("GDM")]
        public void get_Nuevo_Mapa(ClienteTcp cliente, string paquete) => Task.Run(() =>
        {
            cliente.cuenta.juego.mapa.get_Actualizar_Mapa(paquete.Substring(4));
        }).Wait();

        [PaqueteAtributo("GDK")]
        public void get_Mapa_Cambiado(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            cuenta.juego.mapa.get_Evento_Mapa_Cambiado();
        }

        [PaqueteAtributo("GV")]
        public void get_Reiniciar_Pantalla(ClienteTcp cliente, string paquete)
        {
            Cuenta cuenta = cliente.cuenta;
            cuenta.conexion.enviar_Paquete("GC1");
        }
    }
}

using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public class CambiarMapaAccion : AccionesScript
    {
        public MapaTeleportCeldas direccion { get; private set; }
        public short celda_id { get; private set; }

        public bool celda_especifica => direccion == MapaTeleportCeldas.NINGUNO && celda_id != -1;
        public bool direccion_especifica => direccion != MapaTeleportCeldas.NINGUNO && celda_id == -1;

        public CambiarMapaAccion(MapaTeleportCeldas _direccion, short _celda_id)
        {
            direccion = _direccion;
            celda_id = _celda_id;
        }

        internal override Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            if (celda_especifica)
            {
                if (cuenta.personaje.mapa.get_Mover_Celda_Mapa(cuenta.personaje.celda.id, celda_id, true) != ResultadoMovimientos.EXITO)
                {
                    cuenta.logger.log_Error("SCRIPT", "Error al mover a la celda especificada: ");
                    return resultado_fallado;
                }
            }
            else if (direccion_especifica)
            {
                short celda_teleport = cuenta.personaje.mapa.celdas.Where(celda => celda.tipo_teleport == direccion).Select(celda => celda.id).SingleOrDefault();

                if (cuenta.personaje.mapa.get_Mover_Celda_Mapa(cuenta.personaje.celda.id, celda_teleport, true) != ResultadoMovimientos.EXITO)
                {
                    cuenta.logger.log_Error("SCRIPT", "Error al encontrar la celda teleport, usa el metodo por id");
                    return resultado_fallado;
                }
            }

            return resultado_procesado;
        }

        public static bool TryParse(string texto, out CambiarMapaAccion accion)
        {
            string[] partes = texto.Split('|');
            string total_partes = partes[Randomize.get_Random_Int(0, partes.Length)];

            Match match = Regex.Match(total_partes, @"(?<direccion>arriba|derecha|abajo|izquierda)\((?<celda>\d{1,3})\)");
            if (match.Success)
            {
                accion = new CambiarMapaAccion((MapaTeleportCeldas)Enum.Parse(typeof(MapaTeleportCeldas), match.Groups["direccion"].Value, true), short.Parse(match.Groups["celda"].Value));
                return true;
            }
            else
            {
                match = Regex.Match(total_partes, @"(?<direccion>arriba|derecha|abajo|izquierda)");
                if (match.Success)
                {
                    accion = new CambiarMapaAccion((MapaTeleportCeldas)Enum.Parse(typeof(MapaTeleportCeldas), match.Groups["direccion"].Value, true), -1);
                    return true;
                }
                else
                {
                    match = Regex.Match(total_partes, @"(?<celda>\d{1,3})");
                    if (match.Success)
                    {
                        accion = new CambiarMapaAccion(MapaTeleportCeldas.NINGUNO, short.Parse(match.Groups["celda"].Value));
                        return true;
                    }
                }
            }
            accion = null;
            return false;
        }
    }
}

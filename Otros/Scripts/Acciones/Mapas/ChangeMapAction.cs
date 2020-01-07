using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Utilities.Crypto;
using System;
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
    public class ChangeMapAction : ScriptAction
    {
        public MapaTeleportCeldas direccion { get; private set; }
        public short celda_id { get; private set; }

        public bool celda_especifica => direccion == MapaTeleportCeldas.NINGUNO && celda_id != -1;
        public bool direccion_especifica => direccion != MapaTeleportCeldas.NINGUNO && celda_id == -1;

        public ChangeMapAction(MapaTeleportCeldas _direccion, short _celda_id)
        {
            direccion = _direccion;
            celda_id = _celda_id;
        }

        internal override Task<ResultadosAcciones> process(Account account)
        {
            if (celda_especifica)
            {
                Cell celda = account.game.map.GetCellFromId(celda_id);
                bool result = account.game.manager.movimientos.get_Cambiar_Mapa(direccion, celda);

                if (account.hasGroup && account.isGroupLeader)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        foreach (var groupMember in account.group.members)
                        {
                            celda = groupMember.game.map.GetCellFromId(celda_id);
                            await Task.Delay(500);
                            groupMember.game.manager.movimientos.get_Cambiar_Mapa(direccion, celda);
                        }
                    });
                }
            }
            else if (direccion_especifica)
            {
                bool result = account.game.manager.movimientos.get_Cambiar_Mapa(direccion);
                if (account.hasGroup && account.isGroupLeader)
                {
                    Task.Factory.StartNew(async () =>
                    {
                        foreach (var groupMember in account.group.members)
                        {
                            await Task.Delay(500);
                            groupMember.game.manager.movimientos.get_Cambiar_Mapa(direccion);
                        }       
                    });
                }
            }

            return resultado_procesado;
        }

        public static bool TryParse(string texto, out ChangeMapAction accion)
        {
            string[] partes = texto.Split('|');
            string total_partes = partes[Randomize.get_Random(0, partes.Length)];

            Match match = Regex.Match(total_partes, @"(?<direction>TOP|RIGHT|BOTTOM|LEFT)\((?<cell>\d{1,3})\)");
            if (match.Success)
            {
                accion = new ChangeMapAction((MapaTeleportCeldas)Enum.Parse(typeof(MapaTeleportCeldas), match.Groups["direction"].Value, true), short.Parse(match.Groups["cell"].Value));
                return true;
            }
            else
            {
                match = Regex.Match(total_partes, @"(?<direction>TOP|RIGHT|BOTTOM|LEFT)");
                if (match.Success)
                {
                    accion = new ChangeMapAction((MapaTeleportCeldas)Enum.Parse(typeof(MapaTeleportCeldas), match.Groups["direction"].Value, true), -1);
                    return true;
                }
                else
                {
                    match = Regex.Match(total_partes, @"(?<cell>\d{1,3})");
                    if (match.Success)
                    {
                        accion = new ChangeMapAction(MapaTeleportCeldas.NINGUNO, short.Parse(match.Groups["cell"].Value));
                        return true;
                    }
                }
            }
            accion = null;
            return false;
        }
    }
}

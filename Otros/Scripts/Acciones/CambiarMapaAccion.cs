using Bot_Dofus_1._29._1.Otros.Mapas.Movimiento;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Utilidades.Criptografia;
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
    public class CambiarMapaAccion : AccionesScript
    {
        public int celda_id { get; private set; }

        public CambiarMapaAccion(int _celda_id) => celda_id = _celda_id;

        internal override async Task<ResultadosAcciones> proceso(Cuenta cuenta)
        {
            ResultadoMovimientos reusltado_movimiento = await cuenta.personaje.mapa.get_Mover_Celda_Resultado(cuenta.personaje.celda_id, celda_id, true);

            if (reusltado_movimiento != ResultadoMovimientos.EXITO)
                return ResultadosAcciones.FALLO;

            return ResultadosAcciones.PROCESANDO;
        }

        public static bool TryParse(string texto, out CambiarMapaAccion accion)
        {
            string[] partes = texto.Split('|');
            string randomPart = partes[Randomize.get_Random_Int(0, partes.Length)];

            Match match = Regex.Match(randomPart, @"(?<celda>\d{1,3})");
            if (match.Success)
            {
                accion = new CambiarMapaAccion(int.Parse(match.Groups["celda"].Value));
                return true;
            }
            accion = null;
            return false;
        }
    }
}

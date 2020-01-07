using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory;
using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory.Enums;
using Bot_Dofus_1._29._1.Otros.Game.Entidades.Manejadores.Movimientos;
using Bot_Dofus_1._29._1.Otros.Mapas;
using Bot_Dofus_1._29._1.Otros.Mapas.Entidades;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Scripts.Acciones
{
    public class PeleasAccion : ScriptAction
    {
        public int monstruos_minimos { get; private set; }
        public int monstruos_maximos { get; private set; }
        public int monstruo_nivel_minimo { get; private set; }
        public int monstruo_nivel_maximo { get; private set; }
        public List<int> monstruos_prohibidos { get; private set; }
        public List<int> monstruos_obligatorios { get; private set; }
        public bool capture { get; private set; }
        public List<KeyValuePair<int, int>> monstre_capturable_nombre { get; private set; }
        public string character_capture { get; private set; }
        public int id_capture { get; private set; }

        public int cac_id { get; private set; }


        public PeleasAccion(int _monstruos_minimos, int _monstruos_maximos, int _monstruo_nivel_minimo, int _monstruo_nivel_maximo, List<int> _monstruos_prohibidos, List<int> _monstruos_obligatorios)
        {
            monstruos_minimos = _monstruos_minimos;
            monstruos_maximos = _monstruos_maximos;
            monstruo_nivel_minimo = _monstruo_nivel_minimo;
            monstruo_nivel_maximo = _monstruo_nivel_maximo;
            monstruos_prohibidos = _monstruos_prohibidos;
            monstruos_obligatorios = _monstruos_obligatorios;
        }

        public PeleasAccion(int _monstruos_minimos, int _monstruos_maximos, int _monstruo_nivel_minimo, int _monstruo_nivel_maximo, List<int> _monstruos_prohibidos, List<int> _monstruos_obligatorios, bool _capture, List<KeyValuePair<int, int>> _monstre_capturable_nombre, string _character_capture, int _id_capture, int _cac_id)
        {
            monstruos_minimos = _monstruos_minimos;
            monstruos_maximos = _monstruos_maximos;
            monstruo_nivel_minimo = _monstruo_nivel_minimo;
            monstruo_nivel_maximo = _monstruo_nivel_maximo;
            monstruos_prohibidos = _monstruos_prohibidos;
            monstruos_obligatorios = _monstruos_obligatorios;
            capture = _capture;
            monstre_capturable_nombre = _monstre_capturable_nombre;
            character_capture = _character_capture;
            id_capture = _id_capture;
            cac_id = _cac_id;
        }

        internal override Task<ResultadosAcciones> process(Account account)
        {
            Map mapa = account.game.map;

            List<Monstruos> availableGroups = mapa.get_Grupo_Monstruos(monstruos_minimos, monstruos_maximos, monstruo_nivel_minimo, monstruo_nivel_maximo, monstruos_prohibidos, monstruos_obligatorios);
            /* reset si je le perso doit capturer ou pas */
            account.needToCapture = false;
            account.capturefight = false;
            account.capturelance = false;
            bool capture_equipped = false;
            if (account.hasGroup == true)
            {
                foreach (var membre in account.group.members)
                {
                    membre.needToCapture = false;
                    membre.capturefight = false;
                    membre.capturelance = false;
                }
            }

            if (availableGroups.Count > 0)
            {
                /* verifie si groupe capturable */ 
                foreach (Monstruos monsterGroup in availableGroups)
                {
                    if (account.game.map.isGroupCapturable(monstre_capturable_nombre, monsterGroup) == true  && capture==true)
                    {
                        var moveResult = account.game.manager.movimientos.get_Mover_A_Celda(monsterGroup.celda, account.game.map.celdas_ocupadas().Where(c => c.cellType == CellTypes.TELEPORT_CELL).ToList());
                        switch (moveResult)
                        {
                            case ResultadoMovimientos.EXITO:
                              
                                /* gestion capture */
                                    account.Logger.LogInfo("SCRIPT", $"Le groupe est capturable on equipe la pierre");
                                    account.capturefight = true;
                                    if (account.isGroupLeader == true && account.hasGroup == true)
                                    {
                                        foreach (var item in account.group.members)
                                        {
                                            item.capturefight = true;
                                        }
                                    }

                                    /*equiper capture is leader = capturer */
                                    if (character_capture.ToLower() == account.game.character.nombre.ToLower())
                                    {

                                        InventoryObject obj = account.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == id_capture && o.posicion == InventorySlots.NOT_EQUIPPED);
                                        if (obj != null)
                                        {
                                            account.game.character.inventario.equipar_Objeto(obj);
                                            account.Logger.LogInfo("SCRIPT", $"La capture(" + id_capture + ") a été équipé  sur :" + character_capture);
                                            capture_equipped = true;
                                            account.needToCapture = true;
                                        }
                                        else
                                        {
                                            InventoryObject obj2 = account.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == id_capture && o.posicion == InventorySlots.WEAPON);
                                            if (obj2 != null)
                                            {
                                                account.Logger.LogInfo("SCRIPT", $"La capture(" + id_capture + ") été déja equipé sur : " + character_capture);
                                                account.needToCapture = true;
                                                capture_equipped = true;
                                            }
                                        }

                                    }
                                    else if (capture_equipped == false && account.hasGroup == true)
                                    {
                                        foreach (var membre in account.group.members)
                                        {
                                            if (character_capture.ToLower() == membre.game.character.nombre.ToLower())
                                            {

                                                InventoryObject obj = membre.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == id_capture && o.posicion == InventorySlots.NOT_EQUIPPED);

                                                if (obj != null)
                                                {
                                                    account.Logger.LogInfo("SCRIPT", $"La capture(" + id_capture + ") a été équipé sur : " + character_capture);
                                                    membre.game.character.inventario.equipar_Objeto(obj);
                                                    capture_equipped = true;
                                                    membre.needToCapture = true;
                                                }
                                                else
                                                {
                                                    InventoryObject obj2 = membre.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == id_capture && o.posicion == InventorySlots.WEAPON);
                                                    if (obj2 != null)
                                                    {
                                                        account.Logger.LogInfo("SCRIPT", $"La capture(" + id_capture + ") été déja equipé sur : " + character_capture);
                                                        capture_equipped = true;
                                                        membre.needToCapture = true;
                                                    }

                                                }
                                            }
                                        }
                                    }
                                    if (capture_equipped == false)
                                    {
                                        account.Logger.LogDanger("SCRIPT", $"La capture(" + id_capture + ") n'a pas pu étre équipé sur" + character_capture + ", non présente dans l'inventaire");
                                    }

                                account.Logger.LogInfo("SCRIPT", $"Mouvent vers un groupes à la cellule : {monsterGroup.celda.cellId}, monstres total: {monsterGroup.get_Total_Monstruos}, niveaux du groupe: {monsterGroup.get_Total_Nivel_Grupo}");
                                return resultado_procesado;

                            case ResultadoMovimientos.PathfindingError:
                            case ResultadoMovimientos.SameCell:
                                account.Logger.LogDanger("SCRIPT", $"Le chemin vers le groupe de monstres est bloqué. Raison : {moveResult}");
                                continue;

                            default:
                                account.script.detener_Script($"Erreur lors de la tentative de déplacement vers un groupe à la cellule : {monsterGroup.celda.cellId}. Raison : {moveResult}");
                                return resultado_fallado;
                        }
                    }

                }
                /* si aucun groupe capturable */
                foreach (Monstruos monsterGroup in availableGroups)
                {
                    var moveResult = account.game.manager.movimientos.get_Mover_A_Celda(monsterGroup.celda, account.game.map.celdas_ocupadas().Where(c => c.cellType == CellTypes.TELEPORT_CELL).ToList());
                    switch (moveResult)
                    {
                        case ResultadoMovimientos.EXITO:
                            /* on rééquipe le CAC */
                            if (character_capture.ToLower() == account.game.character.nombre.ToLower() &&  capture == true)
                            {
                                InventoryObject obj = account.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == cac_id && o.posicion == InventorySlots.NOT_EQUIPPED);

                                if (obj != null)
                                {
                                    account.game.character.inventario.equipar_Objeto(obj);
                                    account.Logger.LogInfo("SCRIPT", $"Le CAC (" + cac_id + ") a été rééquipé " + character_capture);
                                }
                                else
                                {
                                    InventoryObject obj2 = account.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == cac_id && o.posicion == InventorySlots.WEAPON);
                                    if (obj2 != null)
                                        account.Logger.LogDanger("SCRIPT", $"Le CAC (" + cac_id + ") été déja équipé sur : " + character_capture);
                                    else
                                        account.Logger.LogDanger("SCRIPT", $"Le CAC (" + cac_id + ") n'a pas été trouvé sur : " + character_capture);
                                }
                            }
                            else if (account.hasGroup == true &&  capture == true)
                            {
                                foreach (var membre in account.group.members)
                                {
                                    if (character_capture.ToLower() == membre.game.character.nombre.ToLower())
                                    {
                                        InventoryObject obj = membre.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == cac_id);

                                        if (obj != null && obj.posicion == InventorySlots.NOT_EQUIPPED)
                                        {
                                            membre.game.character.inventario.equipar_Objeto(obj);
                                            account.Logger.LogInfo("SCRIPT", $"Le CAC (" + cac_id + ") a été rééquipé  sur :" + character_capture);
                                        }
                                        else
                                        {
                                            InventoryObject obj2 = membre.game.character.inventario.equipamiento.FirstOrDefault(o => o.id_modelo == cac_id && o.posicion == InventorySlots.WEAPON);
                                            if (obj2 != null)
                                                account.Logger.LogDanger("SCRIPT", $"Le CAC (" + cac_id + ") été déja équipé sur : " + character_capture);
                                            else
                                                account.Logger.LogDanger("SCRIPT", $"Le CAC (" + cac_id + ") n'a pas été trouvé sur : " + character_capture);
                                        }
                                    }
                                }
                            }
                            account.Logger.LogInfo("SCRIPT", $"Mouvent vers un groupes à la cellule : {monsterGroup.celda.cellId}, monstres total: {monsterGroup.get_Total_Monstruos}, niveaux du groupe: {monsterGroup.get_Total_Nivel_Grupo}");
                            return resultado_procesado;

                        case ResultadoMovimientos.PathfindingError:
                        case ResultadoMovimientos.SameCell:
                            account.Logger.LogDanger("SCRIPT", $"Le chemin vers le groupe de monstres est bloqué. Raison : {moveResult}");
                            continue;

                        default:
                            account.script.detener_Script($"Erreur lors de la tentative de déplacement vers un groupe à la cellule : {monsterGroup.celda.cellId}. Raison : {moveResult}");
                            return resultado_fallado;
                    }
                }

            }

            return resultado_hecho;
        }
    }
}

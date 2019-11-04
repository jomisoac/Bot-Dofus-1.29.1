using Bot_Dofus_1._29._1.Otros.Game.Character.Inventory.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot_Dofus_1._29._1.Otros.Game.Character.Inventory
{
    public class InventoryClass : IDisposable, IEliminable
    {
        private Account cuenta;
        private ConcurrentDictionary<uint, InventoryObject> _objetos;
        private bool disposed;

        //Propiedades
        public int kamas { get; private set; }
        public short pods_actuales { get; set; }
        public short pods_maximos { get; set; }

        public IEnumerable<InventoryObject> objetos => _objetos.Values;
        public IEnumerable<InventoryObject> equipamiento => objetos.Where(o => o.tipo_inventario == InventoryObjectsTypes.EQUIPMENTS);
        public IEnumerable<InventoryObject> varios => objetos.Where(o => o.tipo_inventario == InventoryObjectsTypes.MISCELLANEOUS);
        public IEnumerable<InventoryObject> recursos => objetos.Where(o => o.tipo_inventario == InventoryObjectsTypes.RESOURCES);
        public IEnumerable<InventoryObject> mision => objetos.Where(o => o.tipo_inventario == InventoryObjectsTypes.QUEST_ITEMS);
        public int porcentaje_pods => (int)((double)pods_actuales / pods_maximos * 100);

        public event Action<bool> inventario_actualizado;
        public event Action almacenamiento_abierto;
        public event Action almacenamiento_cerrado;

        // Constructor
        internal InventoryClass(Account _cuenta)
        {
            cuenta = _cuenta;
            _objetos = new ConcurrentDictionary<uint, InventoryObject>();
        }

        public InventoryObject get_Objeto_Modelo_Id(int gid) => objetos.FirstOrDefault(f => f.id_modelo == gid);
        public InventoryObject get_Objeto_en_Posicion(InventorySlots posicion) => objetos.FirstOrDefault(o => o.posicion == posicion);

        public void agregar_Objetos(string paquete)
        {
            Task.Run(() =>
            {
                foreach (string obj in paquete.Split(';'))
                {
                    if (!string.IsNullOrEmpty(obj))
                    {
                        string[] separador = obj.Split('~');
                        uint id_inventario = Convert.ToUInt32(separador[0], 16);
                        InventoryObject objeto = new InventoryObject(obj);
                        _objetos.TryAdd(id_inventario, objeto);
                    }
                }
            }).Wait();

            inventario_actualizado?.Invoke(true);
        }

        public void modificar_Objetos(string paquete)
        {
            if (!string.IsNullOrEmpty(paquete))
            {
                string[] separador = paquete.Split('|');
                InventoryObject objeto = objetos.FirstOrDefault(f => f.id_inventario == uint.Parse(separador[0]));

                if (objeto != null)
                {
                    int cantidad = int.Parse(separador[1]);
                    InventoryObject nuevo_objeto = objeto;
                    nuevo_objeto.cantidad = cantidad;

                    if (_objetos.TryUpdate(objeto.id_inventario, nuevo_objeto, objeto))
                        inventario_actualizado?.Invoke(true);
                }
            }
        }

        public void eliminar_Objeto(InventoryObject obj, int cantidad, bool paquete_eliminar)
        {
            if (obj == null)
                return;

            cantidad = cantidad == 0 ? obj.cantidad : cantidad > obj.cantidad ? obj.cantidad : cantidad;

            if (obj.cantidad > cantidad)
            {
                InventoryObject nuevo_objeto = obj;
                nuevo_objeto.cantidad -= cantidad;
                _objetos.TryUpdate(obj.id_inventario, nuevo_objeto, obj);
            }
            else
                _objetos.TryRemove(obj.id_inventario, out InventoryObject objeto);

            if (paquete_eliminar)
            {
                cuenta.connexion.SendPacket($"Od{obj.id_inventario}|{cantidad}");
                cuenta.Logger.LogInfo("INVENTAIRE", $"{cantidad} {obj.nombre} éliminée(s).");
            }

            inventario_actualizado?.Invoke(true);
        }

        public void eliminar_Objeto(uint id_inventario, int cantidad, bool paquete_eliminar)
        {
            if(!_objetos.TryGetValue(id_inventario, out InventoryObject obj))
                return;

            eliminar_Objeto(obj, cantidad, paquete_eliminar);
        }

        public bool equipar_Objeto(InventoryObject objeto)
        {
            if (objeto == null || objeto.cantidad == 0 || cuenta.Is_Busy())
            {
                cuenta.Logger.LogError("INVENTAIRE", $"L'objet {objeto.nombre} ne peux être équipé");
                return false;
            }

            if (objeto.nivel > cuenta.game.character.nivel)
            {
                cuenta.Logger.LogError("INVENTAIRE", $"Le niveau de l'objet {objeto.nombre} est supérieur à ton niveau");
                return false;
            }

            if (objeto.posicion != InventorySlots.NOT_EQUIPPED)//objeto ya esta equipado
            {
                cuenta.Logger.LogError("INVENTAIRE", $"l'objet {objeto.nombre} est équipé");
                return false;
            }

            List<InventorySlots> possibles_posiciones = InventoryUtilities.get_Posibles_Posiciones(objeto.tipo);

            if (possibles_posiciones == null || possibles_posiciones.Count == 0)//objeto no equipable
            {
                cuenta.Logger.LogError("INVENTARIO", $"L'objet {objeto.nombre} n'est pas équipable");
                return false;
            }

            foreach (InventorySlots posicion in possibles_posiciones)
            {
                if (get_Objeto_en_Posicion(posicion) == null)
                {
                    cuenta.connexion.SendPacket("OM" + objeto.id_inventario + "|" + (sbyte)posicion, true);
                    cuenta.Logger.LogInfo("INVENTAIRE", $"{objeto.nombre} équipé.");
                    objeto.posicion = posicion;
                    inventario_actualizado?.Invoke(true);
                    return true;
                }
            }

            //desequipa X objeto si ya esta equipado
            if (_objetos.TryGetValue(get_Objeto_en_Posicion(possibles_posiciones[0]).id_inventario, out InventoryObject objeto_equipado))
            {
                objeto_equipado.posicion = InventorySlots.NOT_EQUIPPED;
                cuenta.connexion.SendPacket("OM" + objeto_equipado.id_inventario + "|" + (sbyte)InventorySlots.NOT_EQUIPPED);
            }

            cuenta.connexion.SendPacket("OM" + objeto.id_inventario + "|" + (sbyte)possibles_posiciones[0]);

            if (objeto.cantidad == 1)
                objeto.posicion = possibles_posiciones[0];

            cuenta.Logger.LogInfo("INVENTAIRE", $"{objeto.nombre} équipé.");
            inventario_actualizado?.Invoke(true);
            return true;
        }

        public bool desequipar_Objeto(InventoryObject objeto)
        {
            if (objeto == null)
                return false;

            if (objeto.posicion == InventorySlots.NOT_EQUIPPED)
                return false;

            cuenta.connexion.SendPacket("OM" + objeto.id_inventario + "|" + (sbyte)InventorySlots.NOT_EQUIPPED);
            objeto.posicion = InventorySlots.NOT_EQUIPPED;
            cuenta.Logger.LogInfo("INVENTAIRE", $"{objeto.nombre} déséquipé.");
            inventario_actualizado?.Invoke(true);
            return true;
        }

        public void utilizar_Objeto(InventoryObject objeto)
        {
            if (objeto == null)
                return;

            if(objeto.cantidad == 0)
            {
                cuenta.Logger.LogError("INVENTAIRE", $"L'objet {objeto.nombre} ne peut être mis à cause de tes caractéristiques");
                return;
            }

            cuenta.connexion.SendPacket("OU" + objeto.id_inventario + "|");
            eliminar_Objeto(objeto, 1, false);
            cuenta.Logger.LogInfo("INVENTAIRE", $"{objeto.nombre} utilisée.");
        }

        public void evento_Almacenamiento_Abierto() => almacenamiento_abierto?.Invoke();
        public void evento_Almacenamiento_Cerrado() => almacenamiento_cerrado?.Invoke();

        #region Zona Dispose
        public void Dispose() => Dispose(true);
        ~InventoryClass() => Dispose(false);

        public void Clear()
        {
            kamas = 0;
            pods_actuales = 0;
            pods_maximos = 0;
            _objetos.Clear();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                _objetos.Clear();
                _objetos = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}

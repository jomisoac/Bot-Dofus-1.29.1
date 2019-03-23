using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Inventario
{
    public class InventarioGeneral : IDisposable
    {
        private Cuenta cuenta;
        private ConcurrentDictionary<uint, ObjetosInventario> _objetos;
        private bool disposed;

        //Propiedades
        public int kamas { get; private set; }
        public short pods_actuales { get; set; }
        public short pods_maximos { get; set; }

        public IEnumerable<ObjetosInventario> objetos => _objetos.Values;
        public IEnumerable<ObjetosInventario> equipamiento => objetos.Where(o => o.tipo == TipoObjetosInventario.EQUIPAMIENTO);
        public IEnumerable<ObjetosInventario> recursos => objetos.Where(o => o.tipo == TipoObjetosInventario.RECURSOS);
        public int porcentaje_pods => (int)((double)pods_actuales / pods_maximos * 100);

        public event Action<bool> inventario_actualizado;

        // Constructor
        internal InventarioGeneral(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            _objetos = new ConcurrentDictionary<uint, ObjetosInventario>();
        }

        public void agregar_Objetos(string paquete)
        {
            foreach (string objetos in paquete.Split(';'))
            {
                if (!objetos.Equals(string.Empty))
                {
                    string[] separador = objetos.Split('~');
                    _objetos.TryAdd(Convert.ToUInt32(separador[0], 16), new ObjetosInventario(objetos));
                }
            }
            inventario_actualizado?.Invoke(true);
        }

        public void eliminar_Objetos(ObjetosInventario obj, int cantidad)
        {
            if (obj == null)
                return;

            cantidad = cantidad == 0 ? obj.cantidad : cantidad > obj.cantidad ? obj.cantidad : cantidad;

            _objetos.TryRemove(obj.id_inventario, out ObjetosInventario objeto);
            cuenta.conexion.enviar_Paquete($"Od{obj.id_inventario}|{cantidad}");
            cuenta.logger.log_informacion("Inventario", $"{cantidad} {obj.nombre} eliminados(s).");
            inventario_actualizado?.Invoke(true);
        }

        #region Zona Dispose
        ~InventarioGeneral() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

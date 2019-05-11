using Bot_Dofus_1._29._1.Protocolo.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public IEnumerable<ObjetosInventario> equipamiento => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.EQUIPAMIENTO);
        public IEnumerable<ObjetosInventario> varios => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.VARIOS);
        public IEnumerable<ObjetosInventario> recursos => objetos.Where(o => o.tipo_inventario == TipoObjetosInventario.RECURSOS);
        public int porcentaje_pods => (int)((double)pods_actuales / pods_maximos * 100);

        public event Action<bool> inventario_actualizado;
        public event Action almacenamiento_abierto;
        public event Action almacenamiento_cerrado;

        // Constructor
        internal InventarioGeneral(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            _objetos = new ConcurrentDictionary<uint, ObjetosInventario>();
        }

        public void agregar_Objetos(string paquete)
        {
            foreach (string obj in paquete.Split(';'))
            {
                if (!obj.Equals(string.Empty))
                {
                    string[] separador = obj.Split('~');
                    int cantidad = Convert.ToInt32(separador[2], 16);
                    
                    ObjetosInventario objeto = objetos.FirstOrDefault(f => f.id_modelo == Convert.ToUInt32(separador[1], 16));

                    if (objeto != null)
                    {
                        ObjetosInventario nuevo_objeto = objeto;

                        if (cantidad > 0)
                            nuevo_objeto.cantidad += cantidad;
                        else
                            nuevo_objeto.cantidad -= cantidad;

                        _objetos.TryUpdate(objeto.id_inventario, nuevo_objeto, objeto);
                    }
                    else
                    {
                        _objetos.TryAdd(Convert.ToUInt32(separador[0], 16), new ObjetosInventario(obj));
                    }

                }
            }
            inventario_actualizado?.Invoke(true);
        }

        public void modificar_Objetos(string paquete)
        {
            if (!string.IsNullOrEmpty(paquete))
            {
                string[] separador = paquete.Split('|');
                ObjetosInventario objeto = objetos.FirstOrDefault(f => f.id_inventario == uint.Parse(separador[0]));

                if (objeto != null)
                {
                    int cantidad = int.Parse(separador[1]);
                    ObjetosInventario nuevo_objeto = objeto;
                    nuevo_objeto.cantidad = cantidad;

                    _objetos.TryUpdate(objeto.id_inventario, nuevo_objeto, objeto);
                }
                inventario_actualizado?.Invoke(true);
            }
        }

        public void eliminar_Objetos(ObjetosInventario obj, int cantidad, bool paquete_eliminar)
        {
            if (obj == null)
                return;

            cantidad = cantidad == 0 ? obj.cantidad : cantidad > obj.cantidad ? obj.cantidad : cantidad;

            if (obj.cantidad <= cantidad)
                _objetos.TryRemove(obj.id_inventario, out ObjetosInventario objeto);
            else
            {
                ObjetosInventario nuevo_objeto = obj;
                nuevo_objeto.cantidad -= cantidad;
                _objetos.TryUpdate(obj.id_inventario, nuevo_objeto, obj);
            }

            if (paquete_eliminar)
            {
                cuenta.conexion.enviar_Paquete($"Od{obj.id_inventario}|{cantidad}");
                cuenta.logger.log_informacion("Inventario", $"{cantidad} {obj.nombre} eliminados(s).");
                inventario_actualizado?.Invoke(true);
            }
        }

        public void evento_Actualizar_Inventario() => inventario_actualizado?.Invoke(true);
        public void evento_Almacenamiento_Abierto() => almacenamiento_abierto?.Invoke();
        public void evento_Almacenamiento_Cerrado() => almacenamiento_cerrado?.Invoke();

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

using Bot_Dofus_1._29._1.Otros.Enums;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Utilidades.Extensiones
{
    public static class Extensiones
    {
        public static string cadena_Amigable(this EstadoCuenta estado)
        {
            switch (estado)
            {
                case EstadoCuenta.CONECTANDO:
                    return "Conectando";
                case EstadoCuenta.DESCONECTADO:
                    return "Desconectado";
                case EstadoCuenta.INTERCAMBIO:
                    return "Intercambiando";
                case EstadoCuenta.LUCHANDO:
                    return "Combate";
                case EstadoCuenta.RECOLECTANDO:
                    return "Recolectando";
                case EstadoCuenta.MOVIMIENTO:
                    return "Desplazando";
                case EstadoCuenta.CONECTADO_INACTIVO:
                    return "Inactivo";
                case EstadoCuenta.ALMACENAMIENTO:
                    return "Almacenamiento";
                case EstadoCuenta.DIALOGANDO:
                    return "Dialogando";
                case EstadoCuenta.COMPRANDO:
                    return "Comprando";
                case EstadoCuenta.VENDIENDO:
                    return "Vendiendo";
                case EstadoCuenta.REGENERANDO:
                    return "Regenerando Vida";
                default:
                    return "-";
            }
        }

        public static T get_Or<T>(this Table table, string key, DataType type, T orValue)
        {
            DynValue bandera = table.Get(key);

            if (bandera.IsNil() || bandera.Type != type)
                return orValue;

            try
            {
                return (T)bandera.ToObject(typeof(T));
            }
            catch
            {
                return orValue;
            }
        }
    }
}

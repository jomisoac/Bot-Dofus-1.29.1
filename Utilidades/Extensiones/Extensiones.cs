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
                    return "Connecté";
                case EstadoCuenta.DESCONECTADO:
                    return "Deconnecté";
                case EstadoCuenta.INTERCAMBIO:
                    return "Echange";
                case EstadoCuenta.LUCHANDO:
                    return "Combat";
                case EstadoCuenta.RECOLECTANDO:
                    return "Recolte";
                case EstadoCuenta.MOVIMIENTO:
                    return "Deplacement";
                case EstadoCuenta.CONECTADO_INACTIVO:
                    return "Inactif";
                case EstadoCuenta.ALMACENAMIENTO:
                    return "Stockage";
                case EstadoCuenta.DIALOGANDO:
                    return "Dialog";
                case EstadoCuenta.COMPRANDO:
                    return "Achat";
                case EstadoCuenta.VENDIENDO:
                    return "Vente";
                case EstadoCuenta.REGENERANDO:
                    return "Regeneration";
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

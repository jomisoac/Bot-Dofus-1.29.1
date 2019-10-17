using Bot_Dofus_1._29._1.Otros.Enums;
using MoonSharp.Interpreter;

namespace Bot_Dofus_1._29._1.Utilities.Extensions
{
    public static class Extensions
    {
        public static string cadena_Amigable(this AccountStates accountStatus)
        {
            switch (accountStatus)
            {
                case AccountStates.CONNECTED:
                    return "Connecté";
                case AccountStates.DISCONNECTED:
                    return "Deconnecté";
                case AccountStates.EXCHANGE:
                    return "Echange";
                case AccountStates.FIGHTING:
                    return "Combat";
                case AccountStates.GATHERING:
                    return "Recolte";
                case AccountStates.MOVING:
                    return "Deplacement";
                case AccountStates.CONNECTED_INACTIVE:
                    return "Inactif";
                case AccountStates.STORAGE:
                    return "Stockage";
                case AccountStates.DIALOG:
                    return "Dialogue";
                case AccountStates.BUYING:
                    return "Achat";
                case AccountStates.SELLING:
                    return "Vente";
                case AccountStates.REGENERATION:
                    return "Regeneration";
                default:
                    return "-";
            }
        }

        public static T get_Or<T>(this Table table, string key, DataType type, T orValue)
        {
            DynValue flag = table.Get(key);

            if (flag.IsNil() || flag.Type != type)
                return orValue;

            try
            {
                return (T)flag.ToObject(typeof(T));
            }
            catch
            {
                return orValue;
            }
        }
    }
}

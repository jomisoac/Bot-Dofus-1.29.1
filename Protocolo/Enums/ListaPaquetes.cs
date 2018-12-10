using System.ComponentModel;

namespace Bot_Dofus_1._29._1.Protocolo.Enums
{
    public enum ListaPaquetes
    {
        [Description("HC")] BIENVENIDA_SERVIDOR,
        [Description("AlEv")] ERROR_DOFUS_VERSION,
        [Description("AlEf")] ERROR_CONEXION_CUENTA,
        [Description("AlEb")] CUENTA_BANEADA,
        [Description("AlEk")] CUENTA_TIEMPO_BANEADA,
        [Description("AlEa")] CUENTA_YA_CONECTADA,
        [Description("Ad")] CUENTA_APODO_DOFUS,
        [Description("Ac")] CUENTA_COMUNIDAD,
        [Description("AH")] HOSTS_SERVIDORES,
        [Description("AlK")] CUENTA_ES_ADMINISTRADOR,
        [Description("AQ")] CUENTA_PREGUNTA_SECRETA,
        [Description("Af")] FILA_DE_ESPERA,
        [Description("Ax")] LISTA_SERVIDORES,
        [Description("AxK")] CUENTA_ABONO_TIEMPO,
        [Description("AF")] AMIGOS_LISTA_SERVIDORES_DISPONIBLES,
        [Description("AX")] SERVIDOR_SELECCIONADO,
        [Description("AYK")] CONECTANDO_SERVIDOR_JUEGO,
        [Description("AXEd")] SERVIDOR_NO_DISPONIBLE

    }
}

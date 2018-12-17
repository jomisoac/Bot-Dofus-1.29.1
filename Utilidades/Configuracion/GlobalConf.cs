using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    internal class GlobalConf
    {
        private static List<CuentaConf> lista_cuentas;
        private static readonly string ruta_archivo_cuentas = Path.Combine(Directory.GetCurrentDirectory(), "cuentas.bot");
        public static bool mostrar_mensajes_debug { get; set; }
        public static bool modo_ultra_perfomance = false;
        public static string ip_conexion = "34.251.172.139";

        static GlobalConf()
        {
            lista_cuentas = new List<CuentaConf>();
            mostrar_mensajes_debug = true;
            modo_ultra_perfomance = false;
        }

        public static void cargar_Todas_Cuentas()
        {
            if (File.Exists(ruta_archivo_cuentas))
            {
                lista_cuentas.Clear();
                using (BinaryReader br = new BinaryReader(File.Open(ruta_archivo_cuentas, FileMode.Open)))
                {
                    int registros_totales = br.ReadInt32();
                    for (int i = 0; i < registros_totales; i++)
                    {
                        lista_cuentas.Add(CuentaConf.cargar_Una_Cuenta(br));
                    }
                    mostrar_mensajes_debug = br.ReadBoolean();
                    modo_ultra_perfomance = br.ReadBoolean();
                    //Otros
                    ip_conexion = br.ReadString();
                }
            }
            else
            {
                return;
            }
        }

        public static void guardar_Configuracion()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(ruta_archivo_cuentas, FileMode.Create)))
            {
                bw.Write(lista_cuentas.Count);
                lista_cuentas.ForEach(a => a.guardar_Cuenta(bw));
                bw.Write(mostrar_mensajes_debug);
                bw.Write(modo_ultra_perfomance);
                //Otros
                bw.Write(ip_conexion);
            }
        }

        public static void agregar_Cuenta(string nombre_cuenta, string password, string servidor, string nombre_personaje)
        {
            lista_cuentas.Add(new CuentaConf(nombre_cuenta, password, servidor, nombre_personaje));
        }

        public static void eliminar_Cuenta(int cuenta_index)
        {
            lista_cuentas.RemoveAt(cuenta_index);
        }

        public static CuentaConf get_Cuenta(string nombre_cuenta)
        {
            return lista_cuentas.FirstOrDefault(cuenta => cuenta.get_Nombre_Cuenta() == nombre_cuenta);
        }

        public static List<CuentaConf> get_Lista_Cuentas()
        {
            return lista_cuentas;
        }
    }
}

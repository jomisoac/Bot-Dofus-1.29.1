using System.Collections.Generic;
using System.IO;

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    class GlobalConfiguracion
    {
        public static List<CuentaConfiguracion> lista_cuentas { get; private set; }
        private static string ruta_archivo_cuentas = Path.Combine(Directory.GetCurrentDirectory(), "cuentas.botaidemu");

        static GlobalConfiguracion()
        {
            lista_cuentas = new List<CuentaConfiguracion>();
        }

        public static void cargar_Todas_Cuentas()
        {
            if (File.Exists(ruta_archivo_cuentas))
            {
                lista_cuentas.Clear();
                using (BinaryReader br = new BinaryReader(File.Open(ruta_archivo_cuentas, FileMode.Open)))
                {
                    int c = br.ReadInt32();
                    for (int i = 0; i < c; i++)
                    {
                        lista_cuentas.Add(CuentaConfiguracion.cargar_Cuenta(br));
                    }
                }
            }
            else
            {
                return;
            }
        }

        public static void guardar_Cuenta()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(ruta_archivo_cuentas, FileMode.Create)))
            {
                bw.Write(lista_cuentas.Count);
                lista_cuentas.ForEach(a => a.guardar_Cuenta(bw));
            }
        }

        public static void agregar_Cuenta_y_Guardar(string nombre_cuenta, string password, int servidor, string nombre_personaje)
        {
            CuentaConfiguracion ac = new CuentaConfiguracion(nombre_cuenta, password, servidor, nombre_personaje);
            lista_cuentas.Add(ac);
            guardar_Cuenta();
        }
    }
}

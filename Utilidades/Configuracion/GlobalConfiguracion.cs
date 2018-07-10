using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bot_Dofus_1._29._1.Utilidades.Configuracion
{
    class GlobalConfiguracion
    {
        private static List<CuentaConfiguracion> lista_cuentas;
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
                    int registros_totales = br.ReadInt32();
                    for (int i = 0; i < registros_totales; i++)
                    {
                        lista_cuentas.Add(CuentaConfiguracion.cargar_una_Cuenta(br));
                    }
                }
            }
            else
            {
                return;
            }
        }

        public static void guardar_Todas_Cuentas()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(ruta_archivo_cuentas, FileMode.Create)))
            {
                bw.Write(lista_cuentas.Count);
                lista_cuentas.ForEach(a => a.guardar_Cuenta(bw));
            }
        }

        public static void agregar_Cuenta(string nombre_cuenta, string password, string servidor, string nombre_personaje)
        {
            lista_cuentas.Add(new CuentaConfiguracion(nombre_cuenta, password, servidor, nombre_personaje));
        }

        public static void eliminar_Cuenta(int cuenta_index)
        {
            lista_cuentas.RemoveAt(cuenta_index);
        }

        public static CuentaConfiguracion get_Cuenta(string nombre_cuenta)
        {
            return lista_cuentas.FirstOrDefault(cuenta => cuenta.get_Nombre_cuenta() == nombre_cuenta);
        }

        public static List<CuentaConfiguracion> get_Lista_Cuentas()
        {
            return lista_cuentas;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                    for (int i = 0; i < br.ReadInt32(); i++)
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

        public static void guardar_Cuenta()
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(ruta_archivo_cuentas, FileMode.Create)))
            {
                bw.Write(lista_cuentas.Count);
                lista_cuentas.ForEach(a => a.guardar_Cuenta(bw));
            }
        }

        public static void agregar_Cuenta_Guardar(string nombre_cuenta, string password, string servidor, string nombre_personaje)
        {
            lista_cuentas.Add(new CuentaConfiguracion(nombre_cuenta, password, servidor, nombre_personaje));
            guardar_Cuenta();
        }

        public static void eliminar_Cuenta_Guardar(int cuenta_index)
        {
            lista_cuentas.RemoveAt(cuenta_index);
            guardar_Cuenta();
        }

        public static CuentaConfiguracion get_Cuenta(string nombre_cuenta)
        {
            return lista_cuentas.FirstOrDefault(cuenta => cuenta.nombre_cuenta == nombre_cuenta);
        }
    }
}

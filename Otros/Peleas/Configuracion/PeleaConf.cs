using System;
using System.Collections.Generic;
using System.IO;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class PeleaConf : IDisposable
    {
        private const string configuracion_ruta = @"Peleas/";
        private Cuenta cuenta;

        private string configuracion_archivo_ruta => Path.Combine(configuracion_ruta, $"{cuenta.cuenta_configuracion.nombre_cuenta}_{cuenta.personaje.nombre_personaje}.config");
        public List<HechizoPelea> hechizos { get; private set; }

        public PeleaConf(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            hechizos = new List<HechizoPelea>();
        }

        public void guardar()
        {
            Directory.CreateDirectory(configuracion_ruta);

            using (BinaryWriter bw = new BinaryWriter(File.Open(configuracion_archivo_ruta, FileMode.Create)))
            {
                bw.Write((byte)hechizos.Count);
                for (int i = 0; i < hechizos.Count; i++)
                    hechizos[i].guardar(bw);
            }
        }

        public void cargar()
        {
            if (File.Exists(configuracion_archivo_ruta))
            {
                using (BinaryReader br = new BinaryReader(File.Open(configuracion_archivo_ruta, FileMode.Open)))
                {
                    hechizos.Clear();
                    byte c = br.ReadByte();
                    for (int i = 0; i < c; i++)
                        hechizos.Add(HechizoPelea.cargar(br));
                }
            }
        }

        #region Zona Dispose
        ~PeleaConf() => Dispose(false);
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            cuenta = null;
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class PeleaConf : IDisposable
    {
        private const string configuracion_ruta = @"peleas/";
        private Cuenta cuenta;
        private bool disposed;

        private string configuracion_archivo_ruta => Path.Combine(configuracion_ruta, $"{cuenta.cuenta_configuracion.nombre_cuenta}_{cuenta.personaje.nombre_personaje}.config");
        public List<HechizoPelea> hechizos { get; private set; }
        public byte celdas_maximas { get; set; }
        public bool desactivar_espectador { get; set; }
        public bool utilizar_dragopavo { get; set; }
        public Tactica tactica { get; set; }
        public PosicionamientoInicioPelea posicionamiento { get; set; }

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
                bw.Write((byte)tactica);
                bw.Write((byte)posicionamiento);
                bw.Write(celdas_maximas);
                bw.Write(desactivar_espectador);
                bw.Write(utilizar_dragopavo);
                bw.Write((byte)hechizos.Count);
                for (int i = 0; i < hechizos.Count; i++)
                    hechizos[i].guardar(bw);
            }
        }

        public void cargar()
        {
            if (!File.Exists(configuracion_archivo_ruta))
            {
                get_Perfil_Defecto();
                return;
            }

            using (BinaryReader br = new BinaryReader(File.Open(configuracion_archivo_ruta, FileMode.Open)))
            {
                tactica = (Tactica)br.ReadByte();
                posicionamiento = (PosicionamientoInicioPelea)br.ReadByte();
                celdas_maximas = br.ReadByte();
                desactivar_espectador = br.ReadBoolean();
                utilizar_dragopavo = br.ReadBoolean();

                hechizos.Clear();
                byte c = br.ReadByte();
                for (int i = 0; i < c; i++)
                    hechizos.Add(HechizoPelea.cargar(br));
            }
        }

        private void get_Perfil_Defecto()
        {
            celdas_maximas = 12;
            desactivar_espectador = false;
            utilizar_dragopavo = false;
            tactica = Tactica.AGRESIVA;
            posicionamiento = PosicionamientoInicioPelea.CERCA_DE_ENEMIGOS;
        }

        #region Zona Dispose
        ~PeleaConf() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                hechizos.Clear();
                hechizos = null;
                cuenta = null;
                disposed = true;
            }
        }
        #endregion
    }
}

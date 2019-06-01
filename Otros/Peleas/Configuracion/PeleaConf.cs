using System;
using System.Collections.Generic;
using System.IO;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class PeleaConf : IDisposable
    {
        private const string carpeta_configuracion = @"peleas/";
        private Cuenta cuenta;
        private bool disposed;

        private string archivo_configuracion => Path.Combine(carpeta_configuracion, $"{cuenta.juego.personaje.nombre_personaje}.config");
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
            Directory.CreateDirectory(carpeta_configuracion);

            using (BinaryWriter bw = new BinaryWriter(File.Open(archivo_configuracion, FileMode.Create)))
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
            if (!File.Exists(archivo_configuracion))
            {
                get_Perfil_Defecto();
                return;
            }
            
            using (BinaryReader br = new BinaryReader(File.Open(archivo_configuracion, FileMode.Open)))
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
        public void Dispose() => Dispose(true);
        ~PeleaConf() => Dispose(false);
        
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

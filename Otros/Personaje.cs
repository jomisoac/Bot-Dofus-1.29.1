using System;

namespace Bot_Dofus_1._29._1.Otros
{
    public class Personaje : IDisposable
    {
        public int id { get; set; } = 0;
        public string nombre_personaje { get; set; } = string.Empty;
        public byte nivel { get; set; } = 0;
        public int gremio { get; set; } = 0;
        public byte sexo { get; set; } = 0;
        public int gfxID { get; set; } = 0;
        public int color1 { get; set; } = 0;
        public int color2 { get; set; } = 0;
        public int color3 { get; set; } = 0;
        public string objetos { get; set; }

        public event Action personaje_seleccionado;

        public Personaje(int _id, string _nombre_personaje, byte _nivel, int _gremio, byte _sexo, int _gfxID, int _color1, int _color2, int _color3, string _objetos)
        {
            id = _id;
            nombre_personaje = _nombre_personaje;
            nivel = _nivel;
            gremio = _gremio;
            sexo = _sexo;
            gfxID = _gfxID;
            color1 = _color1;
            color2 = _color2;
            color3 = _color3;
            objetos = _objetos;
        }

        public void evento_Personaje_Seleccionado()
        {
            personaje_seleccionado?.Invoke();
        }
        
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}

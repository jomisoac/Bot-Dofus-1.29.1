using System.IO;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class HechizoPelea
    {
        public short id { get; private set; } = 0;
        public string nombre { get; private set; } = null;
        public HechizoFocus focus { get; private set; }
        public byte lanzamientos_x_turno { get; set; }
        public byte lanzamientos_restantes { get; set; }
        public MetodoLanzamiento metodo_lanzamiento { get; private set; }

        public HechizoPelea(short _id, string _nombre, HechizoFocus _focus, MetodoLanzamiento _metodo_lanzamiento, byte _lanzamientos_x_turno)
        {
            id = _id;
            nombre = _nombre;
            focus = _focus;
            metodo_lanzamiento = _metodo_lanzamiento;
            lanzamientos_restantes = _lanzamientos_x_turno;
            lanzamientos_x_turno = _lanzamientos_x_turno;
        }

        public void guardar(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(nombre);
            bw.Write((byte)focus);
            bw.Write((byte)metodo_lanzamiento);
            bw.Write(lanzamientos_x_turno);
        }
        
        public static HechizoPelea cargar(BinaryReader br) => new HechizoPelea(br.ReadInt16(), br.ReadString(), (HechizoFocus)br.ReadByte(), (MetodoLanzamiento)br.ReadByte(), br.ReadByte());
    }
}

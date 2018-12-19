using System.IO;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class HechizoPelea
    {
        public int id { get; private set; }
        public string nombre { get; private set; }
        public HechizoFocus focus { get; private set; }

        public HechizoPelea(int _id, string _nombre, HechizoFocus _focus)
        {
            id = _id;
            nombre = _nombre;
            focus = _focus;
        }

        public void guardar(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(nombre);
            bw.Write((byte)focus);
        }

        public static HechizoPelea cargar(BinaryReader br) => new HechizoPelea(br.ReadInt32(), br.ReadString(), (HechizoFocus)br.ReadByte());
    }
}

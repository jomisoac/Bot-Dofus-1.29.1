using System.IO;
using Bot_Dofus_1._29._1.Otros.Peleas.Enums;

namespace Bot_Dofus_1._29._1.Otros.Peleas.Configuracion
{
    public class HechizoPelea
    {
        public int id { get; private set; } = 0;
        public string nombre { get; private set; } = null;
        public HechizoFocus focus { get; private set; }
        public byte lanzamientos_x_turno { get; set; } = 0;
        public byte lanzamientos_restantes { get; set; } = 0;
        public bool lanzar_cuerpo_cuerpo { get; private set; } = false;

        public HechizoPelea(int _id, string _nombre, HechizoFocus _focus, bool _lanzar_cuerpo_cuerpo, byte _lanzamientos_x_turno)
        {
            id = _id;
            nombre = _nombre;
            focus = _focus;
            lanzar_cuerpo_cuerpo = _lanzar_cuerpo_cuerpo;
            lanzamientos_restantes = _lanzamientos_x_turno;
            lanzamientos_x_turno = _lanzamientos_x_turno;
        }

        public void guardar(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(nombre);
            bw.Write((byte)focus);
            bw.Write(lanzar_cuerpo_cuerpo);
            bw.Write(lanzamientos_x_turno);
        }

        public static HechizoPelea cargar(BinaryReader br) => new HechizoPelea(br.ReadInt32(), br.ReadString(), (HechizoFocus)br.ReadByte(), br.ReadBoolean(), br.ReadByte());
    }
}

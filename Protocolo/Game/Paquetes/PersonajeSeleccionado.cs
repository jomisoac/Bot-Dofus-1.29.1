using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes;

namespace Bot_Dofus_1._29._1.Protocolo.Game.Paquetes
{
    class PersonajeSeleccionado : Mensajes
    {
        public PersonajeSeleccionado(string paquete, Cuenta cuenta)
        {
            string[] _loc4 = paquete.Split('|');
            cuenta.personaje = new Personaje(int.Parse(_loc4[0]), _loc4[1], byte.Parse(_loc4[2]), int.Parse(_loc4[3]), byte.Parse(_loc4[4]), int.Parse(_loc4[5]), int.Parse(_loc4[6]), int.Parse(_loc4[7]), int.Parse(_loc4[8]), _loc4[9]);
        }

        public string get_Mensaje()
        {
            return "GC1";
        }
    }
}

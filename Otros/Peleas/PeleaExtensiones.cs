using Bot_Dofus_1._29._1.Otros.Peleas.Configuracion;

namespace Bot_Dofus_1._29._1.Otros.Peleas
{
    public class PeleaExtensiones
    {
        public PeleaConf configuracion { get; set; }
        private readonly Cuenta cuenta;

        public PeleaExtensiones(Cuenta _cuenta)
        {
            cuenta = _cuenta;
            configuracion = new PeleaConf(cuenta);
        }
    }
}

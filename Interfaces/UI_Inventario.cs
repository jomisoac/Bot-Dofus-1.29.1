using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Otros.Entidades.Personajes.Inventario;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Inventario : UserControl
    {
        private Cuenta cuenta = null;

        public UI_Inventario(Cuenta _cuenta)
        {
            InitializeComponent();
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, dataGridView_equipamientos, new object[] { true });

            cuenta = _cuenta;
            cuenta.personaje.inventario.inventario_actualizado += actualizar_Inventario;
        }

        private void actualizar_Inventario(bool objetos_actualizados)
        {
            if (!objetos_actualizados)
                return;
            
            Task.Run(() =>
            {
                if (!IsHandleCreated)
                    return;
                
                BeginInvoke((Action)(() =>
                {
                    dataGridView_equipamientos.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.personaje.inventario.equipamiento)
                        dataGridView_equipamientos.Rows.Add(new object[] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad, "", "", "Eliminar" });

                    dataGridView_recursos.Rows.Clear();
                    foreach (ObjetosInventario obj in cuenta.personaje.inventario.recursos)
                        dataGridView_recursos.Rows.Add(new object[] { obj.id_inventario, obj.id_modelo, obj.nombre, obj.cantidad, "Eliminar" });
                }));
            });
        }

        private void dataGridView_equipamientos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 5)
                return;

            ObjetosInventario objeto = cuenta.personaje.inventario.equipamiento.ElementAt(e.RowIndex);
            string accion = dataGridView_equipamientos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            switch (accion)
            {
                case "Eliminar":
                    cuenta.personaje.inventario.eliminar_Objetos(objeto, 1);
                break;
            }
        }

        private void dataGridView_recursos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 4)
                return;

            ObjetosInventario objeto = cuenta.personaje.inventario.recursos.ElementAt(e.RowIndex);
            string accion = dataGridView_recursos.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

            string mensaje = Interaction.InputBox($"Ingresa la cantidad para {accion.ToLower()} el objeto {objeto.nombre} (0 = toda la cantidad):", accion, "1");
            if (!int.TryParse(mensaje, out int cantidad))
                return;

            switch (accion)
            {
                case "Eliminar":
                    cuenta.personaje.inventario.eliminar_Objetos(objeto, cantidad);
                break;
            }
        }
    }
}

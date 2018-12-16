using System;
using System.Drawing;
using System.Windows.Forms;
using Bot_Dofus_1._29._1.Forms;
using Bot_Dofus_1._29._1.LibreriaSockets;
using Bot_Dofus_1._29._1.Otros;
using Bot_Dofus_1._29._1.Protocolo.Enums;
using Bot_Dofus_1._29._1.Protocolo.Extensiones;
using Bot_Dofus_1._29._1.Utilidades.Configuracion;
using Bot_Dofus_1._29._1.Utilidades.Logs;

namespace Bot_Dofus_1._29._1.Interfaces
{
    public partial class UI_Principal : UserControl
    {
        private CuentaConf configuracion_cuenta;
        private Cuenta cuenta;

        public UI_Principal(CuentaConf _configuracion_cuenta)
        {
            InitializeComponent();
            configuracion_cuenta = _configuracion_cuenta;
            desconectarOconectarToolStripMenuItem.Text = "Conectar";
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nombre_cuenta = configuracion_cuenta.get_Nombre_Cuenta();
            if (Principal.get_Paginas_Cuentas_Cargadas().ContainsKey(nombre_cuenta))
            {
                desconectar_Cuenta();
                Principal.get_Paginas_Cuentas_Cargadas()[nombre_cuenta].contenido.Dispose();
                Principal.get_Paginas_Cuentas_Cargadas().Remove(nombre_cuenta);
            }
        }

        private void cambiar_Tab_Imagen(Image image)
        {
            if (Principal.paginas_cuentas_cargadas.ContainsKey(configuracion_cuenta.get_Nombre_Cuenta()))
            {
                Principal.paginas_cuentas_cargadas[configuracion_cuenta.get_Nombre_Cuenta()].cabezera.propiedad_Imagen = image;
            }
        }

        private void button_limpiar_consola_Click(object sender, EventArgs e) => textbox_logs.Clear();

        private void desconectarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (desconectarOconectarToolStripMenuItem.Text.Equals("Conectar"))
            {
                if (cuenta == null)
                {
                    cuenta = new Cuenta(configuracion_cuenta.get_Nombre_Cuenta(), configuracion_cuenta.get_Password(), configuracion_cuenta.get_Servidor_Id());
                    while (tabControl_principal.TabPages.Count > 2)
                    {
                        tabControl_principal.TabPages.RemoveAt(2);
                    }
                    cargar_Eventos_Login();
                    cuenta.evento_fase_socket += cargar_Eventos_Debugger;
                    desconectarOconectarToolStripMenuItem.Text = "Desconectar";
                }
            }
            else if (desconectarOconectarToolStripMenuItem.Text.Equals("Desconectar"))
            {
                if (cuenta.Fase_Socket != EstadoSocket.CAMBIANDO_A_JUEGO)
                {
                    desconectar_Cuenta();
                    desconectarOconectarToolStripMenuItem.Text = "Conectar";
                }
            }
        }

        private void desconectar_Cuenta()
        {
            if (cuenta != null)
            {
                if (cuenta.conexion != null)
                {
                    cuenta.conexion.evento_paquete_recibido -= debugger.paquete_Recibido;
                    cuenta.conexion.evento_paquete_enviado -= debugger.paquete_Enviado;
                    cuenta.conexion.evento_socket_informacion -= escribir_mensaje;
                    cuenta.conexion.evento_socket_desconectado -= escribir_mensaje;
                }

                cuenta.Dispose();
                cuenta = null;
                activar_Todos_Controles_Chat(false);

                for (int i = 2; i < tabControl_principal.TabPages.Count; i++)
                {
                    tabControl_principal.TabPages[i].Enabled = false;
                }
            }
        }

        private void cargar_Eventos_Debugger(ClienteProtocolo socket)
        {
            switch (cuenta.Fase_Socket)
            {
                case EstadoSocket.CAMBIANDO_A_JUEGO:
                    socket.evento_paquete_recibido += debugger.paquete_Recibido;
                    socket.evento_paquete_enviado += debugger.paquete_Enviado;
                    socket.evento_socket_informacion += escribir_mensaje;
                    socket.evento_socket_desconectado += escribir_mensaje;
                break;

                case EstadoSocket.JUEGO:
                    agregar_Tab_Pagina("Personaje", new UI_Personaje(cuenta), 2);
                    agregar_Tab_Pagina("Mapa", new UI_Mapa(cuenta), 4);
                    activar_Todos_Controles_Chat(true);
                    cuenta.personaje.socket_canal_personaje += socket_Evento_Chat;
                    cuenta.personaje.caracteristicas_actualizadas += personaje_Caracteristicas_Actualizadas;
                break;
            }
        }

        private void personaje_Caracteristicas_Actualizadas()
        {
            BeginInvoke((Action)(() =>
            {
                progresBar_vitalidad.Valor = cuenta.personaje.caracteristicas.vitalidad_actual;
                progresBar_vitalidad.valor_Maximo = cuenta.personaje.caracteristicas.vitalidad_maxima;
                progresBar_energia.Valor = cuenta.personaje.caracteristicas.energia_actual;
                progresBar_energia.valor_Maximo = cuenta.personaje.caracteristicas.maxima_energia;
                progresBar_experiencia.Text = cuenta.personaje.nivel.ToString();
                progresBar_experiencia.Valor = cuenta.personaje.porcentaje_experiencia;
                label_kamas_principal.Text = cuenta.personaje.caracteristicas.kamas.ToString();
            }));
        }

        private void activar_Todos_Controles_Chat(bool estado_botones)
        {
            BeginInvoke((Action)(() =>
            {
                canal_informaciones.Enabled = estado_botones;
                canal_general.Enabled = estado_botones;
                canal_privado.Enabled = estado_botones;
                canal_gremio.Enabled = estado_botones;
                canal_alineamiento.Enabled = estado_botones;
                canal_reclutamiento.Enabled = estado_botones;
                canal_comercio.Enabled = estado_botones;
                canal_incarnam.Enabled = estado_botones;
                textBox_enviar_consola.Enabled = estado_botones;
                cargarScriptToolStripMenuItem.Enabled = estado_botones;
            }));
        }

        private void cargar_Eventos_Login()
        {
            if (cuenta != null)
            {
                cuenta.conexion.evento_paquete_recibido += debugger.paquete_Recibido;
                cuenta.conexion.evento_paquete_enviado += debugger.paquete_Enviado;
                cuenta.conexion.evento_socket_informacion += escribir_mensaje;
                cuenta.conexion.evento_socket_desconectado += escribir_mensaje;

                cuenta.evento_estado_cuenta += eventos_Estados_Cuenta;
                cuenta.logger.log_evento += (mensaje, color) => escribir_mensaje(mensaje.ToString(), color);
            }
        }

        private void eventos_Estados_Cuenta()
        {
            switch (cuenta.Estado_Cuenta)
            {
                case EstadoCuenta.DESCONECTADO:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_rojo);
                break;

                case EstadoCuenta.CONECTANDO:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_naranja);
                break;

                default:
                    cambiar_Tab_Imagen(Properties.Resources.circulo_verde);
                break;
            }
            if (cuenta != null && Principal.paginas_cuentas_cargadas.ContainsKey(configuracion_cuenta.get_Nombre_Cuenta()))
            {
                Principal.paginas_cuentas_cargadas[configuracion_cuenta.get_Nombre_Cuenta()].cabezera.propiedad_Estado = cuenta.Estado_Cuenta.cadena_Amigable();
            }
        }

        private void agregar_Tab_Pagina(string nombre, UserControl control, int imagen_index)
        {
            tabControl_principal.BeginInvoke((Action)(() =>
            {
                control.Dock = DockStyle.Fill;
                var nueva_pagina = new TabPage(nombre);
                nueva_pagina.ImageIndex = imagen_index;
                nueva_pagina.Controls.Add(control);
                tabControl_principal.TabPages.Add(nueva_pagina);
            }));
        }

        private void escribir_mensaje(object error) => escribir_mensaje(DateTime.Now.ToString("HH:mm:ss") + " -> [Conexión] " + error, LogTipos.PELIGRO.ToString("X"));

        private void escribir_mensaje(string mensaje, string color)
        {
            if (!IsHandleCreated)
                return;

            textbox_logs.BeginInvoke((Action)(() =>
            {
                textbox_logs.Select(textbox_logs.TextLength, 0);
                textbox_logs.SelectionColor = ColorTranslator.FromHtml("#" + color);
                textbox_logs.AppendText(mensaje + Environment.NewLine);
                textbox_logs.ScrollToCaret();
            }));
        }

        private void socket_Evento_Chat()
        {
            BeginInvoke((Action)(() =>
            {
                canal_informaciones.Checked = cuenta.personaje.canales.Contains("i");
                canal_general.Checked = cuenta.personaje.canales.Contains("*");
                canal_privado.Checked = cuenta.personaje.canales.Contains("#$p");
                canal_gremio.Checked = cuenta.personaje.canales.Contains("%");
                canal_alineamiento.Checked = cuenta.personaje.canales.Contains("!");
                canal_reclutamiento.Checked = cuenta.personaje.canales.Contains("?");
                canal_comercio.Checked = cuenta.personaje.canales.Contains(":");
                canal_incarnam.Checked = cuenta.personaje.canales.Contains("^");
            }));
        }

        private void canal_CheckedChanged(object sender, EventArgs e)
        {
            if (cuenta.personaje != null && cuenta.Estado_Cuenta != EstadoCuenta.CONECTANDO)
            {
                CheckBox control = sender as CheckBox;
                switch (control.Name)
                {
                    case "canal_informaciones":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+i" : "cC-i");
                    break;

                    case "canal_general":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+*" : "cC-*");
                    break;

                    case "canal_privado":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+#$p" : "cC-#$p");
                    break;

                    case "canal_gremio":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+%" : "cC-%");
                    break;

                    case "canal_alineamiento":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+!" : "cC-!");
                    break;

                    case "canal_reclutamiento":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+?" : "cC-?");
                    break;

                    case "canal_comercio":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+:" : "cC-:");
                    break;

                    case "canal_incarnam":
                        cuenta.conexion.enviar_Paquete(control.Checked ? "cC+^" : "cC-^");
                    break;
                }
            }
        }

        private void textBox_enviar_consola_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox_enviar_consola.TextLength > 0 && textBox_enviar_consola.TextLength < 255)
            {
                if (cuenta.personaje != null && cuenta.Estado_Cuenta != EstadoCuenta.CONECTANDO)
                {
                    cuenta.conexion.enviar_Paquete("BM*|" + textBox_enviar_consola.Text + "|");

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    textBox_enviar_consola.Clear();
                }
            }
        }

        private void cargarScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Title = "Selecciona el script para el bot";
                    ofd.Filter = "Extension (.lua) | *.lua";

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        Console.WriteLine(ofd.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                escribir_mensaje(DateTime.Now.ToString("HH:mm:ss") + " -> [Script] " + ex.Message, LogTipos.ERROR.ToString("X"));
            }
        }
    }
}

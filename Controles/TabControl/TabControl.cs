using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1

    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Controles.TabControl
{
    public partial class TabControl : UserControl
    {
        private int anchura_cabezera;
        private Dictionary<string, Pagina> paginas;
        private string nombre_pagina_seleccionada;

        public List<string> titulos_paginas => paginas.Keys.ToList();
        public Pagina pagina_seleccionada => nombre_pagina_seleccionada == null ? null : paginas.ContainsKey(nombre_pagina_seleccionada) ? paginas[nombre_pagina_seleccionada] : null;
        public event EventHandler pagina_cambiada;

        public TabControl()
        {
            InitializeComponent();
            anchura_cabezera = 164;
            paginas = new Dictionary<string, Pagina>();
        }
        
        public Pagina agregar_Nueva_Pagina(string titulo)
        {
            if (string.IsNullOrEmpty(titulo))
                throw new ArgumentNullException("Nombre de la cuenta vacía");

            if (paginas.ContainsKey(titulo))
                throw new InvalidOperationException("Ya existe una cuenta cargada con este nombre");

            if (panelCabezeraCuentas.Controls.Count > 0)
            {
                panelCabezeraCuentas.Controls[panelCabezeraCuentas.Controls.Count - 1].Margin = new Padding(2, 0, 2, 0);
            }

            Pagina pagina = new Pagina(titulo, anchura_cabezera);
            paginas.Add(titulo, pagina);

            pagina.cabezera.Click += (s, e) => seleccionar_Pagina((s as Cabezera).propiedad_Cuenta);
            pagina.contenido.Disposed += (s, e) => eliminar_Pagina(pagina.cabezera.propiedad_Cuenta);

            panelCabezeraCuentas.Controls.Add(pagina.cabezera);
            panelContenidoCuenta.Controls.Add(pagina.contenido);

            ajustar_Cabezera_Anchura();
            seleccionar_Pagina(titulo);
            return pagina;
        }

        public void eliminar_Pagina(string titulo)
        {
            if (paginas.ContainsKey(titulo))
            {
                Pagina pagina = paginas[titulo];

                panelCabezeraCuentas.Controls.Remove(pagina.cabezera);
                panelContenidoCuenta.Controls.Remove(pagina.contenido);

                pagina.cabezera.Dispose();
                pagina.contenido.Dispose();
                paginas.Remove(titulo);

                if (nombre_pagina_seleccionada == titulo)
                {
                    nombre_pagina_seleccionada = null;
                    if (paginas.Count > 0)
                    {
                        seleccionar_Pagina(titulos_paginas[0]);
                    }
                }
                ajustar_Cabezera_Anchura();
                GC.Collect();
            }
            else
            {
                return;
            }
        }

        private void ajustar_Cabezera_Anchura()
        {
            if (anchura_cabezera == 164 && panelCabezeraCuentas.VerticalScroll.Visible)
            {
                anchura_cabezera = 150;

                panelCabezeraCuentas.SuspendLayout();
                for (int i = 0; i < panelCabezeraCuentas.Controls.Count; i++)
                {
                    panelCabezeraCuentas.Controls[i].Size = new Size(anchura_cabezera, 40);
                }
                panelCabezeraCuentas.ResumeLayout();
            }
            else if (anchura_cabezera == 150 && !panelCabezeraCuentas.VerticalScroll.Visible)
            {
                anchura_cabezera = 164;

                panelCabezeraCuentas.SuspendLayout();
                for (int i = 0; i < panelCabezeraCuentas.Controls.Count; i++)
                {
                    panelCabezeraCuentas.Controls[i].Size = new Size(anchura_cabezera, 40);
                }
                panelCabezeraCuentas.ResumeLayout();
            }
        }

        public void seleccionar_Pagina(string title)
        {
            if (nombre_pagina_seleccionada != title)
            {
                if (paginas.ContainsKey(title))
                {
                    if (nombre_pagina_seleccionada != null && paginas.ContainsKey(nombre_pagina_seleccionada))
                    {
                        var previouslySelectedPage = paginas[nombre_pagina_seleccionada];
                        previouslySelectedPage.cabezera.propiedad_Esta_Seleccionada = false;
                        previouslySelectedPage.contenido.Visible = false;
                    }

                    nombre_pagina_seleccionada = title;
                    pagina_seleccionada.cabezera.propiedad_Esta_Seleccionada = true;
                    pagina_seleccionada.contenido.Visible = true;

                    pagina_cambiada?.Invoke(paginas[nombre_pagina_seleccionada], EventArgs.Empty);
                }
                else
                {
                    throw new InvalidOperationException("No se puede seleccionar una página que no existe.");
                }
            }
            else
            {
                return;
            }
        }
    }
}

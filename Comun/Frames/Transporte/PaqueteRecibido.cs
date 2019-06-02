using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Bot_Dofus_1._29._1.Comun.Network;

/*
    Este archivo es parte del proyecto BotDofus_1.29.1
    BotDofus_1.29.1 Copyright (C) 2019 Alvaro Prendes — Todos los derechos reservados.
    Creado por Alvaro Prendes
    web: http://www.salesprendes.com
*/

namespace Bot_Dofus_1._29._1.Comun.Frames.Transporte
{
    public class PaqueteRecibido
    {
        public List<PaqueteDatos> metodos { get; private set; }
        private bool esta_iniciado { get; set; }

        public PaqueteRecibido()
        {
            metodos = new List<PaqueteDatos>();
            esta_iniciado = false;
        }

        public void Inicializar()
        {
            if (!esta_iniciado)
            {
                Assembly assembly = typeof(Frame).GetTypeInfo().Assembly;

                foreach (MethodInfo tipo in assembly.GetTypes().SelectMany(x => x.GetMethods()).Where(m => m.GetCustomAttributes(typeof(PaqueteAtributo), false).Length > 0).ToArray())
                {
                    PaqueteAtributo atributo = tipo.GetCustomAttributes(typeof(PaqueteAtributo), true)[0] as PaqueteAtributo;
                    Type tipo_string = Type.GetType(tipo.DeclaringType.FullName);

                    object instancia = Activator.CreateInstance(tipo_string, null);
                    metodos.Add(new PaqueteDatos(instancia, atributo.paquete, tipo));
                }
                esta_iniciado = true;
            }
        }

        public void Recibir(ClienteTcp cliente, string paquete)
        {
            if (!esta_iniciado)
                Inicializar();

            PaqueteDatos metodo = metodos.Find(m => paquete.StartsWith(m.nombre_paquete));

            if (metodo != null)
                metodo.informacion.Invoke(metodo.instancia, new object[] { cliente, paquete });
        }
    }
}
using System;
using System.Collections.Generic;
using Banobras.Credito.SICREB.Data.Seguridad;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Vistas;
using System.Collections;

namespace Banobras.Credito.SICREB.Business.Repositorios
{
    public class ActividadRules
    {

        public List<Actividad> GetActividades(string fechaInicial, string fechaFinal)
        {
            return ActividadDataAccess.consultaActividad(fechaInicial, fechaFinal);
        }

        public List<ActividadDatos> GetActividadesDatos(string fechaInicial, string fechaFinal)
        {
            return ActividadDataAccess.consultaActividadDatos(fechaInicial, fechaFinal);
        }
       
        public static bool GuardarActividad(int idUsuario, string catalogo, string fechaRegistro, string hora, string status, int facultad, string datoInicial, string datoFinal)
        {
            bool resp;
            return resp = ActividadDataAccess.insertActividad(facultad, idUsuario, catalogo);
        }

        public List<Actividad> GetActividades(DateTime fechaInicial, DateTime fechaFinal)
        {
            return ActividadDataAccess.consultaActividad(fechaInicial.ToString(), fechaFinal.ToString());
        }

        public static bool GuardarActividad(int idActividad, int idUsuario, String detalle)
        {
            bool resp;
            return resp = ActividadDataAccess.insertActividad(idActividad, idUsuario, detalle);
        }

        public static bool GuardarActividad(int idActividad, int idUsuario, String detalle, String antes, String despues, String catalogo, int Tipo)
        {
            bool resp;
            return resp = ActividadDataAccess.insertActividad(idActividad, idUsuario, detalle, antes, despues, catalogo, Tipo);
        }

        public static bool GActividadCatalogo(int idActividad, int idUsuario, String detalle, String antes,
            String despues, String catalogo, int Tipo, Hashtable Old, Hashtable New)
        {

            if (New != null)
            {
                if (Old != null)
                {
                    foreach (DictionaryEntry col in Old)
                    {
                        if (col.Value.ToString() == New[col.Key].ToString()) continue;
                        if (antes == null)
                        {
                            antes += col.Key + "= " + col.Value;
                            despues += col.Key + "= " + New[col.Key];
                        }
                        else
                        {
                            antes += ", " + col.Key + "= " + col.Value;
                            despues += ", " + col.Key + "= " + New[col.Key];
                        }
                    }
                }
                else
                {
                    foreach (DictionaryEntry col in New)
                    {
                        if (despues == null)
                        {
                            despues += col.Key + "= " + New[col.Key];
                        }
                        else
                        {
                            despues += ", " + col.Key + "= " + New[col.Key];
                        }
                        antes = null;
                    }
                }
            }

            bool resp;
            return resp = ActividadDataAccess.insertActividad(idActividad, idUsuario, detalle, antes, despues, catalogo, Tipo);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Banobras.Credito.SICREB.Data.Transaccionales;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Business;
using Banobras.Credito.SICREB.Business.Repositorios;

/// <summary>
/// Descripción breve de cls_ReporteInconsistencias
/// </summary>
public class cls_ReporteInconsistencias
{
    private Banobras.Credito.SICREB.Business.Repositorios.Inconsistencias_Rules inconsistencias = null;
    private Enums.Persona _persona;
    private int _archivoId;
    private DateTime _fecha;

    public cls_ReporteInconsistencias()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public cls_ReporteInconsistencias(Enums.Persona persona)
    {
        //archivosData = new ArchivosDataAccess(persona);
        //inconsistencias = new Inconsistencias_Rules
        //_pPersona = persona;
        this._persona = persona;        
        //this._fecha = fdReporte;
    }

    public DataTable GetTablaInconsistencia()
    {
        //cls_ReporteInconsistencias clsMe = new cls_ReporteInconsistencias();

        Archivo arch = new Archivo();
        Archivos_Rules archivos = new Archivos_Rules(this._persona);
        Archivo ultimoArchivo = archivos.GetUltimoArchivo();

        DataTable dtCambios = new DataTable();
        Inconsistencias_Rules inconsistencias = new Inconsistencias_Rules(this._persona, this._archivoId, ultimoArchivo.Fecha);
        dtCambios = inconsistencias.GetInconsistencias();

        //dtCambios = clsMe.BuscaCambiosReport(dtArchivo1, dtArchivo2);
        return dtCambios;
    }
}
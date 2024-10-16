using System;

namespace Banobras.Credito.SICREB.Entities
{
    public class Dia
    {
        public int Id { get; private set; }
        public string IdententicadorDia { get; private set; }
        public string Fecha { get; private set; }
        public string FechaInhabil { get; private set; }
        public DateTime dtFecha { get; private set; }
        public DateTime dtFechaInhabil { get; private set; }

        //constructores
        public Dia(int id, string IdententicadorAlertaP, string fechaP, string fechaInhabilP)
        {
            this.Id = id;
            this.IdententicadorDia = IdententicadorAlertaP;
            this.Fecha = fechaP;
            this.FechaInhabil = fechaInhabilP;
        }

        public Dia(int id, string IdententicadorAlertaP, DateTime fechaP, DateTime fechaInhabilP)
        {
            this.Id = id;
            this.IdententicadorDia = IdententicadorAlertaP;
            this.dtFecha = fechaP;
            this.dtFechaInhabil = fechaInhabilP;
        }

    }// CLASS DIA
}//namespace

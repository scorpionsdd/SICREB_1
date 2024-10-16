namespace Banobras.Credito.SICREB.Entities
{
    public class SEPOMEX
    {
      public int Id{get;set;}
      public string DCodigo { get; set; }
      public string DAsenta { get; set; }
      public string DTipoAsenta { get; set; }
      public string DMnpio { get; set; }
      public string DEstado { get; set; }
      public string DCiudad { get; set; }
      public string DCP { get; set; }
      public string CEstado { get; set; }
      public string COficina { get; set; }
      public string CCP { get; set; }
      public string CTipoAsenta { get; set; }
      public string CMnpio { get; set; }
      public string IdAsentaCpcons { get; set; }
      public string DZona { get; set; }
      public string CCveCiudad { get; set; }


      public SEPOMEX(int id, string dcodigo, string dasenta, string dtipoasenta, string dmnpio, string destado, string dciudad, string dcp, string cestado, string coficina, string ccp, string ctipoasenta, string cmnpio, string idasentacpcons,string dzona,string ccveciudad)
      {
          this.Id = id;
          this.DCodigo = dcodigo;
          this.DAsenta = dasenta;
          this.DTipoAsenta = dtipoasenta;
          this.DMnpio = dmnpio;
          this.DEstado = destado;
          this.DCiudad = dciudad;
          this.DCP = dcp;
          this.CEstado = cestado;
          this.COficina = coficina;
          this.CCP = ccp;
          this.CTipoAsenta = ctipoasenta;
          this.CMnpio = cmnpio;
          this.IdAsentaCpcons = idasentacpcons;
          this.DZona = dzona;
          this.CCveCiudad = ccveciudad;
      }
    }
}

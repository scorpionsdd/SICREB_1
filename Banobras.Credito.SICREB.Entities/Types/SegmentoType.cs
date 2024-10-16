using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Banobras.Credito.SICREB.Entities.Types.PM;
using Banobras.Credito.SICREB.Entities.Types.PF;
using Banobras.Credito.SICREB.Entities.Util;

namespace Banobras.Credito.SICREB.Entities.Types
{
    /// <summary>
    /// Interfaz para los tipos de segmentos
    /// </summary>
    /// <typeparam name="T">Especifica el tipo de segmento del root del registro.</typeparam>
    /// <typeparam name="U">Especifica el tipo de segmento del Padre</typeparam>
    public abstract class SegmentoType<T, U> : ISegmentoTypeRoot<T, U>
        where T : ISegmentoType, new()
        where U : ISegmentoType, new()
        
    {
        
        public enum TipoDato { Texto, Numero };
        public Enums.Persona Persona { get; private set; }

        #region ISegmentoTypeRoot<T,U> Members

        private T root;
        public T MainRoot
        {
            get
            {
                if (root == null)
                {
                    root = (T)GetRoot(this);
                }
                if (root == null)
                    root = new T();
                return root;
            }
        }

        public U TypedParent
        {
            get
            {
                try
                {
                    return (U)Parent;
                }
                catch (InvalidCastException ex)
                {
                    return default(U);
                }

            }
            set
            {
                Parent = value;
            }
        }

        #endregion

        #region ISegmentoType Members

        public int Correctos { get; set; }
        
        public int AuxId { get; set; }

        private ISegmentoType parent = null;
        public ISegmentoType Parent
        {
            get 
            {
                return parent;
            }
            set
            {
                parent = value;

                if (parent != null)
                {
                    string segmento = null;
                    Type segType = this.GetType();
                    if (segType == typeof(PM_EM))
                        segmento = "EM";
                    else if (segType == typeof(PM_CR))
                        segmento = "CR";
                    else if (segType == typeof(PM_DE))
                        segmento = "DE";
                    else if (segType == typeof(PM_AC))
                        segmento = "AC";
                    else if (segType == typeof(PM_AV))
                        segmento = "AV";
                    else if (segType == typeof(PM_HD))
                        segmento = "HD";
                    else if (segType == typeof(PM_TS))
                        segmento = "TS";
                    else if (segType == typeof(PF_INTF))
                        segmento = "INTF";
                    else if (segType == typeof(PF_PA))
                        segmento = "PA";
                    else if (segType == typeof(PF_PE))
                        segmento = "PE";
                    else if (segType == typeof(PF_PN))
                        segmento = "PN";
                    else if (segType == typeof(PF_TL))
                        segmento = "TL";
                    else if (segType == typeof(PF_TR))
                        segmento = "TR";

                    if (!String.IsNullOrWhiteSpace(segmento))
                    {
                        LoadEtiquetas(segmento);
                    }
                }
            }
        }

        public List<Etiqueta> Etiquetas { get; set; }
        public List<Validacion> Validaciones { get; set; }

        private bool isValid = true;
        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
            }
        }


        #endregion

        #region Constructores

        public SegmentoType(Enums.Persona persona)
        {
            this.Persona = persona;
            
            InicializaEmpty();
        }

        public SegmentoType(Enums.Persona persona, ISegmentoType parent) 
            : this(persona)
        {
            this.Parent = parent;

        }

        #endregion

        #region Metodos

        public abstract void InicializaEmpty();

        private ISegmentoType GetRoot(ISegmentoType seg)
        {
            try
            {
                if (seg == null)
                    return null;
                if (seg.GetType() == typeof(T))
                    return seg;
                else
                {
                    
                    return GetRoot(seg.Parent);
                }
            }
            catch
            { return null; }
            
        }

        public String Str(string value, int size, TipoDato tipo)
        {
            if (value.Trim().Length > size)
                value = value.Trim().Substring(0, size);

            string v = "";
            if (!String.IsNullOrWhiteSpace(value))
                v = value.Trim();

            if (tipo == TipoDato.Texto)
                return v.PadRight(size, ' ');
            else
                return v.PadLeft(size, '0');
        }
        public void Str(StringBuilder sb, string etiqueta, object value, TipoDato tipo, bool requerido)
        {
            if (value != null)
            {
                string val = value.ToString();
                if (requerido || val.Trim() != string.Empty)
                {
                    sb.AppendFormat("{0}{1}{2}", etiqueta, val.Length, val);
                }
            }
        }

        private void LoadEtiquetas(string segmento)
        {
            ISegmentoType cinta = GetCinta();
                    
            if (cinta != null && cinta.Etiquetas != null)
            {
                if (!String.IsNullOrEmpty(segmento))
                {
                    Type type = cinta.GetType();
                    List<Segmento> segmentos = null;
                    if (type == typeof(PM_Cinta))
                        segmentos = ((PM_Cinta)cinta).Segmentos;
                    else if (type == typeof(PF_Cinta))
                        segmentos = ((PF_Cinta)cinta).Segmentos;


                    if (segmentos != null && segmentos.Count > 0)
                    {

                        int idSegmento = (from seg in segmentos
                                          where seg.Codigo.Equals(segmento, StringComparison.InvariantCultureIgnoreCase)
                                          select seg.Id).SingleOrDefault();

                        this.Etiquetas = (from etiq in cinta.Etiquetas
                                          where etiq.SegmentoId == idSegmento
                                          select etiq).ToList();
                    }
                }
            }

            LoadValidaciones();
        }
        private void LoadValidaciones()
        {
            this.Validaciones = new List<Validacion>();
            ISegmentoType cinta = GetCinta();
            if (cinta != null && cinta.Validaciones != null)
            {
                List<Validacion> validacionesCinta = cinta.Validaciones;
                foreach (Etiqueta etiq in this.Etiquetas)
                {
                    var val = (from va in validacionesCinta
                                            where va.Etiqueta_Id == etiq.Id
                                            select va).ToList();
                    if(val != null)
                    {
                        this.Validaciones.AddRange(val);
                    }
                }
            }
            
        }

        private ISegmentoType GetCinta()
        {
            ISegmentoType cinta = this;

            while (cinta != null && (cinta as PM_Cinta) == null && (cinta as PF_Cinta) == null)
            {
                cinta = cinta.Parent;
            }
            return cinta;
        }

        #endregion

    }
}

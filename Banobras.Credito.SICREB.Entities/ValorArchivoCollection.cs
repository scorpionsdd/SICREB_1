using System;
using System.Collections.Generic;
using System.Linq;
using Banobras.Credito.SICREB.Entities.Util;
using System.Text.RegularExpressions;

namespace Banobras.Credito.SICREB.Entities
{
    public class ValorArchivoCollection : ICollection<ValorArchivo>
    {
        private List<ValorArchivo> coleccion = null;

        public ValorArchivoCollection()
        {
            coleccion = new List<ValorArchivo>();
        }

        #region ICollection<ValorArchivo> Members

        public void Add(ValorArchivo item)
        {
            if (item.ArchivoId <= 0 || item.EtiquetaId <= 0)
                throw new ArgumentException();

            if (!String.IsNullOrWhiteSpace(item.Valor))
            {
                Regex reg = new Regex(@"^\d*$");
                if (reg.IsMatch(item.Valor))
                {
                    if (Parser.ToDouble(item.Valor) > 0.0)
                    {
                        coleccion.Add(item);
                    }
                }
                else
                {
                    coleccion.Add(item);
                }
            }
        }

        public void Clear()
        {
            coleccion.Clear();
        }

        public bool Contains(ValorArchivo item)
        {
            return coleccion.Contains(item);
        }

        public void CopyTo(ValorArchivo[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return coleccion.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.IsReadOnly; }
        }

        public bool Remove(ValorArchivo item)
        {
            return coleccion.Remove(item);
        }

        #endregion

        #region IEnumerable<ValorArchivo> Members

        public IEnumerator<ValorArchivo> GetEnumerator()
        {
            return coleccion.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return coleccion.AsEnumerable().GetEnumerator();
        }

        #endregion
    }
}

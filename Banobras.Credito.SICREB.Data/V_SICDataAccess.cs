using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using Banobras.Credito.SICREB.Entities;
using Banobras.Credito.SICREB.Entities.Util;
using Banobras.Credito.SICREB.Common;

//using testConectionOracle;


namespace Banobras.Credito.SICREB.Data
{

    public class V_SICDataAccess : OracleBase
    {

        private List<SICItem> allRecords = null;

        public List<SICItem> GetRegistros()
        {
            if (allRecords == null)
            {
                allRecords = new List<SICItem>();

                String query = "DATOSEXTERNOS.SP_ObtenerSIC";

                //MASS. 04octubre2021 para pruebas en desarrollo
                if (WebConfig.MailFrom.StartsWith("desarrollo"))
                {
                    query = "DATOSEXTERNOS_Z.SP_ObtenerSIC";
                }

                DbCommand cmd = DB.GetStoredProcCommand(query);

                using (IDataReader reader = DB.ExecuteReader(cmd))
                {
                    while (reader.Read())
                    {
                        SICItem item = new SICItem();

                        item.Rfc = reader["RFC"].ToString();
                        item.Credito = Parser.ToNumber(reader["CREDITO"]);
                        item.Auxiliar = Parser.ToNumber(reader["AUXILIAR"]);
                        item.Credito_Anterior = Parser.ToNumber(reader["CREDITO_ANTERIOR"]);
                        item.Numero_Cuenta = reader["NUMERO_CUENTA"].ToString();
                        item.Cuenta_Contable_Capital = reader["CUENTA_CONTABLE_CAPITAL"].ToString();
                        item.Fecha_Apertura = Parser.ToDateTime(reader["FECHA_APERTURA"]);
                        item.Fecha_Vencimiento = Parser.ToDateTime(reader["FECHA_VENCIMIENTO"]);
                        item.Tipo_Credito = Parser.ToNumber(reader["TIPO_CREDITO"]);
                        item.Saldo_Inicial = Parser.ToNumber(reader["SALDO_INICIAL"]);
                        item.Clave_Moneda = Parser.ToNumber(reader["CLAVE_MONEDA"]);
                        item.Moneda = reader["MONEDA"].ToString();
                        item.Periodicidad_Intereses = Parser.ToNumber(reader["PERIODICIDAD_INTERESES"]);
                        item.Numero_Pagos = reader["NUMERO_PAGOS"].ToString();
                        item.Frecuencia_Pagos = reader["FRECUENCIA_PAGOS"].ToString();
                        item.Importe_Pagos = Parser.ToNumber(reader["IMPORTE_PAGOS"]);
                        item.Monto_Pagar = Parser.ToNumber(reader["MONTO_PAGAR"]);
                        item.Monto_Pagar_Vencido = Parser.ToNumber(reader["MONTO_PAGAR_VENCIDO"]);
                        item.Fecha_Ultimo_Pago = Parser.ToDateTime(reader["FECHA_ULTIMO_PAGO"]);
                        item.Fecha_Ultima_Compra = Parser.ToDateTime(reader["FECHA_ULTIMA_COMPRA"]);
                        item.Fecha_Reestructura = Parser.ToDateTime(reader["FECHA_REESTRUCTURA"]);
                        item.Fecha_Cierre = Parser.ToDateTime(reader["FECHA_CIERRE"]);
                        item.Importe = Parser.ToNumber(reader["IMPORTE"]);
                        item.Pago_Efectivo = Parser.ToNumber(reader["PAGO_EFECTIVO"]);
                        item.Fecha_Liquidacion = Parser.ToDateTime(reader["FECHA_LIQUIDACION"]);
                        item.Quita = Parser.ToNumber(reader["QUITA"]);
                        item.Dacion_Pago = Parser.ToNumber(reader["DACION_PAGO"]);
                        item.Quebranto = Parser.ToNumber(reader["QUEBRANTO"]);
                        item.Dias_Vencimiento = Parser.ToNumber(reader["DIAS_VENCIMIENTO"]);
                        item.Num_Pagos_Vencidos = Parser.ToNumber(reader["NUM_PAGOS_VENCIDOS"]);
                        item.Formas_Pago = Parser.ToNumber(reader["FORMA_PAGO"]);
                        item.Fecha_Amortizacion_Vencida = Parser.ToDateTime(reader["FECHA_AMORTIZACION_VENCIDA"]);

                        allRecords.Add(item);
                    }
                }
            }

            return allRecords;
        }

        public List<SICItem> GetItems(string rfc)
        {
            var items = GetRegistros()
                            .Where(s => s.Rfc.Equals(rfc, StringComparison.InvariantCultureIgnoreCase))
                            .ToList();
            return items;
        }

    }

}

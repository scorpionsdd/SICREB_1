using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Banobras.Credito.SICREB.Data;
using System.Data.Common;
using System.Data;

/// <summary>
/// Descripción breve de cls_eliminacionMasiva
/// </summary>
public class cls_eliminacionMasiva
{

    cls_accesoData accesoData = new cls_accesoData();
    int numeroParametros=0;

	public cls_eliminacionMasiva()
	{

    }//cls_eliminacionMasiva

    public int eliminaRegistro(string storeDeletep,DbParameter[] parametroP) {
        int resp_delete=0;
        
        try {

                DbParameter[] parametro=new DbParameter[parametroP.Length];
    
                parametro[0]=    OracleBase.DB.DbProviderFactory.CreateParameter();
                parametro[0].ParameterName = parametroP[0].ParameterName; //"idnacionalidad";
                parametro[0].DbType =        parametroP[0].DbType; //DbType.Decimal;
                parametro[0].Direction =     parametroP[0].Direction; //ParameterDirection.Output;
                parametro[0].Size =          parametroP[0].Size; // 38;
                parametro[0].Value=          parametroP[0].Value; //IDRowDeletep;


    
  


                accesoData.fn_getResultadoSTORE_Command(storeDeletep, parametro);



            return resp_delete;
        }
        catch(Exception e) {

            return resp_delete=0;

        }//try-cath
      
    }        //eliminaRegistro




}//class cls_eliminacionMasiva
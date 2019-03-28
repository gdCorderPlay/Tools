using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ExcelDataReader;
using System.Data;

namespace GD
{
    /// <summary>
    /// 键入注释
    /// </summary>
    public class ReadExcel : MonoBehaviour
    {


        public string ExcelPathName;

        void Start()
        {
            GameReadExcel(ExcelPathName);
        }

        /// <summary>
        /// 只读Excel方法
        /// </summary>
        /// <param name="ExcelPath"></param>
        /// <returns></returns>
        public static void GameReadExcel(string ExcelPath)
        {
           
            FileStream stream = File.Open(Application.dataPath + ExcelPath, FileMode.Open/*, FileAccess.Read*/);
            Debug.Log(Application.dataPath + ExcelPath);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();

            int columns = result.Tables[0].Columns.Count;//获取列数
            int rows = result.Tables[0].Rows.Count;//获取行数


            //从第二行开始读
            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    string nvalue = result.Tables[0].Rows[i][j].ToString();
                    Debug.Log(nvalue);
                }
            }

        }



    }
    //=======================================================
    // 作者：GD
    // 始于11/22/2018 6:18:46 PM
    //=======================================================
}


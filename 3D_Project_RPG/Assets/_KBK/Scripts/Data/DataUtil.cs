using System;
using System.IO;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

public static class DataUtil
{
    public static DataTable GetDataTable(string fileName, string tableName)
    {
        var obj = Resources.Load(fileName);
        string value = (obj as TextAsset).ToString();
        DataTable data = JsonConvert.DeserializeObject<DataTable>(value);
        data.TableName = tableName;

        return data;
    }
    
    public static DataTable GetDataTable(FileInfo info)
    {
        string fileName = Path.GetFileNameWithoutExtension(info.Name);
        string path = string.Concat("JsonData/", fileName);
        string value = string.Empty;
        try
        {
            value = (Resources.Load(path) as TextAsset).ToString();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }

        DataTable data = JsonConvert.DeserializeObject<DataTable>(value);
        data.TableName = fileName;

        return data;
    }

    public static string GetDataValue(DataSet dataSet, string tableName, string primary,
        string value, string column)
    {
        DataRow[] rows = dataSet.Tables[tableName].Select(string.Concat(primary, " = ' ", value, "'"));

        return rows[0][column].ToString();
    }

    public static void SetObjectFile<T>(string key, T data)
    {
        string value = JsonConvert.SerializeObject(data);
        File.WriteAllText(Application.dataPath + "/Resources/JsonData/" + key + ".json", value);
    }
}

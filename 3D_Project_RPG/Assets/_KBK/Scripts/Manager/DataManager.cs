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

public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager I
    {
        get
        {
            if(_instance == null)
            {
                GameObject dataManager = new GameObject();
                dataManager.name = "DataManager";
                _instance = dataManager.AddComponent<DataManager>();
            }
            return _instance;
        }
    }

    //테이블 묶음 관리할 DataSet 변수
    private DataSet _database;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        InitDataManager();
    }

    public void InitDataManager()
    {
        _database = new DataSet("Database");
#if UNITY_EDITOR
        //에디터에서 실행시 스프레드 시트 API 호출
        MakeSheetDatset(_database);
#else
        //Android, IOS 환경에서 실행시 로컬 json파일에서 데이터 받아옴
        LoadJsonData(_database);
#endif
    }
    
    private void MakeSheetDatset(DataSet dataset)
    {
        var pass = new ClientSecrets();
        pass.ClientId = "1024523033319-olfsc86rm86gmi13h1r498ibg65o9mh0.apps.googleusercontent.com";
        pass.ClientSecret = "BBqqhmhvZhxLc9f-h6d5ZTxY";

        var scopes = new string[] { SheetsService.Scope.SpreadsheetsReadonly };
        var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(pass, scopes,
            "UnityEditor", CancellationToken.None).Result;

        var service = new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "VillageKeeper"
        });

        var request = service.Spreadsheets.Get("1yKHcZLMZsFn3oobEg9Sv6aReWN9fNHby178IhA8NKTU").Execute();

        foreach (Sheet sheet in request.Sheets)
        {
            DataTable table = SendRequest(service, sheet.Properties.Title);
            dataset.Tables.Add(table);
        }
    }

    private DataTable SendRequest(SheetsService service, string sheetName)
    {
        DataTable result = null;
        bool success = true;

        try
        {
            //!A1:M은 스프레드 시트 A열부터 M열까지 데이터 받아오겠다는 소리
            var request = service.Spreadsheets.Values.Get("1yKHcZLMZsFn3oobEg9Sv6aReWN9fNHby178IhA8NKTU", sheetName + "!A1:M");
            //API 호출로 받아온 IList 데이터
            var jsonObject = request.Execute().Values;
            //IList 데이터를 jsonConvert 하기 위해 직렬화
            string jsonString = ParseSheetData(jsonObject);

            //DataTable로 변환
            result = SpreadSheetToDataTable(jsonString);
        }
        catch (Exception e)
        {
            success = false;
            Debug.LogError(e);
            string path = string.Format("JsonData/{0}", sheetName);
            //예외 발생시 로컬 경로에 있는 json파일을 통해 데이터 가져옴
            result = DataUtil.GetDataTable(path, sheetName);
            Debug.Log("시트 로드 실패로 로컬 " + sheetName + " json 데이터 불러옴");
        }

        Debug.Log(sheetName + " 스프레드 시트 로드 " + (success ? "성공" : "실패"));

        result.TableName = sheetName;

        if (result != null)
        {
            //변환한 테이블을 json파일로 저장
            SaveDataToFile(result);
        }

        return result;

    }
    
    private DataTable SpreadSheetToDataTable(string json)
    {
        DataTable data = JsonConvert.DeserializeObject<DataTable>(json);
        return data;
    }

    private string ParseSheetData(IList<IList<object> > value)
    {
        StringBuilder jsonBuilder = new StringBuilder();

        IList<object> colums = value[0];

        jsonBuilder.Append("[");
        for (int row = 0; row < value.Count; row++)
        {
            var data = value[row];
            jsonBuilder.Append("{");
            for (int col = 0; col < data.Count; col++)
            {
                jsonBuilder.Append("\"" + colums[col] + "\"" + ":");
                jsonBuilder.Append("\"" + data[col] + "\"");
                jsonBuilder.Append(",");
            }
            jsonBuilder.Append("}");
            if (row != value.Count - 1)
                jsonBuilder.Append(",");
        }
        jsonBuilder.Append("]");
        return jsonBuilder.ToString();
    }

    private void SaveDataToFile(DataTable newTable)
    {
        //로컬 경로
        string JsonPath = string.Concat(Application.dataPath + "/Resources/JSonData/" + newTable.TableName + ".json");
        FileInfo info = new FileInfo(JsonPath);

        //동일파일 유무 체크
        if (info.Exists)
        {
            DataTable checkTable = DataUtil.GetDataTable(info);
            //파일 내용 체크
            if (BinaryCheck<DataTable>(newTable, checkTable))
            {
                return;
            }
        }
        //json 파일 저장
        DataUtil.SetObjectFile(newTable.TableName, newTable);
    }

    private bool BinaryCheck<T>(T src, T target)
    {
        //두 대상을 바이너리로 변환해서 비교, 다르면 false 반환
        BinaryFormatter formatter1 = new BinaryFormatter();
        MemoryStream stream1 = new MemoryStream();
        formatter1.Serialize(stream1, src);

        BinaryFormatter formatter2 = new BinaryFormatter();
        MemoryStream stream2 = new MemoryStream();
        formatter1.Serialize(stream2, target);

        byte[] srcByte = stream1.ToArray();
        byte[] tarByte = stream2.ToArray();

        if(srcByte.Length != tarByte.Length)
        {
            return false;
        }
        for (int i = 0; i < srcByte.Length; i++)
        {
            if (srcByte[i] != tarByte[i])
                return false;
        }
        return true;
    }

    private void LoadJsonData(DataSet dataSet)
    {
        string JsonPath = string.Concat(Application.dataPath + "/Resources/JSonData/");
        DirectoryInfo info = new DirectoryInfo(JsonPath);
        foreach (FileInfo file in info.GetFiles())
        {
            //로컬 경로에서 json 가져와서 DataTable으로 변환
            DataTable table = DataUtil.GetDataTable(file);
            dataSet.Tables.Add(table);
        }
    }

    //테이블 내 데이터 가져올 때
    public string SelectTableData(string tableName, string primary, string column)
    {
        DataRow[] rows = _database.Tables[tableName].Select(string.Concat(primary, " = '", column, "'"));

        return rows[0][column].ToString();
    }
}
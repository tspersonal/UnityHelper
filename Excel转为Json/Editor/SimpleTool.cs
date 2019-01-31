﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Data;
using System.IO;
using LitJson;
using Excel;
using System.Text;
using Newtonsoft.Json;

public class SimpleTool : MonoBehaviour
{
	[MenuItem("DevelopmentTool/SimpleTool/excel转为json文件")]
	public static void ExcelToJson()
	{
		DataTable dataTable;
		//列数
		int keyIndex = 0;
		//excel文件路径
		string excelFilePath = @"C:\Users\sun\Desktop\4DFishingLocalization.xlsx";

		if (!File.Exists(excelFilePath))
		{
			print("不存在文件：" + excelFilePath);
			return;
		}

		//文件流
		FileStream mStream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read);


		//转化为excel
		IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
		//表格数据集合
		DataSet mResultSet = mExcelReader.AsDataSet();

		if (mResultSet.Tables.Count < 1) return;
		//获取第一个数据表
		dataTable = mResultSet.Tables[0];
		for (int i = 0; i < dataTable.Columns.Count; i++)
		{
			if (dataTable.Rows[0][i].ToString().Equals("Key"))
				keyIndex = i;
		}

		//Row[][] Row[行][列]
		int count = dataTable.Rows.Count;
		TDRTGFishingLocalization tDRTGFishingLocalization = new TDRTGFishingLocalization();
		for (int i = 1; i < count; i++)
		{
			string key = dataTable.Rows[i][keyIndex].ToString();
			string ChineseSimplified = dataTable.Rows[i][keyIndex + 1].ToString();
			string ChineseTraditional = dataTable.Rows[i][keyIndex + 2].ToString();
			string English = dataTable.Rows[i][keyIndex + 3].ToString();

			Language1 language = new Language1(key, ChineseSimplified, ChineseTraditional, English);
			tDRTGFishingLocalization.Languages.Add(language);
		}

		//生成Json字符串
		string json = JsonConvert.SerializeObject(tDRTGFishingLocalization, Formatting.Indented);
		//写入文件
		using (FileStream fileStream = new FileStream(Application.dataPath + "/Resources/4DFishingLocalization.json", FileMode.Create, FileAccess.Write))
		{
			using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
			{
				textWriter.Write(json);
			}
		}
	}


}
[System.Serializable]
public class TDRTGFishingLocalization
{
	public List<Language1> Languages;
	public TDRTGFishingLocalization()
	{
		Languages = new List<Language1>();
	}
}
[System.Serializable]
public class Language1
{
	public string Key;
	public string ChineseSimplified;
	public string ChineseTraditional;
	public string English;
	public Language1(string Key, string ChineseSimplified, string ChineseTraditional, string English)
	{
		this.Key = Key;
		this.ChineseSimplified = ChineseSimplified;
		this.ChineseTraditional = ChineseTraditional;
		this.English = English;
	}
}
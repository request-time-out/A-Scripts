// Decompiled with JetBrains decompiler
// Type: AIChara.ChaRandomName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AIChara
{
  public class ChaRandomName
  {
    public List<string> lstRandLastNameH { get; set; } = new List<string>();

    public List<string> lstRandLastNameK { get; set; } = new List<string>();

    public List<string> lstRandFirstNameHF { get; set; } = new List<string>();

    public List<string> lstRandFirstNameKF { get; set; } = new List<string>();

    public List<string> lstRandFirstNameHM { get; set; } = new List<string>();

    public List<string> lstRandFirstNameKM { get; set; } = new List<string>();

    public List<string> lstRandMiddleName { get; set; } = new List<string>();

    public void Initialize()
    {
      List<ExcelData.Param> source = ChaListControl.LoadExcelData("list/characustom/namelist.unity3d", "RandNameList_Name", 2, 1);
      this.lstRandLastNameH = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[0])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandLastNameK = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[1])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandFirstNameHF = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[2])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandFirstNameKF = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[3])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandFirstNameHM = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[4])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandFirstNameKM = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[5])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
      this.lstRandMiddleName = source.Select<ExcelData.Param, string>((Func<ExcelData.Param, string>) (x => x.list[6])).Where<string>((Func<string, bool>) (x => x != "0" && x != string.Empty)).ToList<string>();
    }

    public string GetRandName(byte Sex)
    {
      StringBuilder stringBuilder = new StringBuilder(64);
      if (ChaRandomName.GetRandomIndex(5, 95) == 0)
      {
        if (ChaRandomName.GetRandomIndex(10, 90) == 0)
        {
          if (Sex == (byte) 0)
          {
            if (this.lstRandFirstNameKM.Count != 0)
              stringBuilder.Append(this.lstRandFirstNameKM[Random.Range(0, this.lstRandFirstNameKM.Count)]);
          }
          else if (this.lstRandFirstNameKF.Count != 0)
            stringBuilder.Append(this.lstRandFirstNameKF[Random.Range(0, this.lstRandFirstNameKF.Count)]);
        }
        else if (Sex == (byte) 0)
        {
          if (this.lstRandFirstNameHM.Count != 0)
            stringBuilder.Append(this.lstRandFirstNameHM[Random.Range(0, this.lstRandFirstNameKM.Count)]);
        }
        else if (this.lstRandFirstNameHF.Count != 0)
          stringBuilder.Append(this.lstRandFirstNameHF[Random.Range(0, this.lstRandFirstNameKF.Count)]);
      }
      else if (ChaRandomName.GetRandomIndex(10, 90) == 0)
      {
        if (Sex == (byte) 0)
        {
          if (this.lstRandFirstNameKM.Count != 0)
            stringBuilder.Append(this.lstRandFirstNameKM[Random.Range(0, this.lstRandFirstNameKM.Count)]);
        }
        else if (this.lstRandFirstNameKF.Count != 0)
          stringBuilder.Append(this.lstRandFirstNameKF[Random.Range(0, this.lstRandFirstNameKF.Count)]);
        stringBuilder.Append("・");
        string empty = string.Empty;
        while (this.lstRandLastNameK.Count != 0)
        {
          empty = this.lstRandLastNameK[Random.Range(0, this.lstRandLastNameK.Count)];
          if (empty.Length + stringBuilder.Length < 16)
            break;
        }
        if (string.Empty != empty && stringBuilder.Length + empty.Length < 10)
        {
          if (ChaRandomName.GetRandomIndex(10, 90) == 0)
          {
            string str = this.lstRandMiddleName[Random.Range(0, this.lstRandMiddleName.Count)];
            stringBuilder.Append(str).Append("・").Append(empty);
          }
          else
            stringBuilder.Append(empty);
        }
        else
          stringBuilder.Append(empty);
      }
      else
      {
        if (this.lstRandLastNameH.Count != 0)
          stringBuilder.Append(this.lstRandLastNameH[Random.Range(0, this.lstRandLastNameH.Count)]);
        stringBuilder.Append(" ");
        if (Sex == (byte) 0)
        {
          if (this.lstRandFirstNameHM.Count != 0)
            stringBuilder.Append(this.lstRandFirstNameHM[Random.Range(0, this.lstRandFirstNameHM.Count)]);
        }
        else if (this.lstRandFirstNameHF.Count != 0)
          stringBuilder.Append(this.lstRandFirstNameHF[Random.Range(0, this.lstRandFirstNameHF.Count)]);
      }
      return stringBuilder.Length == 0 ? string.Empty : stringBuilder.ToString();
    }

    public static int GetRandomIndex(params int[] weightTable)
    {
      int num1 = Random.Range(1, ((IEnumerable<int>) weightTable).Sum() + 1);
      int num2 = -1;
      for (int index = 0; index < weightTable.Length; ++index)
      {
        if (weightTable[index] >= num1)
        {
          num2 = index;
          break;
        }
        num1 -= weightTable[index];
      }
      return num2;
    }
  }
}

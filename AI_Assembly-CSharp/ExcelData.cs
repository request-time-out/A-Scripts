// Decompiled with JetBrains decompiler
// Type: ExcelData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class ExcelData : ScriptableObject
{
  public List<ExcelData.Param> list;

  public ExcelData()
  {
    base.\u002Ector();
  }

  public string this[int cell, int row]
  {
    get
    {
      return this.Get(cell, row);
    }
  }

  public int MaxCell
  {
    get
    {
      return this.list.Count;
    }
  }

  public string Get(ExcelData.Specify specify)
  {
    return this.Get(specify.cell, specify.row);
  }

  public string Get(int cell, int row)
  {
    string empty = string.Empty;
    if (cell < this.list.Count && row < this.list[cell].list.Count)
      empty = this.list[cell].list[row];
    return empty;
  }

  public List<string> GetCell(int row)
  {
    List<string> stringList = new List<string>();
    foreach (ExcelData.Param obj in this.list)
    {
      if (row < obj.list.Count)
        stringList.Add(obj.list[row]);
    }
    return stringList;
  }

  public List<string> GetCell(int rowStart, int rowEnd)
  {
    List<string> stringList = new List<string>();
    if ((long) (uint) rowStart > (long) rowEnd)
      return stringList;
    using (List<ExcelData.Param>.Enumerator enumerator = this.list.GetEnumerator())
    {
label_7:
      while (enumerator.MoveNext())
      {
        ExcelData.Param current = enumerator.Current;
        int index = rowStart;
        while (true)
        {
          if (index < current.list.Count && index <= rowEnd)
          {
            stringList.Add(current.list[index]);
            ++index;
          }
          else
            goto label_7;
        }
      }
    }
    return stringList;
  }

  public List<string> GetRow(int cell)
  {
    List<string> stringList = new List<string>();
    if (cell < this.list.Count)
    {
      foreach (string str in this.list[cell].list)
        stringList.Add(str);
    }
    return stringList;
  }

  public List<string> GetRow(int cellStart, int cellEnd)
  {
    List<string> stringList = new List<string>();
    if ((long) (uint) cellStart > (long) cellEnd)
      return stringList;
    for (int index = cellStart; index < this.list.Count && index <= cellEnd; ++index)
    {
      foreach (string str in this.list[index].list)
        stringList.Add(str);
    }
    return stringList;
  }

  public List<ExcelData.Param> Get(ExcelData.Specify start, ExcelData.Specify end)
  {
    List<ExcelData.Param> objList = new List<ExcelData.Param>();
    if ((long) (uint) start.cell > (long) end.cell || (long) (uint) start.row > (long) end.row || start.cell >= this.list.Count)
      return objList;
    for (int cell = start.cell; cell < this.list.Count && cell <= end.cell; ++cell)
    {
      ExcelData.Param obj = new ExcelData.Param();
      if (start.row < this.list[cell].list.Count)
      {
        obj.list = new List<string>();
        for (int row = start.row; row < this.list[cell].list.Count && row <= end.row; ++row)
          obj.list.Add(this.list[cell].list[row]);
      }
      objList.Add(obj);
    }
    return objList;
  }

  public List<ExcelData.Param> Get(
    ExcelData.Specify start,
    ExcelData.Specify end,
    string find)
  {
    List<ExcelData.Param> objList1 = new List<ExcelData.Param>();
    List<ExcelData.Param> objList2 = this.Get(start, end);
    foreach (ExcelData.Param obj1 in objList2)
    {
      foreach (string str1 in obj1.list)
      {
        if (find == str1)
        {
          ExcelData.Param obj2 = new ExcelData.Param();
          obj2.list = new List<string>();
          foreach (string str2 in obj1.list)
            obj2.list.Add(str2);
          objList2.Add(obj2);
          break;
        }
      }
    }
    return objList2;
  }

  public List<ExcelData.Param> Find(string find)
  {
    List<ExcelData.Param> objList = new List<ExcelData.Param>();
    foreach (ExcelData.Param obj1 in this.list)
    {
      foreach (string str1 in obj1.list)
      {
        if (find == str1)
        {
          ExcelData.Param obj2 = new ExcelData.Param();
          obj2.list = new List<string>();
          foreach (string str2 in obj1.list)
            obj2.list.Add(str2);
          objList.Add(obj2);
          break;
        }
      }
    }
    return objList;
  }

  public List<ExcelData.Param> FindArea(string find, ExcelData.Specify spe)
  {
    List<ExcelData.Param> objList = new List<ExcelData.Param>();
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < this.list.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.list[index1].list.Count; ++index2)
      {
        if (find == this.list[index1].list[index2])
        {
          num1 = index1 + 1;
          num2 = index2;
          break;
        }
      }
    }
    for (int index1 = num1; index1 < this.list.Count && index1 < num1 + spe.cell; ++index1)
    {
      ExcelData.Param obj = new ExcelData.Param();
      for (int index2 = num2; index2 < this.list[index1].list.Count && index2 < num2 + spe.row; ++index2)
      {
        obj.list = new List<string>();
        obj.list.Add(this.list[index1].list[index2]);
        objList.Add(obj);
      }
    }
    return objList;
  }

  public List<ExcelData.Param> FindArea(string find)
  {
    List<ExcelData.Param> objList = new List<ExcelData.Param>();
    int num1 = 0;
    int num2 = 0;
    for (int index1 = 0; index1 < this.list.Count; ++index1)
    {
      for (int index2 = 0; index2 < this.list[index1].list.Count; ++index2)
      {
        if (find == this.list[index1].list[index2])
        {
          num1 = index1 + 1;
          num2 = index2;
          break;
        }
      }
    }
    for (int index1 = num1; index1 < this.list.Count; ++index1)
    {
      ExcelData.Param obj = new ExcelData.Param();
      for (int index2 = num2; index2 < this.list[index1].list.Count; ++index2)
      {
        obj.list = new List<string>();
        obj.list.Add(this.list[index1].list[index2]);
        objList.Add(obj);
      }
    }
    return objList;
  }

  public class Specify
  {
    public int cell;
    public int row;

    public Specify(int cell, int row)
    {
      this.cell = cell;
      this.row = row;
    }

    public Specify()
    {
    }
  }

  [Serializable]
  public class Param
  {
    public List<string> list = new List<string>();
  }
}

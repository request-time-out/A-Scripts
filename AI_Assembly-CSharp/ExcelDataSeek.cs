// Decompiled with JetBrains decompiler
// Type: ExcelDataSeek
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

public class ExcelDataSeek
{
  public ExcelDataSeek(ExcelData data)
  {
    this.data = data;
    this.cell = 0;
    this.row = 0;
  }

  public ExcelData data { get; private set; }

  public int cell { get; private set; }

  public int row { get; private set; }

  public bool isEndCell
  {
    get
    {
      return this.cell >= this.data.list.Count;
    }
  }

  public bool isEndRow
  {
    get
    {
      bool flag = true;
      if (this.cell < this.data.list.Count && this.row < this.data.list[this.cell].list.Count)
        flag = false;
      return flag;
    }
  }

  public int SetCell(int set)
  {
    int num = set;
    this.cell = num;
    return num;
  }

  public int SetRow(int set)
  {
    int num = set;
    this.row = num;
    return num;
  }

  public int NextCell(int next)
  {
    return this.cell += next;
  }

  public int NextRow(int next)
  {
    return this.row += next;
  }

  public bool SearchCell(int next = 0, bool isErrorCheck = false)
  {
    bool flag = false;
    for (this.cell += next; this.cell < this.data.list.Count; ++this.cell)
    {
      if (this.row < this.data.list[this.cell].list.Count && this.data.list[this.cell].list[this.row] != string.Empty)
      {
        flag = true;
        break;
      }
    }
    return flag;
  }

  public bool SearchRow(int next = 0, bool isErrorCheck = false)
  {
    bool flag = false;
    this.row += next;
    if (this.cell < this.data.list.Count)
    {
      for (; this.row < this.data.list[this.cell].list.Count; ++this.row)
      {
        if (this.data.list[this.cell].list[this.row] != string.Empty)
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }
}

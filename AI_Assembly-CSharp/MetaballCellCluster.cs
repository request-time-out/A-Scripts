// Decompiled with JetBrains decompiler
// Type: MetaballCellCluster
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MetaballCellCluster : MetaballCellClusterInterface
{
  private List<MetaballCell> _cells = new List<MetaballCell>();
  private Vector3 _baseColor = Vector3.get_one();
  private float _baseRadius;

  public float BaseRadius
  {
    get
    {
      return this._baseRadius;
    }
    set
    {
      this._baseRadius = value;
    }
  }

  public void DoForeachCell(ForeachCellDeleg deleg)
  {
    foreach (MetaballCell cell in this._cells)
      deleg(cell);
  }

  public MetaballCell GetCell(int index)
  {
    return this._cells[index];
  }

  public int FindCell(MetaballCell cell)
  {
    for (int index = 0; index < this._cells.Count; ++index)
    {
      if (this._cells[index] == cell)
        return index;
    }
    return -1;
  }

  public int CellCount
  {
    get
    {
      return this._cells.Count;
    }
  }

  public MetaballCell AddCell(
    Vector3 position,
    float minDistanceCoef = 1f,
    float? radius = null,
    string tag = null)
  {
    MetaballCell cell = new MetaballCell();
    cell.baseColor = this._baseColor;
    cell.radius = radius.HasValue ? radius.Value : this._baseRadius;
    cell.modelPosition = position;
    cell.tag = tag;
    bool bFail = false;
    this.DoForeachCell((ForeachCellDeleg) (c =>
    {
      Vector3 vector3 = Vector3.op_Subtraction(cell.modelPosition, c.modelPosition);
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() >= (double) cell.radius * (double) cell.radius * (double) minDistanceCoef * (double) minDistanceCoef)
        return;
      bFail = true;
    }));
    if (!bFail)
      this._cells.Add(cell);
    return bFail ? (MetaballCell) null : cell;
  }

  public void RemoveCell(MetaballCell cell)
  {
    this._cells.Remove(cell);
  }

  public void ClearCells()
  {
    this._cells.Clear();
  }

  public string GetPositionsString()
  {
    string str = string.Empty;
    foreach (MetaballCell cell in this._cells)
    {
      str += ((Vector3) ref cell.modelPosition).ToString("F3");
      str += ";";
    }
    if (str[str.Length - 1] == ';')
      str = str.Substring(0, str.Length - 1);
    return str;
  }

  public void ReadPositionsString(string positions)
  {
    this.ClearCells();
    string[] strArray = positions.Split(';');
    if (strArray.Length == 0)
      throw new UnityException("invalid input positions data :" + positions);
    for (int index = 0; index < strArray.Length; ++index)
      this.AddCell(MetaballCellCluster.ParseVector3(strArray[index]), 0.0f, new float?(), (string) null);
  }

  private static Vector3 ParseVector3(string data)
  {
    int num1 = data.IndexOf('(');
    int num2 = data.IndexOf(')');
    string[] strArray = data.Substring(num1 + 1, num2 - num1 - 1).Split(',');
    Vector3 zero = Vector3.get_zero();
    for (int index = 0; index < 3 && index < strArray.Length; ++index)
      ((Vector3) ref zero).set_Item(index, float.Parse(strArray[index]));
    return zero;
  }
}

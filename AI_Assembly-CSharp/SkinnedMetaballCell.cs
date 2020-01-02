// Decompiled with JetBrains decompiler
// Type: SkinnedMetaballCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class SkinnedMetaballCell : MetaballCell, MetaballCellClusterInterface
{
  public List<SkinnedMetaballCell> children = new List<SkinnedMetaballCell>();
  public List<SkinnedMetaballCell> links = new List<SkinnedMetaballCell>();
  public SkinnedMetaballCell parent;
  public int distanceFromRoot;

  public float BaseRadius
  {
    get
    {
      return this.radius;
    }
  }

  public bool IsRoot
  {
    get
    {
      return this.parent == null;
    }
  }

  public bool IsTerminal
  {
    get
    {
      return this.children.Count == 0;
    }
  }

  public bool IsBranch
  {
    get
    {
      return this.IsRoot || this.children.Count > 1;
    }
  }

  public SkinnedMetaballCell Root
  {
    get
    {
      return this.IsRoot ? this : this.parent.Root;
    }
  }

  public int CellCount
  {
    get
    {
      int num = 1;
      foreach (SkinnedMetaballCell child in this.children)
        num += child.CellCount;
      return num;
    }
  }

  public void DoForeachSkinnedCell(SkinnedMetaballCell.ForeachSkinnedCellDeleg deleg)
  {
    deleg(this);
    foreach (SkinnedMetaballCell child in this.children)
      child.DoForeachSkinnedCell(deleg);
  }

  public void DoForeachCell(ForeachCellDeleg deleg)
  {
    deleg((MetaballCell) this);
    foreach (SkinnedMetaballCell child in this.children)
      child.DoForeachCell(deleg);
  }

  public int DistanceFromBranch
  {
    get
    {
      return this.IsBranch ? 0 : Mathf.Min(this.DistanceFromLastBranch, this.DistanceToNextBranch);
    }
  }

  public int DistanceFromLastLink
  {
    get
    {
      return this.IsRoot || this.children.Count > 1 || this.links.Count > 0 ? 0 : this.parent.DistanceFromLastLink + 1;
    }
  }

  private int DistanceFromLastBranch
  {
    get
    {
      return this.IsBranch ? 0 : 1 + this.parent.DistanceFromLastBranch;
    }
  }

  private int DistanceToNextBranch
  {
    get
    {
      if (this.IsBranch)
        return 0;
      int num = int.MaxValue;
      foreach (SkinnedMetaballCell child in this.children)
      {
        int distanceToNextBranch = child.DistanceToNextBranch;
        if (distanceToNextBranch < num)
          num = distanceToNextBranch;
      }
      return num;
    }
  }

  public SkinnedMetaballCell AddChild(
    Vector3 position,
    float in_radius,
    float minDistanceCoef = 1f)
  {
    SkinnedMetaballCell child = new SkinnedMetaballCell();
    child.baseColor = this.baseColor;
    child.radius = in_radius;
    child.distanceFromRoot = this.distanceFromRoot + 1;
    child.modelPosition = position;
    child.parent = this;
    this.children.Add(child);
    bool bFail = false;
    this.Root.DoForeachSkinnedCell((SkinnedMetaballCell.ForeachSkinnedCellDeleg) (c =>
    {
      if (c == child)
        return;
      Vector3 vector3 = Vector3.op_Subtraction(child.modelPosition, c.modelPosition);
      if ((double) ((Vector3) ref vector3).get_sqrMagnitude() >= (double) child.radius * (double) child.radius * (double) minDistanceCoef * (double) minDistanceCoef)
        return;
      bFail = true;
    }));
    if (bFail)
    {
      this.children.Remove(child);
      return (SkinnedMetaballCell) null;
    }
    child.CalcRotation();
    return child;
  }

  public void CalcRotation()
  {
    if (this.IsRoot)
      this.modelRotation = Quaternion.FromToRotation(Vector3.get_forward(), Vector3.get_up());
    else
      this.modelRotation = Quaternion.op_Multiply(Quaternion.FromToRotation(Quaternion.op_Multiply(this.parent.modelRotation, Vector3.get_forward()), Vector3.op_Subtraction(this.modelPosition, this.parent.modelPosition)), this.parent.modelRotation);
  }

  public string GetStringExpression()
  {
    string str = string.Empty + ((Vector3) ref this.modelPosition).ToString("F3") + ";";
    foreach (SkinnedMetaballCell child in this.children)
    {
      str += child.GetStringExpression();
      str += ";";
    }
    if (str[str.Length - 1] == ';')
      str = str.Substring(0, str.Length - 1);
    return str;
  }

  public static SkinnedMetaballCell ConstructFromString(
    string data,
    float radius)
  {
    string[] strArray = data.Split(';');
    if (strArray.Length == 0)
      throw new UnityException("invalid input data :" + data);
    SkinnedMetaballCell skinnedMetaballCell = new SkinnedMetaballCell();
    skinnedMetaballCell.parent = (SkinnedMetaballCell) null;
    skinnedMetaballCell.modelPosition = SkinnedMetaballCell.ParseVector3(strArray[0]);
    skinnedMetaballCell.radius = radius;
    skinnedMetaballCell.baseColor = Vector3.get_zero();
    skinnedMetaballCell.CalcRotation();
    for (int index = 1; index < strArray.Length; ++index)
    {
      Vector3 vector3 = SkinnedMetaballCell.ParseVector3(strArray[index]);
      skinnedMetaballCell.AddChild(vector3, radius, 0.0f);
    }
    return skinnedMetaballCell;
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

  public delegate void ForeachSkinnedCellDeleg(SkinnedMetaballCell c);
}

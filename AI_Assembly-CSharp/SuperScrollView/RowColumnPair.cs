// Decompiled with JetBrains decompiler
// Type: SuperScrollView.RowColumnPair
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace SuperScrollView
{
  public struct RowColumnPair
  {
    public int mRow;
    public int mColumn;

    public RowColumnPair(int row1, int column1)
    {
      this.mRow = row1;
      this.mColumn = column1;
    }

    public bool Equals(RowColumnPair other)
    {
      return this.mRow == other.mRow && this.mColumn == other.mColumn;
    }

    public static bool operator ==(RowColumnPair a, RowColumnPair b)
    {
      return a.mRow == b.mRow && a.mColumn == b.mColumn;
    }

    public static bool operator !=(RowColumnPair a, RowColumnPair b)
    {
      return a.mRow != b.mRow || a.mColumn != b.mColumn;
    }

    public override int GetHashCode()
    {
      return 0;
    }

    public override bool Equals(object obj)
    {
      return !object.ReferenceEquals((object) null, obj) && obj is RowColumnPair other && this.Equals(other);
    }
  }
}

// Decompiled with JetBrains decompiler
// Type: Studio.AddObjectCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace Studio
{
  public static class AddObjectCommand
  {
    public class AddItemCommand : ICommand
    {
      private int group = -1;
      private int category = -1;
      private int no = -1;
      private int dicKey = -1;
      private int initialPosition;
      private TreeNodeObject tno;

      public AddItemCommand(
        int _group,
        int _category,
        int _no,
        int _dicKey,
        int _initialPosition)
      {
        this.group = _group;
        this.category = _category;
        this.no = _no;
        this.dicKey = _dicKey;
        this.initialPosition = _initialPosition;
      }

      public void Do()
      {
        OCIItem ociItem = AddObjectItem.Load(new OIItemInfo(this.group, this.category, this.no, this.dicKey), (ObjectCtrlInfo) null, (TreeNodeObject) null, true, this.initialPosition);
        this.tno = ociItem == null ? (TreeNodeObject) null : ociItem.treeNodeObject;
      }

      public void Undo()
      {
        Studio.Studio.DeleteNode(this.tno);
        this.tno = (TreeNodeObject) null;
      }

      public void Redo()
      {
        this.Do();
        Studio.Studio.SetNewIndex(this.dicKey);
      }
    }

    public class AddLightCommand : ICommand
    {
      private int no = -1;
      private int dicKey = -1;
      private int initialPosition;
      private TreeNodeObject tno;

      public AddLightCommand(int _no, int _dicKey, int _initialPosition)
      {
        this.no = _no;
        this.dicKey = _dicKey;
        this.initialPosition = _initialPosition;
      }

      public void Do()
      {
        OCILight ociLight = AddObjectLight.Load(new OILightInfo(this.no, this.dicKey), (ObjectCtrlInfo) null, (TreeNodeObject) null, true, this.initialPosition);
        this.tno = ociLight == null ? (TreeNodeObject) null : ociLight.treeNodeObject;
      }

      public void Undo()
      {
        Studio.Studio.DeleteNode(this.tno);
        this.tno = (TreeNodeObject) null;
      }

      public void Redo()
      {
        this.Do();
        Studio.Studio.SetNewIndex(this.dicKey);
      }
    }
  }
}

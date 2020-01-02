// Decompiled with JetBrains decompiler
// Type: Housing.Command.AddItemCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace Housing.Command
{
  public class AddItemCommand : Housing.ICommand
  {
    private int id;
    private ObjectCtrl objectCtrl;

    public AddItemCommand(int _id)
    {
      this.id = _id;
    }

    public void Do()
    {
      this.objectCtrl = (ObjectCtrl) Singleton<Manager.Housing>.Instance.AddObject(this.id);
      bool flag = Singleton<Manager.Housing>.Instance.CheckOverlap((ObjectCtrl) (this.objectCtrl as OCItem));
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.AddList(this.objectCtrl);
      if (flag)
        Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
    }

    public void Redo()
    {
      bool flag = Singleton<Manager.Housing>.Instance.RestoreObject(this.objectCtrl, (ObjectCtrl) null, -1, true);
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.AddList(this.objectCtrl);
      if (!flag)
        return;
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }

    public void Undo()
    {
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.isOnFuncSkip = true;
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RemoveList(this.objectCtrl);
      Singleton<CraftScene>.Instance.UICtrl.AddUICtrl.Reselect();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RefreshList();
    }
  }
}

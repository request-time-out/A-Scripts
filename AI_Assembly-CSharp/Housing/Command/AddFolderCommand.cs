// Decompiled with JetBrains decompiler
// Type: Housing.Command.AddFolderCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace Housing.Command
{
  public class AddFolderCommand : Housing.ICommand
  {
    private ObjectCtrl objectCtrl;

    public void Do()
    {
      this.objectCtrl = Singleton<Manager.Housing>.Instance.AddFolder();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.AddList(this.objectCtrl);
    }

    public void Redo()
    {
      Singleton<Manager.Housing>.Instance.RestoreObject(this.objectCtrl, (ObjectCtrl) null, -1, true);
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.AddList(this.objectCtrl);
    }

    public void Undo()
    {
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.isOnFuncSkip = true;
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.RemoveList(this.objectCtrl);
    }
  }
}

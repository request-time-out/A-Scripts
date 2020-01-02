// Decompiled with JetBrains decompiler
// Type: Studio.OCICharMale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Manager;

namespace Studio
{
  public class OCICharMale : OCIChar
  {
    public ChaControl male
    {
      get
      {
        return this.charInfo;
      }
    }

    public override void OnDelete()
    {
      base.OnDelete();
      Singleton<Character>.Instance.DeleteChara(this.male, false);
    }

    public override void SetClothesStateAll(int _state)
    {
      base.SetClothesStateAll(_state);
      this.male.SetClothesStateAll((byte) _state);
    }

    public override void ChangeChara(string _path)
    {
      base.ChangeChara(_path);
    }

    public override void LoadClothesFile(string _path)
    {
      base.LoadClothesFile(_path);
    }
  }
}

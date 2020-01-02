// Decompiled with JetBrains decompiler
// Type: Studio.OCICharFemale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using Manager;

namespace Studio
{
  public class OCICharFemale : OCIChar
  {
    public ChaControl female
    {
      get
      {
        return this.charInfo;
      }
    }

    public override void OnDelete()
    {
      base.OnDelete();
      Singleton<Character>.Instance.DeleteChara(this.female, false);
    }

    public override void SetSiruFlags(ChaFileDefine.SiruParts _parts, byte _state)
    {
      base.SetSiruFlags(_parts, _state);
      this.female.SetSiruFlag(_parts, _state);
    }

    public override byte GetSiruFlags(ChaFileDefine.SiruParts _parts)
    {
      return this.female.GetSiruFlag(_parts);
    }

    public override void SetNipStand(float _value)
    {
      base.SetNipStand(_value);
      this.oiCharInfo.nipple = _value;
      this.female.ChangeNipRate(_value);
    }

    public override void ChangeChara(string _path)
    {
      base.ChangeChara(_path);
      this.female.UpdateBustSoftnessAndGravity();
      this.optionItemCtrl.ReCounterScale();
      this.optionItemCtrl.height = this.animeOptionParam1;
      this.animeOptionParam1 = this.animeOptionParam1;
      this.animeOptionParam2 = this.animeOptionParam2;
    }

    public override void SetClothesStateAll(int _state)
    {
      base.SetClothesStateAll(_state);
      this.female.SetClothesStateAll((byte) _state);
    }

    public override void LoadClothesFile(string _path)
    {
      base.LoadClothesFile(_path);
      this.female.UpdateBustSoftnessAndGravity();
      this.skirtDynamic = AddObjectFemale.GetSkirtDynamic(this.charInfo.objClothes);
      this.ActiveFK(OIBoneInfo.BoneGroup.Skirt, this.oiCharInfo.activeFK[6], this.oiCharInfo.enableFK);
    }
  }
}

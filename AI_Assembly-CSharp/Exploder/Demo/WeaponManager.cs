// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.WeaponManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public class WeaponManager : MonoBehaviour
  {
    public GameObject Shotgun;
    public GameObject RPG;

    public WeaponManager()
    {
      base.\u002Ector();
    }

    private void Update()
    {
      if (Input.GetKeyDown((KeyCode) 49))
      {
        ExploderUtils.SetActiveRecursively(this.RPG, false);
        ExploderUtils.SetActiveRecursively(this.Shotgun, true);
        ((ShotgunController) this.Shotgun.GetComponent<ShotgunController>()).OnActivate();
      }
      if (!Input.GetKeyDown((KeyCode) 50))
        return;
      ExploderUtils.SetActiveRecursively(this.RPG, true);
      ExploderUtils.SetActiveRecursively(this.Shotgun, false);
      ((RPGController) this.RPG.GetComponent<RPGController>()).OnActivate();
    }
  }
}

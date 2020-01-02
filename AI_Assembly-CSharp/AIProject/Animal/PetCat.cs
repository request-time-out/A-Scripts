// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.PetCat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

namespace AIProject.Animal
{
  public class PetCat : WalkingPetAnimal
  {
    [SerializeField]
    private string _materialTargetObjectName = string.Empty;
    [SerializeField]
    private string _textureKeyName = string.Empty;
    [SerializeField]
    private string[] _colorKeyNames = new string[4];

    protected override void Initialize()
    {
      base.Initialize();
      if (Object.op_Equality((Object) this.bodyObject, (Object) null))
        return;
      GameObject loop = this.bodyObject.get_transform().FindLoop(this._materialTargetObjectName);
      if (Object.op_Equality((Object) loop, (Object) null))
        return;
      Renderer component = (Renderer) loop.GetComponent<Renderer>();
      if (Object.op_Equality((Object) component, (Object) null))
        return;
      Material material = component.get_material();
      if (Object.op_Equality((Object) material, (Object) null) || !material.HasProperty(this._textureKeyName))
        return;
      AIProject.SaveData.AnimalData animalData = this.AnimalData;
      if (!Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<int, ValueTuple<Texture2D, Color[]>>> textureTable = Singleton<Resources>.Instance.AnimalTable.TextureTable;
      if (((IReadOnlyDictionary<int, Dictionary<int, ValueTuple<Texture2D, Color[]>>>) textureTable).IsNullOrEmpty<int, Dictionary<int, ValueTuple<Texture2D, Color[]>>>())
        return;
      int animalTypeId = this.AnimalTypeID;
      Dictionary<int, ValueTuple<Texture2D, Color[]>> dictionary;
      if (!textureTable.TryGetValue(animalTypeId, out dictionary) || ((IReadOnlyDictionary<int, ValueTuple<Texture2D, Color[]>>) dictionary).IsNullOrEmpty<int, ValueTuple<Texture2D, Color[]>>())
        return;
      if (animalData.First || !dictionary.ContainsKey(animalData.TextureID))
      {
        List<int> source = ListPool<int>.Get();
        source.AddRange((IEnumerable<int>) dictionary.Keys);
        animalData.TextureID = source.Rand<int>();
      }
      ValueTuple<Texture2D, Color[]> valueTuple = dictionary[animalData.TextureID];
      material.SetTexture(this._textureKeyName, (Texture) valueTuple.Item1);
      Color[] colorArray = (Color[]) valueTuple.Item2;
      if (((IReadOnlyList<Color>) colorArray).IsNullOrEmpty<Color>() || ((IReadOnlyList<string>) this._colorKeyNames).IsNullOrEmpty<string>())
        return;
      for (int index = 0; index < colorArray.Length && index < this._colorKeyNames.Length; ++index)
      {
        string colorKeyName = this._colorKeyNames[index];
        if (material.HasProperty(colorKeyName))
          material.SetColor(colorKeyName, colorArray[index]);
      }
    }
  }
}

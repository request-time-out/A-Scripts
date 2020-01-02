// Decompiled with JetBrains decompiler
// Type: AIProject.SaveData.AnimalData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIProject.Animal;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AIProject.SaveData
{
  [MessagePackObject(false)]
  public class AnimalData
  {
    public AnimalData()
    {
    }

    public AnimalData(AnimalData data)
    {
      this.Copy(data);
    }

    [Key(0)]
    public int AnimalID { get; set; }

    [Key(1)]
    public int RegisterID { get; set; } = -1;

    [Key(2)]
    public AnimalTypes AnimalType { get; set; }

    [Key(3)]
    public BreedingTypes BreedingType { get; set; } = BreedingTypes.Pet;

    [Key(4)]
    public string Nickname { get; set; } = string.Empty;

    [Key(5)]
    public int ItemCategoryID { get; set; } = -1;

    [Key(6)]
    public int ItemID { get; set; } = -1;

    [Key(7)]
    public int ModelID { get; set; }

    [Key(8)]
    public Vector3 Position { get; set; } = Vector3.get_zero();

    [Key(9)]
    public Quaternion Rotation { get; set; } = Quaternion.get_identity();

    [Key(10)]
    public Dictionary<int, float> Favorability { get; set; } = new Dictionary<int, float>();

    [Key(11)]
    public List<AnimalData.ColorData> ColorList { get; set; } = new List<AnimalData.ColorData>();

    [Key(12)]
    public bool First { get; set; }

    [Key(13)]
    public int TextureID { get; set; }

    [Key(14)]
    public int AnimalTypeID { get; set; } = -1;

    [Key(15)]
    public bool InitAnimalTypeID { get; set; }

    public void Copy(AnimalData data)
    {
      if (data == null)
        return;
      this.AnimalID = data.AnimalID;
      this.RegisterID = data.RegisterID;
      this.AnimalType = data.AnimalType;
      this.BreedingType = data.BreedingType;
      this.Nickname = data.Nickname;
      this.ItemCategoryID = data.ItemCategoryID;
      this.ItemID = data.ItemID;
      this.ModelID = data.ModelID;
      this.Position = data.Position;
      this.Rotation = data.Rotation;
      if (!data.Favorability.IsNullOrEmpty<int, float>())
        this.Favorability = data.Favorability.ToDictionary<KeyValuePair<int, float>, int, float>((Func<KeyValuePair<int, float>, int>) (x => x.Key), (Func<KeyValuePair<int, float>, float>) (x => x.Value));
      else
        this.Favorability.Clear();
      if (!data.ColorList.IsNullOrEmpty<AnimalData.ColorData>())
        this.ColorList = data.ColorList.Where<AnimalData.ColorData>((Func<AnimalData.ColorData, bool>) (x => x != null)).Select<AnimalData.ColorData, AnimalData.ColorData>((Func<AnimalData.ColorData, AnimalData.ColorData>) (x => new AnimalData.ColorData(x))).ToList<AnimalData.ColorData>();
      else
        this.ColorList.Clear();
      this.First = data.First;
      this.TextureID = data.TextureID;
      this.AnimalTypeID = data.AnimalTypeID;
      this.InitAnimalTypeID = data.InitAnimalTypeID;
    }

    public float AddFavorability(int actorID, float value)
    {
      float num1;
      if (!this.Favorability.TryGetValue(actorID, out num1))
        num1 = 0.0f;
      float num2 = Mathf.Clamp(num1 + value, 0.0f, 100f);
      this.Favorability[actorID] = num2;
      return num2;
    }

    public void SetFavorability(int actorID, float value)
    {
      this.Favorability[actorID] = Mathf.Clamp(value, 0.0f, 100f);
    }

    public float GetFavorability(int actorID)
    {
      float num;
      return this.Favorability.TryGetValue(actorID, out num) ? num : 0.0f;
    }

    public bool RemoveFavorability(int actorID)
    {
      return this.Favorability.Remove(actorID);
    }

    public void GetFavorabilityKeyPairs(ref List<KeyValuePair<int, float>> list)
    {
      if (list == null)
        return;
      foreach (KeyValuePair<int, float> keyValuePair in this.Favorability)
        list.Add(keyValuePair);
    }

    public bool MostFavorabilityActor(int actorID)
    {
      float num1 = -1f;
      int num2 = -1;
      foreach (KeyValuePair<int, float> keyValuePair in this.Favorability)
      {
        if ((double) num1 < (double) keyValuePair.Value)
        {
          num1 = keyValuePair.Value;
          num2 = keyValuePair.Key;
        }
      }
      return actorID == num2 && 0.0 <= (double) num1;
    }

    [MessagePackObject(false)]
    public class ColorData
    {
      public ColorData()
      {
      }

      public ColorData(AnimalData.ColorData data)
      {
        this.Copy(data);
      }

      [Key(0)]
      public float r { get; set; }

      [Key(1)]
      public float g { get; set; }

      [Key(2)]
      public float b { get; set; }

      [Key(3)]
      public float a { get; set; } = 1f;

      public void Copy(AnimalData.ColorData data)
      {
        if (data == null)
          return;
        this.r = data.r;
        this.g = data.g;
        this.b = data.b;
        this.a = data.a;
      }
    }
  }
}

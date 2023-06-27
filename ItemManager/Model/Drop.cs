using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace ItemManager.Model;

[PublicAPI]
public class DropTargets
{
    public readonly List<DropTarget> Drops = new();

    public void Add(string creatureName, float chance, int min = 1, int? max = null, bool levelMultiplier = true)
    {
        Drops.Add(new DropTarget { creature = creatureName, chance = chance, min = min, max = max ?? min, levelMultiplier = levelMultiplier });
    }
}

public struct DropTarget
{
    public string creature;
    public int min;
    public int max;
    public float chance;
    public bool levelMultiplier;
}

public class SerializedDrop
{
    public readonly List<DropTarget> Drops;

    public SerializedDrop(List<DropTarget> drops) => Drops = drops;

    public SerializedDrop(string drops)
    {
        Drops = (drops == "" ? Array.Empty<string>() : drops.Split(',')).Select(r =>
        {
            string[] parts = r.Split(':');
            if (parts.Length <= 2 || !int.TryParse(parts[2], out int min))
            {
                min = 1;
            }
            if (parts.Length <= 3 || !int.TryParse(parts[3], out int max))
            {
                max = min;
            }
            bool levelMultiplier = parts.Length <= 4 || parts[4] != "0";
            return new DropTarget { creature = parts[0], chance = parts.Length > 1 && float.TryParse(parts[1], out float chance) ? chance : 1, min = min, max = max, levelMultiplier = levelMultiplier };
        }).ToList();
    }

    public override string ToString()
    {
        return string.Join(",", Drops.Select(r => $"{r.creature}:{r.chance.ToString(CultureInfo.InvariantCulture)}:{r.min}:" + (r.min == r.max ? "" : $"{r.max}") + (r.levelMultiplier ? "" : ":0")));
    }

    private static Character? fetchByName(ZNetScene netScene, string name)
    {
        Character? character = netScene.GetPrefab(name)?.GetComponent<Character>();
        if (character == null)
        {
            Debug.LogWarning($"The drop target character '{name}' does not exist.");
        }
        return character;
    }

    public Dictionary<Character, CharacterDrop.Drop> toCharacterDrops(ZNetScene netScene, GameObject item)
    {
        Dictionary<Character, CharacterDrop.Drop> drops = new();
        foreach (DropTarget drop in Drops)
        {
            if (fetchByName(netScene, drop.creature) is { } character)
            {
                drops[character] = new CharacterDrop.Drop { m_prefab = item, m_amountMin = drop.min, m_amountMax = drop.max, m_chance = drop.chance, m_levelMultiplier = drop.levelMultiplier };
            }
        }

        return drops;
    }
}
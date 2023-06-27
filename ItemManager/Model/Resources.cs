using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace ItemManager.Model;

public struct Requirement
{
    public string itemName;
    public int amount;
    public int amountConfig;
}

[PublicAPI]
public class RequiredResourceList
{
    public readonly List<Requirement> Requirements = new();

    public bool
        Free = false; // If Requirements empty and Free is true, then it costs nothing. If Requirements empty and Free is false, then it won't be craftable.

    public void Add(string itemName, int amount) =>
        Requirements.Add(new Requirement { itemName = itemName, amount = amount });
}

public class SerializedRequirements
{
    public readonly List<Requirement> Reqs;

    public SerializedRequirements(List<Requirement> reqs) => Reqs = reqs;

    public SerializedRequirements(string reqs)
    {
        Reqs = reqs.Split(',').Select(r =>
        {
            string[] parts = r.Split(':');
            return new Requirement
            {
                itemName = parts[0], amount = parts.Length > 1 && int.TryParse(parts[1], out int amount) ? amount : 1
            };
        }).ToList();
    }

    public override string ToString()
    {
        return string.Join(",", Reqs.Select(r => $"{r.itemName}:{r.amount}"));
    }

    public static ItemDrop? fetchByName(ObjectDB objectDB, string name)
    {
        ItemDrop? item = objectDB.GetItemPrefab(name)?.GetComponent<ItemDrop>();
        if (item == null)
        {
            Debug.LogWarning($"The required item '{name}' does not exist.");
        }

        return item;
    }

    public static Piece.Requirement[] toPieceReqs(ObjectDB objectDB, SerializedRequirements craft,
        SerializedRequirements upgrade)
    {
        ItemDrop? ResItem(Requirement r) => fetchByName(objectDB, r.itemName);

        Dictionary<string, Piece.Requirement?> resources = craft.Reqs.Where(r => r.itemName != "")
            .ToDictionary(r => r.itemName,
                r => ResItem(r) is { } item
                    ? new Piece.Requirement
                        { m_amount = r.amountConfig, m_resItem = item, m_amountPerLevel = 0 }
                    : null);
        foreach (Requirement req in upgrade.Reqs.Where(r => r.itemName != ""))
        {
            if ((!resources.TryGetValue(req.itemName, out Piece.Requirement? requirement) || requirement == null) &&
                ResItem(req) is { } item)
            {
                requirement = resources[req.itemName] = new Piece.Requirement { m_resItem = item, m_amount = 0 };
            }

            if (requirement != null)
            {
                requirement.m_amountPerLevel = req.amountConfig;
            }
        }

        return resources.Values.Where(v => v != null).ToArray()!;
    }
}
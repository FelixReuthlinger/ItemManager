using System.Collections.Generic;
using JetBrains.Annotations;

namespace ItemManager.Model;

public struct CraftingStationConfig
{
    public CraftingTable Table;
    public int level;
    public string? custom;
}

[PublicAPI]
public class CraftingStationList
{
    public readonly List<CraftingStationConfig> Stations = new();

    public void Add(CraftingTable table, int level) => Stations.Add(new CraftingStationConfig { Table = table, level = level });
    public void Add(string customTable, int level) => Stations.Add(new CraftingStationConfig { Table = CraftingTable.Custom, level = level, custom = customTable });
}
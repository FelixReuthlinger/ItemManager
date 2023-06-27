using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ItemManager.Model;

public class Item
{
    private static readonly List<Item> RegisteredItems = new();
    private static readonly Dictionary<ItemDrop, Item> ItemDropMap = new();
    private static Dictionary<Item, Dictionary<string, List<Recipe>>> _activeRecipes = new();
    private static Dictionary<Recipe, bool> _hiddenCraftRecipes = new();
    private static Dictionary<Recipe, bool> _hiddenUpgradeRecipes = new();
    private static Dictionary<Item, Dictionary<string, ItemConfig>> _itemCraftConfigs = new();
    private static Dictionary<Item, string> _itemDropConfigs = new();

    private class ItemConfig
    {
        public string craft;
        public string upgrade;
        public CraftingTable table;
        public int tableLevel;
        public string customTable;
        public int maximumTableLevel;
        public Toggle requireOneIngredient;
        public float qualityResultAmountMultiplier;
    }

    public readonly GameObject Prefab;
    private Dictionary<CharacterDrop, CharacterDrop.Drop> _characterDrops = new();
    private readonly Dictionary<bool, Action> _statsConfigs = new();
    public int MaximumRequiredStationLevel = int.MaxValue;
    public readonly DropTargets DropsFrom = new();

    internal List<Conversion> Conversions = new();
    internal List<Smelter.ItemConversion> conversions = new();
    public Dictionary<string, ItemRecipe> Recipes = new();

    public Item(string assetBundleFileName, string prefabName, string folderName = "assets") : this(
        PrefabManager.RegisterAssetBundle(assetBundleFileName, folderName), prefabName)
    {
    }

    public Item(AssetBundle bundle, string prefabName) : this(PrefabManager.RegisterPrefab(bundle, prefabName, true),
        true)
    {
    }

    public Item(GameObject prefab, bool skipRegistering = false)
    {
        if (!skipRegistering)
        {
            PrefabManager.RegisterPrefab(prefab, true);
        }

        Prefab = prefab;
        RegisteredItems.Add(this);
        ItemDropMap[Prefab.GetComponent<ItemDrop>()] = this;
    }

    private delegate void setDmgFunc(ref HitData.DamageTypes dmg, float value);

    private static Localization? _english;
    private static Localization english => _english ??= LocalizationCache.ForLanguage("English");
    
    private LocalizeKey? _name;

    public LocalizeKey Name
    {
        get
        {
            if (_name is { } name)
            {
                return name;
            }

            ItemDrop.ItemData.SharedData data = Prefab.GetComponent<ItemDrop>().m_itemData.m_shared;
            if (data.m_name.StartsWith("$"))
            {
                _name = new LocalizeKey(data.m_name);
            }
            else
            {
                string key = "$item_" + Prefab.name.Replace(" ", "_");
                _name = new LocalizeKey(key).English(data.m_name);
                data.m_name = key;
            }

            return _name;
        }
    }

    private LocalizeKey? _description;

    public LocalizeKey Description
    {
        get
        {
            if (_description is { } description)
            {
                return description;
            }

            ItemDrop.ItemData.SharedData data = Prefab.GetComponent<ItemDrop>().m_itemData.m_shared;
            if (data.m_description.StartsWith("$"))
            {
                _description = new LocalizeKey(data.m_description);
            }
            else
            {
                string key = "$itemdesc_" + Prefab.name.Replace(" ", "_");
                _description = new LocalizeKey(key).English(data.m_description);
                data.m_description = key;
            }

            return _description;
        }
    }
}
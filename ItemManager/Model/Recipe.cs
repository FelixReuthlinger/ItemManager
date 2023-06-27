using JetBrains.Annotations;

namespace ItemManager.Model;

[PublicAPI]
public class ItemRecipe
{
    public readonly RequiredResourceList RequiredItems = new();
    public readonly RequiredResourceList RequiredUpgradeItems = new();
    public readonly CraftingStationList Crafting = new();
    public int CraftAmount = 1;
    public bool RequireOnlyOneIngredient = false;
    public float QualityResultAmountMultiplier = 1;
    public bool RecipeIsActive;
}
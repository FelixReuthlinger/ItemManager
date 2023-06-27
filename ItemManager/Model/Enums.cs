using System;
using JetBrains.Annotations;

namespace ItemManager.Model;

public enum Toggle
{
    On = 1,
    Off = 0
}

[PublicAPI]
public enum CraftingTable
{
    Disabled,
    Inventory,
    [InternalName("piece_workbench")] Workbench,
    [InternalName("piece_cauldron")] Cauldron,
    [InternalName("forge")] Forge,
    [InternalName("piece_artisanstation")] ArtisanTable,
    [InternalName("piece_stonecutter")] StoneCutter,
    [InternalName("piece_magetable")] MageTable,
    [InternalName("blackforge")] BlackForge,
    Custom
}

[PublicAPI]
public enum ConversionPiece
{
    Disabled,
    [InternalName("smelter")] Smelter,
    [InternalName("charcoal_kiln")] CharcoalKiln,
    [InternalName("blastfurnace")] BlastFurnace,
    [InternalName("windmill")] Windmill,
    [InternalName("piece_spinningwheel")] SpinningWheel,
    [InternalName("eitrrefinery")] EitrRefinery,
    Custom
}

[PublicAPI]
public enum DamageModifier
{
    Normal,
    Resistant,
    Weak,
    Immune,
    Ignore,
    VeryResistant,
    VeryWeak,
    None
}
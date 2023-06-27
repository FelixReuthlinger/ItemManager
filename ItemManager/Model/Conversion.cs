using JetBrains.Annotations;

namespace ItemManager.Model;


[PublicAPI]
public class Conversion
{
    internal class ConversionConfig
    {
        public string input = null!;
        public string? activePiece;
        public ConversionPiece piece ;
        public string customPiece = null!;
    }

    public string Input = null!;
    public ConversionPiece Piece;
    internal string? customPiece = null;

    public string? Custom
    {
        get => customPiece;
        set
        {
            customPiece = value;
            Piece = ConversionPiece.Custom;
        }
    }

    internal ConversionConfig? config;

    public Conversion(Item outputItem)
    {
        outputItem.Conversions.Add(this);
    }
}
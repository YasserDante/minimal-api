namespace Api;

public class Symbol
{
    public required string Name { get; set; }

    public double High { get; set; }

    public double Low { get; set; }

    public double Open { get; set; }

    public double Close { get; set; }

    public DateTime Date { get; set; }
}

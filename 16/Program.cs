using Common;

string? s;
using (var streamReader = new StreamReader("input.txt"))
{
    s = streamReader.ReadLine();
}

var bytes = StringToByteArray(s);

var parser = new Parser();
var packet = parser.Parse(bytes);
Console.WriteLine(parser.Packets.Sum(x => x.Version));
var result = packet.Evaluate();
Console.WriteLine(result);

byte[] StringToByteArray(string hex)
{
    return Enumerable.Range(0, hex.Length)
        .Where(x => x % 2 == 0)
        .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
        .ToArray();
}

public class Parser
{
    public List<Packet> Packets { get; set; } = new List<Packet>();

    public Packet Parse(byte[] bytes)
    {
        var position = 0;
        return Parse(bytes, ref position);
    }

    private Packet Parse(byte[] bytes, ref int position)
    {
        var version = bytes.Next(ref position, 3);
        var packetType = bytes.Next(ref position, 3);
        if (packetType == 4)
        {
            return ParseLiteral(bytes, ref position, version);
        }
        else
        {
            return ParseOperator(bytes, ref position, version, packetType);
        }
    }

    private LiteralPacket ParseLiteral(byte[] bytes, ref int position, int version)
    {
        int isNotLast;
        long number = 0;
        do
        {
            number <<= 4;
            isNotLast = bytes.Next(ref position, 1);
            var part = bytes.Next(ref position, 4);
            number += part;
        } while (isNotLast == 1);

        var literalPacket = new LiteralPacket(version, number);
        Packets.Add(literalPacket);
        return literalPacket;
    }

    private OperatorPacket ParseOperator(byte[] bytes, ref int position, int version, int packetType)
    {
        var subPackets = new List<Packet>();
        var is11Bit = bytes.Next(ref position, 1);
        if (is11Bit == 1)
        {
            var numberOfSubPackets = bytes.Next(ref position, 11);
            for (int i = 0; i < numberOfSubPackets; i++)
            {
                subPackets.Add(Parse(bytes, ref position));
            }
        }
        else
        {
            var lengthInBits = bytes.Next(ref position, 15);
            var initPosition = position;
            while (initPosition + lengthInBits > position)
            {
                subPackets.Add(Parse(bytes, ref position));
            }
        }

        OperatorPacket operatorPacket = packetType switch
        {
            0 => new SumOperatorPacket(version, packetType, subPackets),
            1 => new ProductOperatorPacket(version, packetType, subPackets),
            2 => new MinimumOperatorPacket(version, packetType, subPackets),
            3 => new MaximumOperatorPacket(version, packetType, subPackets),
            5 => new GraterThanOperatorPacket(version, packetType, subPackets[0], subPackets[1]),
            6 => new LessThanOperatorPacket(version, packetType, subPackets[0], subPackets[1]),
            7 => new EqualToOperatorPacket(version, packetType, subPackets[0], subPackets[1]),
            _ => throw new ArgumentOutOfRangeException(nameof(packetType), packetType, null)
        };
        Packets.Add(operatorPacket);
        return operatorPacket;
    }
}

public class LiteralPacket : Packet
{
    public long Number { get; }

    public LiteralPacket(int version, long number) : base(version, 4)
    {
        Number = number;
    }

    public override long Evaluate()
    {
        return Number;
    }
}

public abstract class OperatorPacket : Packet
{
    protected OperatorPacket(int version, int packetType) : base(version, packetType)
    {
    }
}
public abstract class MultiOperatorPacket : OperatorPacket
{
    public List<Packet> Packets { get; }
    protected MultiOperatorPacket(int version, int packetType, List<Packet> packets) : base(version, packetType)
    {
        Packets = packets;
    }
}


public abstract class BiOperatorPacket : OperatorPacket
{
    public Packet Packet1 { get; }
    public Packet Packet2 { get; }

    public BiOperatorPacket(int version, int packetType, Packet packet1, Packet packet2) : base(version, packetType)
    {
        Packet1 = packet1;
        Packet2 = packet2;
    }
}
public class SumOperatorPacket : MultiOperatorPacket
{
    public SumOperatorPacket(int version, int packetType, List<Packet> packets) : base(version, packetType, packets)
    {
    }

    public override long Evaluate()
    {
        return Packets.Sum(x => x.Evaluate());
    }
}

public class ProductOperatorPacket : MultiOperatorPacket
{
    public ProductOperatorPacket(int version, int packetType, List<Packet> packets) : base(version, packetType, packets)
    {
    }

    public override long Evaluate()
    {
        return Packets.Aggregate(1L, (i, packet) => i * packet.Evaluate());
    }
}

public class MinimumOperatorPacket : MultiOperatorPacket
{
    public MinimumOperatorPacket(int version, int packetType, List<Packet> packets) : base(version, packetType, packets)
    {
    }

    public override long Evaluate()
    {
        return Packets.Min(x => x.Evaluate());
    }
}

public class MaximumOperatorPacket : MultiOperatorPacket
{
    public MaximumOperatorPacket(int version, int packetType, List<Packet> packets) : base(version, packetType, packets)
    {
    }

    public override long Evaluate()
    {
        return Packets.Max(x => x.Evaluate());
    }
}


public class GraterThanOperatorPacket : BiOperatorPacket
{
    public GraterThanOperatorPacket(int version, int packetType, Packet packet1, Packet packet2) : base(version, packetType, packet1, packet2)
    {
    }


    public override long Evaluate()
    {
        var r1 = Packet1.Evaluate();
        var r2 = Packet2.Evaluate();
        return r1 > r2 ? 1 : 0;
    }
}

public class LessThanOperatorPacket : BiOperatorPacket
{
    public LessThanOperatorPacket(int version, int packetType, Packet packet1, Packet packet2) : base(version, packetType, packet1, packet2)
    {
    }
    public override long Evaluate()
    {
        var r1 = Packet1.Evaluate();
        var r2 = Packet2.Evaluate();
        return r1 < r2 ? 1 : 0;
    }
}

public class EqualToOperatorPacket : BiOperatorPacket
{
    public EqualToOperatorPacket(int version, int packetType, Packet packet1, Packet packet2) : base(version, packetType, packet1, packet2)
    {
    }
    public override long Evaluate()
    {
        var r1 = Packet1.Evaluate();
        var r2 = Packet2.Evaluate();
        return r1 == r2 ? 1 : 0;
    }
}

public abstract class Packet
{
    public int Version { get; }
    public int PacketType { get; }

    public Packet(int version, int packetType)
    {
        Version = version;
        PacketType = packetType;
    }

    public abstract long Evaluate();
}


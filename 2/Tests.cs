using Common;
using Xunit;

namespace _2;

public class Tests
{
    [Theory]
    [InlineData("8A004A801A8002F478", 0, 3, 4)]
    [InlineData("8A004A801A8002F478", 3, 3, 2)]
    [InlineData("8A004A801A8002F478", 6, 1, 1)]
    [InlineData("8A004A801A8002F478", 7, 11, 1)]
    [InlineData("8A004A801A8002F478", 18, 3, 1)]
    [InlineData("8A004A801A8002F478", 21, 3, 2)]
    public void ByteExtensionExtractNumber(string hex, int start, int length, int expectedResult)
    {
        // Arrange
        var bytes = StringToByteArray(hex);

        // Act
        var result = bytes.ExtractNumber(start, length);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    public static byte[] StringToByteArray(string hex) {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray();
    }
}
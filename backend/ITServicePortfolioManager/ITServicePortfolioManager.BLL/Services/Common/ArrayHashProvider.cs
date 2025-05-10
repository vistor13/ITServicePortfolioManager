using System.Security.Cryptography;

namespace ITServicePortfolioManager.BLL.Services.Common;

public static class ArrayHashProvider
{
    public static byte[] ConvertArrayToByteArray(int[,] array)
    {
        int rows = array.GetLength(0);
        int cols = array.GetLength(1);
        byte[] byteArray = new byte[rows * cols * sizeof(int)];

        Buffer.BlockCopy(array, 0, byteArray, 0, byteArray.Length);
        return byteArray;
    }
   
    public static string ComputeSHA256Hash(byte[] byteArray)
    {
        using SHA256 sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(byteArray);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
    
}
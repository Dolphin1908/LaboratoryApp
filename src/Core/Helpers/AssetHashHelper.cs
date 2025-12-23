using System.Security.Cryptography;
using System.Text;

namespace LaboratoryApp.src.Core.Helpers
{
    /// <summary>
    /// Helpers hỗ trợ xử lý hash cho tài nguyên (Asset)
    /// </summary>
    public static class AssetHashHelper
    {
        public static string ComputeSha256Hash(byte[] data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(data);
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string ComputeTextHash(string text)
        {
            return ComputeSha256Hash(Encoding.UTF8.GetBytes(text));
        }
    }
}

using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace CommonLib
{
	public class Utils
	{
		public static int ParseInt(ReadOnlySpan<char> p_StringValue)
		{
			int _numberreturn;
			int.TryParse(p_StringValue, out _numberreturn);
			return _numberreturn;
		}

		public static long ParseLongSpan(ReadOnlySpan<char> p_StringValue)
		{
			long _numberreturn;
			long.TryParse(p_StringValue, out _numberreturn);
			return _numberreturn;
		}

		public static double ParseDoubleSpan(ReadOnlySpan<char> p_StringValue)
		{
			double _numberreturn;
			double.TryParse(p_StringValue, out _numberreturn);
			return _numberreturn;
		}

		public static bool Validator_Data_Date(string value)
		{
			DateTime date;
			return DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
		}

		public static string EncryptAES(string plainText, string Key, string IV)
		{
			try
			{
				using (Aes aesAlg = Aes.Create())
				{
					aesAlg.Key = Encoding.UTF8.GetBytes(Key);
					aesAlg.IV = Encoding.UTF8.GetBytes(IV);
					aesAlg.Mode = CipherMode.CBC;
					aesAlg.Padding = PaddingMode.PKCS7;
					ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

					byte[] encryptedBytes;

					using (MemoryStream msEncrypt = new MemoryStream())
					{
						using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
						{
							using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
							{
								swEncrypt.Write(plainText);
							}
						}
						encryptedBytes = msEncrypt.ToArray();
					}

					return Convert.ToBase64String(encryptedBytes);
				}
			}
			catch (Exception ex)
			{
				Logger.log.Error("Error exception when EncryptStringAES . Exception : {0}", ex);
				return "";
			}
		}

		public static string DecryptAES(string CipherText, string Key, string IV)
		{
			try
			{
				byte[] CipherTextByte = Convert.FromBase64String(CipherText);
				byte[] KeyByte = Encoding.UTF8.GetBytes(Key);
				byte[] IVByte = Encoding.UTF8.GetBytes(IV);
				string plaintext = null;
				// Create AesManaged
				using (Aes aesAlg = Aes.Create())
				{
					aesAlg.Padding = PaddingMode.PKCS7;

					aesAlg.Key = KeyByte;
					aesAlg.IV = IVByte;

					aesAlg.Mode = CipherMode.CBC;
					// Create a decryptor to perform the stream transform.
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					// Create the streams used for decryption.
					using (MemoryStream msDecrypt = new MemoryStream(CipherTextByte))
					{
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							using (StreamReader srDecrypt = new StreamReader(csDecrypt))
							{
								// Read the decrypted bytes from the decrypting stream
								// and place them in a string.
								plaintext = srDecrypt.ReadToEnd();
							}
						}
					}
				}
				return plaintext;
			}
			catch (Exception ex)
			{
				Logger.log.Error("Error exception when EncryptStringAES . Exception : {0}", ex);
				return "";
			}
		}

		public static string GenOrderNo()
		{
			return Guid.NewGuid().ToString();
		}

		private static readonly string VietNamChar = "áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ" + "éèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄ" + "óòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ" + "úùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮ" + "íìịỉĩÍÌỊỈĨ" + "đĐ" + "ýỳỵỷỹÝỲỴỶỸ";

		public static bool CheckTiengViet(string str)
		{
			for (int i = 0; i < str.Length; i++)
			{
				if (VietNamChar.Contains(str[i]))
				{
					return true;
				}
			}
			return false;
		}

		private static readonly string lowChar = "qwertyuiopasdfghjklzxcvbnm";

		public static bool CheckPassword_LowChar(string str)
		{
			for (int i = 0; i < lowChar.Length; i++)
			{
				for (int j = 0; j < str.Length; j++)
				{
					if (str[j].ToString() == lowChar[i].ToString())
						return true;
				}
			}
			return false;
		}

		private static readonly string upChar = "QWERTYUIOPASDFGHJKLZXCVBNM";

		public static bool CheckPassword_UpperChar(string str)
		{
			for (int i = 0; i < upChar.Length; i++)
			{
				for (int j = 0; j < str.Length; j++)
				{
					if (str[j].ToString() == upChar[i].ToString())
						return true;
				}
			}
			return false;
		}

		private static readonly string numChar = "0123456789";

		public static bool CheckPassword_NumChar(string str)
		{
			for (int i = 0; i < numChar.Length; i++)
			{
				for (int j = 0; j < str.Length; j++)
				{
					if (str[j].ToString() == numChar[i].ToString())
						return true;
				}
			}
			return false;
		}

		private static readonly string specPass = "-@#$%^&+=()!";

		public static bool CheckPassword_SpecialChar(string str)
		{
			for (int i = 0; i < specPass.Length; i++)
			{
				for (int j = 0; j < str.Length; j++)
				{
					if (str[j].ToString() == specPass[i].ToString())
						return true;
				}
			}
			return false;
		}
	}
}
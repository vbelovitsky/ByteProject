using System;
using System.IO;

namespace ByteLibrary
{
	public abstract class AbstractByte
	{

		protected FileStream BaseStream { get; set; }

		protected AbstractByte(FileStream stream)
		{
			BaseStream = stream;
		}

		protected int GetByteCount(Type T)
		{
			switch (Type.GetTypeCode(T))
			{
				case TypeCode.Boolean:
					return 1;
				case TypeCode.Byte:
					return 1;
				case TypeCode.Char:
					return 2;
				case TypeCode.Int16:
					return 2;
				case TypeCode.Int32:
					return 4;
				case TypeCode.Int64:
					return 8;
				case TypeCode.Double:
					return 8;
				case TypeCode.Single:
					return 4;
				default:
					return 1;
			}
		}

		protected static bool CheckStructType(Type T)
		{
			switch (Type.GetTypeCode(T))
			{
				case TypeCode.Boolean:
				case TypeCode.Byte:
				case TypeCode.Char:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.Double:
				case TypeCode.Single:
					return true;
				default:
					return false;
			}
		}
	}
}

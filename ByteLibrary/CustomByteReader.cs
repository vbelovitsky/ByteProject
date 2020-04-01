using System;
using System.IO;
using System.Collections.Generic;

namespace ByteLibrary
{

	public delegate void OnReadValue(dynamic value);

	public class CustomByteReader : AbstractByte
	{

		/// <summary>
		/// Можно получать
		/// </summary>
		public event OnReadValue OnRead;

		private BinaryReader reader { get; set; }

		public CustomByteReader(FileStream fileStream) : base(fileStream)
		{
			reader = new BinaryReader(BaseStream);
			
		}

		/// <summary>
		/// Считывает массив данных
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] ReadArray<T>() where T : struct
		{
			if (!CheckStructType(typeof(T)))
			{
				throw new ByteException("Method assepts only primitive CLR types as <T> parameter, e.g. int, char, double...");
			}

			List<T> array = new List<T>();

			int sizeBytes = GetByteCount(typeof(T));
			
			long len = BaseStream.Length / sizeBytes;
			try
			{
				for (int i = 0; i < len; i++)
				{
					// BaseStream.Seek(i * sizeBytes, SeekOrigin.Begin);

					dynamic value = ReadValue<T>();
					OnRead?.Invoke(value);
					array.Add(value);
				}
			}
			catch (IOException)
			{
				Console.Error.WriteLine("io: Unable to create file stream due to an i/o error, exiting...");
			}
			catch (System.Security.SecurityException)
			{
				Console.Error.WriteLine("security: Not enough permissions to open stream on this file, exiting...");
			}
			catch (ArgumentException)
			{
				Console.Error.WriteLine("env: Output file path given incorrectly, exiting...");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"error: {e.Message}");
			}

			return array.ToArray();
		}

		/// <summary>
		/// Считывает данные наоборот
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] ReadReversedArray<T>() where T : struct
		{
			if (!CheckStructType(typeof(T)))
			{
				throw new ByteException("Method assepts only primitive CLR types as <T> parameter, e.g. int, char, double...");
			}

			List<T> array = new List<T>();

			int sizeBytes = GetByteCount(typeof(T));

			long len = BaseStream.Length / sizeBytes;
			try
			{
				for (int i = (int)(len - 1); i >= 0; i--)
				{
					BaseStream.Seek(i * 4, SeekOrigin.Begin);
					dynamic value = ReadValue<T>();
					OnRead?.Invoke(value);
					array.Add(value);
				}
			}
			catch (IOException)
			{
				Console.Error.WriteLine("io: Unable to create file stream due to an i/o error, exiting...");
			}
			catch (System.Security.SecurityException)
			{
				Console.Error.WriteLine("security: Not enough permissions to open stream on this file, exiting...");
			}
			catch (ArgumentException)
			{
				Console.Error.WriteLine("env: Output file path given incorrectly, exiting...");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"error: {e.Message}");
			}

			return array.ToArray();
		}

		/// <summary>
		/// Считывает значение из reader
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		private dynamic ReadValue<T>() where T : struct
		{
			
			switch (Type.GetTypeCode(typeof(T)))
			{
				case TypeCode.Boolean:
					return reader.ReadBoolean();
				case TypeCode.Byte:
					return reader.ReadByte();
				case TypeCode.Char:
					return reader.ReadChar();
				case TypeCode.Int16:
					return reader.ReadInt16();
				case TypeCode.Int32:
					return reader.ReadInt32();
				case TypeCode.Int64:
					return reader.ReadInt64();
				case TypeCode.Double:
					return reader.ReadDouble();
				case TypeCode.Single:
					return reader.ReadSingle();
				default:
					return reader.ReadByte();
			}
		}

		/// <summary>
		/// Закрывает writer и поток
		/// </summary>
		public void Close()
		{
			try
			{
				reader.Close();
			}
			catch (IOException e)
			{
				Console.WriteLine($"Unable to close writer and stream: {e.Message}");
			}
		}

	}
}

using System;
using System.IO;

namespace ByteLibrary
{
    public class CustomByteWriter : AbstractByte
    {

		private BinaryWriter writer { get; set; }

		public CustomByteWriter(FileStream stream) : base(stream)
		{
			writer = new BinaryWriter(BaseStream);
		}

		public void WriteArray<T>(T[] array) where T : struct
		{
			if (!CheckStructType(typeof(T)))
			{
				throw new ByteException("Method assepts only primitive CLR types as <T> parameter, e.g. int, char, double...");
			}
			try
			{
				for (int i = 0; i < array.Length; i += 1)
				{
					writer.Write((dynamic)array[i]);
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
		}

		public void WriteValue<T>(T value, int index) where T : struct
		{
			if (!CheckStructType(typeof(T)))
			{
				throw new ByteException("Method assepts only primitive CLR types as <T> parameter, e.g. int, char, double...");
			}
			try
			{
				BaseStream.Seek( index * GetByteCount(typeof(T)), SeekOrigin.Begin);
				writer.Write((dynamic)value);
			}
			catch (IOException e)
			{
				Console.Error.WriteLine($"io: Unable to create file stream due to an i/o error, exiting... {e.Message}");
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
		}

		public void Close()
		{
			try
			{
				writer.Close();
			}
			catch (IOException e)
			{
				Console.WriteLine($"Unable to close writer and stream: {e.Message}");
			}
		}

    }
}

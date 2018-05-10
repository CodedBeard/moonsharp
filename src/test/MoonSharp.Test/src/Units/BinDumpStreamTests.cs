using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter.IO;
using Xunit;

namespace MoonSharp.Interpreter.Tests.Units
{
	
	public class BinDumpStreamTests
	{
		[Fact]
		public void BinDumpBinaryStreams_TestIntWrites()
		{
			int[] values = new int[] { 0, 1, -1, 10, -10, 32767, 32768, -32767, -32768, int.MinValue, int.MaxValue };

			using(MemoryStream ms_orig = new MemoryStream())
			{
				UndisposableStream ms = new UndisposableStream(ms_orig);

				using(BinDumpBinaryWriter bdbw = new BinDumpBinaryWriter(ms, Encoding.UTF8))
				{
					for(int i = 0; i < values.Length; i++)
					{
						bdbw.Write(values[i]);
					}
				}

				ms.Seek(0, SeekOrigin.Begin);

				using(BinDumpBinaryReader bdbr = new BinDumpBinaryReader(ms, Encoding.UTF8))
				{
					for (int i = 0; i < values.Length; i++)
					{
						int v = bdbr.ReadInt32();
                        
                        Assert.Equal(values[i], v);
                    }
				}
			}
		}

		[Fact]
		public void BinDumpBinaryStreams_TestUIntWrites()
		{
			uint[] values = new uint[] { 0, 1, 0x7F, 10, 0x7E, 32767, 32768, uint.MinValue, uint.MaxValue };

			using (MemoryStream ms_orig = new MemoryStream())
			{
				UndisposableStream ms = new UndisposableStream(ms_orig);

				using (BinDumpBinaryWriter bdbw = new BinDumpBinaryWriter(ms, Encoding.UTF8))
				{
					for (int i = 0; i < values.Length; i++)
					{
						bdbw.Write(values[i]);
					}
				}

				ms.Seek(0, SeekOrigin.Begin);

				using (BinDumpBinaryReader bdbr = new BinDumpBinaryReader(ms, Encoding.UTF8))
				{
					for (int i = 0; i < values.Length; i++)
					{
						uint v = bdbr.ReadUInt32();
                        
                        Assert.Equal(values[i], v);
					}
				}
			}
		}


		[Fact]
		public void BinDumpBinaryStreams_TestStringWrites()
		{
			string[] values = new string[] { "hello", "you", "fool", "hello", "I", "love", "you" };

			using (MemoryStream ms_orig = new MemoryStream())
			{
				UndisposableStream ms = new UndisposableStream(ms_orig);

				using (BinDumpBinaryWriter bdbw = new BinDumpBinaryWriter(ms, Encoding.UTF8))
				{
					for (int i = 0; i < values.Length; i++)
					{
						bdbw.Write(values[i]);
					}
				}

				ms.Seek(0, SeekOrigin.Begin);

				using (BinDumpBinaryReader bdbr = new BinDumpBinaryReader(ms, Encoding.UTF8))
				{
					for (int i = 0; i < values.Length; i++)
					{
						string v = bdbr.ReadString();
                        
                        Assert.Equal(values[i], v);
                    }
				}
			}
		}


	}
}

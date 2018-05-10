using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class VtUserDataIndexerTests
	{
		public struct IndexerTestClass
		{
			public Dictionary<int, int> mymap;

			public int this[int idx]
			{
				get { return mymap[idx]; }
				set { mymap[idx] = value; }
			}

			public int this[int idx1, int idx2, int idx3]
			{
				get { int idx = (idx1 + idx2) * idx3; return mymap[idx]; }
				set { int idx = (idx1 + idx2) * idx3; mymap[idx] = value; }
			}
		}

		private void IndexerTest(string code, int expected)
		{
			Script S = new Script();

			IndexerTestClass obj = new IndexerTestClass();
			obj.mymap = new Dictionary<int, int>();

			UserData.RegisterType<IndexerTestClass>();

			S.Globals.Set("o", UserData.Create(obj));

			DynValue v = S.DoString(code);

			Assert.Equal(DataType.Number, v.Type);
			Assert.Equal(expected, v.Number);
		}

		[Fact]
		public void VInterop_SingleSetterOnly()
		{
			string script = @"o[1] = 1; return 13";
			IndexerTest(script, 13);
		}


		[Fact]
		public void VInterop_SingleIndexerGetSet()
		{
			string script = @"o[5] = 19; return o[5];";
			IndexerTest(script, 19);
		}

		[Fact]
		public void VInterop_MultiIndexerGetSet()
		{
			string script = @"o[1,2,3] = 47; return o[1,2,3];";
			IndexerTest(script, 47);
		}

		[Fact]
		public void VInterop_MultiIndexerMetatableGetSet()
		{
			string script = @"
				m = { 
					__index = o,
					__newindex = o
				}

				t = { }

				setmetatable(t, m);

				t[10,11,12] = 1234; return t[10,11,12];";
			IndexerTest(script, 1234);
		}

		[Fact]
		
		public void VInterop_MultiIndexerMetamethodGetSet()
		{
			string script = @"
				m = { 
					__index = function() end,
					__newindex = function() end
				}

				t = { }

				setmetatable(t, m);

				t[10,11,12] = 1234; return t[10,11,12];";
            Assert.Throws<ScriptRuntimeException>(() => { IndexerTest(script, 1234); });
            
		}





		[Fact]
		public void VInterop_MixedIndexerGetSet()
		{
			string script = @"o[3,2,3] = 119; return o[15];";
			IndexerTest(script, 119);
		}

		[Fact]
		
		public void VInterop_ExpListIndexingCompilesButNotRun1()
		{
			string script = @"    
				x = { 99, 98, 97, 96 }				
				return x[2,3];
				";
            Assert.Throws<ScriptRuntimeException>(() => {
                DynValue res = Script.RunString(script);

                Assert.Equal(DataType.Number, res.Type);
                Assert.Equal(98, res.Number);
            });
            
		}

		[Fact]
		
		public void VInterop_ExpListIndexingCompilesButNotRun2()
		{
			string script = @"    
				x = { 99, 98, 97, 96 }				
				x[2,3] = 5;
				";
            Assert.Throws<ScriptRuntimeException>(() => {
                DynValue res = Script.RunString(script);

                Assert.Equal(DataType.Number, res.Type);
                Assert.Equal(98, res.Number);
            });
            
		}



	}
}

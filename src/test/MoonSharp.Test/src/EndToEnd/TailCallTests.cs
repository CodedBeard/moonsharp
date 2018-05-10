using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoonSharp.Interpreter.Execution;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	
	public class TailCallTests
	{
		[Fact]
		public void TcoTest_Pre()
		{
			// this just verifies the algorithm for TcoTest_Big
			string script = @"
				function recsum(num, partial)
					if (num == 0) then
						return partial
					else
						return recsum(num - 1, partial + num)
					end
				end
				
				return recsum(10, 0)";


			Script S = new Script();
			var res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(55, res.Number);
		}

		[Fact]
		public void TcoTest_Big()
		{
			// calc the sum of the first N numbers in the most stupid way ever to waste stack and trigger TCO..
			// (this could be a simple X*(X+1) / 2... )
			string script = @"
				function recsum(num, partial)
					if (num == 0) then
						return partial
					else
						return recsum(num - 1, partial + num)
					end
				end
				
				return recsum(70000, 0)";


			Script S = new Script();
			var res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(2450035000.0, res.Number);
		}


		[Fact]
		public void TailCallFromCLR()
		{
			string script = @"
				function getResult(x)
					return 156*x;  
				end

				return clrtail(9)";


			Script S = new Script();

			S.Globals.Set("clrtail", DynValue.NewCallback((xc, a) =>
			{
				DynValue fn = S.Globals.Get("getResult");
				DynValue k3 = DynValue.NewNumber(a[0].Number / 3);

				return DynValue.NewTailCallReq(fn, k3);
			}));

			var res = S.DoString(script);

			Assert.Equal(DataType.Number, res.Type);
			Assert.Equal(468, res.Number);
		}


		[Fact]
		public void CheckToString()
		{
			string script = @"
				return tostring(9)";


			Script S = new Script(CoreModules.Basic);
			var res = S.DoString(script);

			Assert.Equal(DataType.String, res.Type);
			Assert.Equal("9", res.String);
		}

		[Fact]
		public void CheckToStringMeta()
		{
			string script = @"
				t = {}
				m = {
					__tostring = function(v)
						return 'ciao';
					end
				}

				setmetatable(t, m);
				s = tostring(t);

				return (s);";


			Script S = new Script();
			var res = S.DoString(script);

			Assert.Equal(DataType.String, res.Type);
			Assert.Equal("ciao", res.String);
		}
	}
}

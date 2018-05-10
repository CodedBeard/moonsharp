using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MoonSharp.Interpreter.Tests.EndToEnd
{
	public static class Utils
	{
		public static void DynAssert(DynValue result, params object[] args)
		{
			if (args == null)
				args = new object[1] { DataType.Void };


			if (args.Length == 1)
			{
				DynAssertValue(args[0], result);
			}
			else
			{
				Assert.Equal(DataType.Tuple, result.Type);
				Assert.Equal(args.Length, result.Tuple.Length);

				for(int i = 0; i < args.Length; i++)
					DynAssertValue(args[i], result.Tuple[i]);
			}
		}

		private static void DynAssertValue(object reference, DynValue dynValue)
		{
			if (reference == (object)DataType.Void)
			{
				Assert.Equal(DataType.Void, dynValue.Type);
			}
			else if (reference == null)
			{
				Assert.Equal(DataType.Nil, dynValue.Type);
			}
			else if (reference is double)
			{
				Assert.Equal(DataType.Number, dynValue.Type);
				Assert.Equal((double)reference, dynValue.Number);
			}
			else if (reference is int)
			{
				Assert.Equal(DataType.Number, dynValue.Type);
				Assert.Equal((int)reference, dynValue.Number);
			}
			else if (reference is string)
			{
				Assert.Equal(DataType.String, dynValue.Type);
				Assert.Equal((string)reference, dynValue.String);
			}
		}


	}
}

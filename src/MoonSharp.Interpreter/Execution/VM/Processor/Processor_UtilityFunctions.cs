﻿using System.Collections.Generic;

namespace MoonSharp.Interpreter.Execution.VM
{
	sealed partial class Processor
	{
        private DynValue[] Internal_AdjustTuple(IList<DynValue> values)
        {
            if (values == null || values.Count == 0)
                return PooledArray<DynValue>.Request(0);

            DynValue[] result;
            if (values[values.Count - 1].Type == DataType.Tuple)
            {
                int baseLen = values.Count - 1 + values[values.Count - 1].Tuple.Length;
                result = PooledArray<DynValue>.Request(baseLen);

                for (int i = 0; i < values.Count - 1; i++)
                {
                    result[i] = values[i].ToScalar();
                }

                for (int i = 0; i < values[values.Count - 1].Tuple.Length; i++)
                {
                    result[values.Count + i - 1] = values[values.Count - 1].Tuple[i];
                }

                if (result[result.Length - 1].Type == DataType.Tuple)
                {
                    var oldResult = result;
                    result = Internal_AdjustTuple(result);
                    PooledArray<DynValue>.Release(oldResult);
                }
            }
            else
            {
                result = PooledArray<DynValue>.Request(values.Count);

                for (int i = 0; i < values.Count; i++)
                {
                    result[i] = values[i].ToScalar();
                }
            }
            return result;
        }



        private int Internal_InvokeUnaryMetaMethod(DynValue op1, string eventName, int instructionPtr)
		{
			DynValue m = DynValue.Invalid;

			if (op1.Type == DataType.UserData)
			{
				m = op1.UserData.Descriptor.MetaIndex(m_Script, op1.UserData, eventName);
			}

			if (!m.IsValid)
			{
				var op1_MetaTable = GetMetatable(op1);

				if (op1_MetaTable != null)
				{
					DynValue meta1 = op1_MetaTable.RawGet(eventName);
					if (meta1.IsValid && meta1.IsNotNil())
						m = meta1;
				}
			}

			if (m.IsValid)
			{
				m_ValueStack.Push(m);
				m_ValueStack.Push(op1);
				return Internal_ExecCall(1, instructionPtr);
			}
			else
			{
				return -1;
			}
		}
		private int Internal_InvokeBinaryMetaMethod(DynValue l, DynValue r, string eventName, int instructionPtr, DynValue extraPush = default(DynValue))
		{
		    var m = GetBinaryMetamethod(l, r, eventName);

		    if (!m.IsValid)
		        return -1;

		    if (extraPush.IsValid)
		        m_ValueStack.Push(extraPush);

		    m_ValueStack.Push(m);
		    m_ValueStack.Push(l);
		    m_ValueStack.Push(r);
		    return Internal_ExecCall(2, instructionPtr);
		}


	    private DynValue[] StackTopToArray(int items, bool pop)
		{
			DynValue[] values = new DynValue[items];

			if (pop)
			{
				for (int i = 0; i < items; i++)
				{
					values[i] = m_ValueStack.Pop();
				}
			}
			else
			{
				for (int i = 0; i < items; i++)
				{
					values[i] = m_ValueStack[m_ValueStack.Count - 1 - i];
				}
			}

			return values;
		}

		private DynValue[] StackTopToArrayReverse(int items, bool pop)
		{
			DynValue[] values = new DynValue[items];

			if (pop)
			{
				for (int i = 0; i < items; i++)
				{
					values[items - 1 - i] = m_ValueStack.Pop();
				}
			}
			else
			{
				for (int i = 0; i < items; i++)
				{
					values[items - 1 - i] = m_ValueStack[m_ValueStack.Count - 1 - i];
				}
			}

			return values;
		}



	}
}

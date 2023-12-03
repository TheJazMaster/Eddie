// Extensions
using System;
using System.Collections.Generic;
using System.Linq;

internal static class Extensions
{
	public static void QueueImmediate<T>(this List<T> source, T item)
	{
		source.Insert(0, item);
	}

	public static void Queue<T>(this List<T> source, T item)
	{
		source.Add(item);
	}
}

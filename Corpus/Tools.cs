using System.Diagnostics.Tracing;

namespace Corpus;

internal static class Tools {
	/// <summary>
	/// Mezcla un diccionario de listas ordenadas de entero.
	/// </summary>
	/// <param name="indexDictionary">Diccionario de (palabra, lista)</param>
	/// <returns>Lista de tuplas ordenadas de la forma(word, index). Siendo word la palabra de ese índice.</returns>
	private static List<(string word, int index)> SortedMerge(this Dictionary<string, List<int>> indexDictionary) {
		var merged = new List<(string word, int index)>();
		var indexes = indexDictionary.Keys.ToDictionary(word => word, _ => 0);

		while (true) {
			var min = int.MaxValue;
			var minWord = "";
			foreach (var (word, index) in indexes) {
				if (index >= indexDictionary[word].Count) continue;
				if (indexDictionary[word][index] >= min) continue;
				min = indexDictionary[word][index];
				minWord = word;
			}

			if (min == int.MaxValue) break;
			merged.Add((minWord, min));
			indexes[minWord]++;
		}

		return merged;
	}

	/// <summary>
	/// Elimina de los extremos de una palabra todos los caracteres no alfanuméricos.
	/// </summary>
	/// <param name="word">Palabra.</param>
	/// <returns>Palabra con los extremos no alfanuméricos eliminados.</returns>
	internal static string TrimPunctuation(this string word) {
		var first = 0;
		var last = word.Length;
		for (var i = 0; i < word.Length; i++) {
			if (!char.IsLetterOrDigit(word[i])) continue;
			first = i;
			break;
		}

		for (var i = word.Length - 1; i >= 0; i--) {
			if (!char.IsLetterOrDigit(word[i])) continue;
			last = i + 1;
			break;
		}

		return first < last ? word[first..last] : "";
	}

	/// <summary>
	/// Aplica la función TrimPunctuation a cada elemento de una colección.
	/// </summary>
	/// <param name="words">Colección de palabras sobre la cual aplicar TrimPunctuation.</param>
	/// <returns>Colección de palabras tras aplicar TrimPunctuation sobre ellas.</returns>
	internal static IEnumerable<string> TrimPunctuation(this IEnumerable<string> words) => words.Select(TrimPunctuation);

	internal static IEnumerable<(int index, T elem)> Enumerate<T>(this IEnumerable<T> collection) {
		var index = 0;
		foreach (var elem in collection)
			yield return (index++, elem);
	}

	internal static int Proximity(Dictionary<string, List<int>> indexDictionary, IEnumerable<int>? gaps, int minAmount) {
		var mask = indexDictionary.Keys.ToDictionary(word => word, _ => false);
		var indexes = indexDictionary.SortedMerge();
		indexes.TrimExcess();
		gaps ??= Enumerable.Range(0, indexes.Last().index);
		var end = indexes.Last().index;
		var minGap = int.MaxValue - end;
		var bestLength = int.MaxValue;
		var count = 0;
		for (var left = indexes.Count - minAmount + 1; left >= 0; left--) {
			for (var right = left;
			     right < indexes.Count && indexes[left].index + minGap > indexes[right].index;
			     right++) {
				var (word, index) = indexes[right];
				if (!mask[word]) count++;
				mask[word] = true;
				if (count != minAmount) continue;
				bestLength = index - indexes[left].index;
				foreach (var gap in gaps) {
					if (gap < bestLength) {
						minGap = gap;
						continue;
					}

					bestLength = gap;
					break;
				}

				break;
			}

			foreach (var wordKey in mask.Keys) mask[wordKey] = false;
			count = 0;
		}

		return bestLength;
	}
}
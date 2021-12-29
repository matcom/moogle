using System.Collections;

namespace Corpus; 

public class Query:IEnumerable<string> {
	private readonly string _text;
	public Query(string text) {
		_text = text;
	}

	public IEnumerator<string> GetEnumerator() {
		return _text.Split().Select(word => TrimPunt(word.ToLower())).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	private string TrimPunt(string word) {
		var first = 0;
		var last = word.Length;
		for (var i = 0; i < word.Length; i++) {
			if (char.IsPunctuation(word[i])) continue;
			first = i;
			break;
		}

		for (var i = word.Length - 1; i >= 0; i--) {
			if (char.IsPunctuation(word[i])) continue;
			last = i + 1;
			break;
		}

		return first < last ? word[first..last] : "";
	}
}
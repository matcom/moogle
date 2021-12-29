using System.Collections;

namespace Corpus; 

public class Query:IEnumerable<string> {
	private readonly string _text;
	public Query(string text) {
		_text = text;
	}

	public IEnumerator<string> GetEnumerator() {
		foreach (var word in _text.Split()) {
			yield return word.ToLower();
		}
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}
}
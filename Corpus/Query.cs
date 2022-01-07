using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Corpus; 

public class Query:IEnumerable<string> {
	private List<string> _text;
	private readonly HashSet<string> _exclusions;
	private readonly HashSet<string> _inclusions;
	private readonly HashSet<(string, string)> _proximity;
	public Query(string text) {
		_text = text.ToLower().Split().ToList();
		_exclusions = new HashSet<string>();
		_inclusions = new HashSet<string>();
		_proximity = new HashSet<(string, string)>();
		ProcessQuery();
	}

	public IEnumerator<string> GetEnumerator() {
		return _text.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	private void ProcessQuery() {
		ProcessProximity();
		foreach (var word in _text) {
			if (ToExclude(word)) _exclusions.Add(TrimPunt(word));
			if (ToInclude(word)) _inclusions.Add(TrimPunt(word));
		}

		_text = _text.Select(TrimPunt).Where(word => !Exclusions().Contains(word)).ToList();
	}

	private void ProcessProximity() {
		for (var i = 1; i < _text.Count - 1; i++) {
			if (_text[i] != "~") continue;
			var previous = TrimPunt(_text[i - 1]);
			var next = TrimPunt(_text[i + 1]);
			if (previous is not "" && next is not "") _proximity.Add((previous, next));
		}
	}
	
	private static bool ToExclude(string word) => word.StartsWith('!');
	private static bool ToInclude(string word) => word.StartsWith('^');

	public IEnumerable<string> Inclusions() => _inclusions;
	public IEnumerable<string> Exclusions() => _exclusions;

	private static string TrimPunt(string word) {
		var first = 0;
		var last = word.Length;
		for (var i = 0; i < word.Length; i++) {
			if (char.IsPunctuation(word[i]) || word[i] == '^' || word[i] == '~') continue;
			first = i;
			break;
		}

		for (var i = word.Length - 1; i >= 0; i--) {
			if (char.IsPunctuation(word[i]) || word[i] == '^' || word[i] == '~') continue;
			last = i + 1;
			break;
		}

		return first < last ? word[first..last] : "";
	}
}
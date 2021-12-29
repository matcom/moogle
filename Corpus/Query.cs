using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Corpus;

public class Query : IEnumerable<string> {
	private List<string> _text;
	private readonly HashSet<string> _exclusions;
	private readonly HashSet<string> _inclusions;
	private readonly HashSet<HashSet<string>> _proximity;

	public Query(string text) {
		_text = text.ToLower().Split().ToList();
		_exclusions = new HashSet<string>();
		_inclusions = new HashSet<string>();
		_proximity = new HashSet<HashSet<string>>();
		ProcessQuery();
	}

	public IEnumerator<string> GetEnumerator() {
		return _text.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator() {
		return GetEnumerator();
	}

	private void ProcessQuery() {
		ProcessBinProximity();
		ProcessNonBinProximity();
		ProcessPriority();
		foreach (var word in _text) {
			if (ToExclude(word)) _exclusions.Add(word.TrimPunctuation());
			if (ToInclude(word)) _inclusions.Add(word.TrimPunctuation());
		}

		_text = _text.Select(Tools.TrimPunctuation).Where(word => !Exclusions().Contains(word) && word.Length!=0).ToList();
	}

	private void ProcessBinProximity() {
		for (var i = 1; i < _text.Count - 1; i++) {
			if (_text[i] != "~") continue;
			var previous = _text[i - 1].TrimPunctuation();
			var next = _text[i + 1].TrimPunctuation();
			if (previous is not "" && next is not "") _proximity.Add(new HashSet<string> { previous, next });
		}
	}

	private void ProcessNonBinProximity() {
		var set = new HashSet<string>();
		for (var i = 1; i < _text.Count - 1; i++) {
			if (_text[i] is not "~~") continue;
			var previous = _text[i - 1].TrimPunctuation();
			if (previous != "") set.Add(previous);
			for (var j = i; j < _text.Count && _text[j] is "~~"; j+=2) {
				i = j;
				var next = _text[j + 1].TrimPunctuation();
				if (next is not "") set.Add(next);
			}

			if (set.Count > 1) _proximity.Add(set);
		}
	}

	private void ProcessPriority() {
		var toAdd = new List<string>();
		foreach (var word in _text) {
			for (var i = 0; i < word.Length; i++) {
				switch (word[i]) {
					case '^':
						continue;
					case '*':
						toAdd.Add(word.TrimPunctuation());
						continue;
				}

				break;
			}
		}

		_text.InsertRange(_text.Count,toAdd);
	}

	private static bool ToExclude(string word) => word.StartsWith('!');
	private static bool ToInclude(string word) => word.StartsWith('^');

	public IEnumerable<string> Inclusions() => _inclusions;
	public IEnumerable<string> Exclusions() => _exclusions;

	public IEnumerable<HashSet<string>> Proximity() => _proximity;
}
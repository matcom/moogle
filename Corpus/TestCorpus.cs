using System.Diagnostics;
using System.Text;

namespace Corpus;

public class TestCorpus : Corpus {
	private readonly string _path;
	private readonly Dictionary<string, (int totalOcurrencies, Dictionary<string, int>)> _vocabulary;
	private readonly Dictionary<string, int> _mostRepeatedWordOccurrences;

	public TestCorpus(string path) {
		_path = path;
		_mostRepeatedWordOccurrences = new Dictionary<string, int>();
		_vocabulary = new Dictionary<string, (int totalOcurrencies, Dictionary<string, int>)>();
		ProcessCorpus();
		WordsCount = _vocabulary.Count;
		DocsCount = Directory.GetFiles(_path).Length;
	}

	public override int this[string document, string word] {
		get {
			if (!_vocabulary.ContainsKey(word) ||
			    !_vocabulary[word].Item2.ContainsKey(document)) return 0;
			return _vocabulary[word].Item2[document];
		}
		protected set {
			if (!_vocabulary.ContainsKey(word)) _vocabulary.Add(word, (0, new Dictionary<string, int>()));
			if (!_vocabulary[word].Item2.ContainsKey(document)) _vocabulary[word].Item2.Add(document, 0);
			if (_vocabulary[word].Item2[document] == 0)
				_vocabulary[word] = (_vocabulary[word].Item1 + 1, _vocabulary[word].Item2);
			_vocabulary[word].Item2[document] = value;
		}
	}

	public override int this[string word] => _vocabulary.ContainsKey(word) ? _vocabulary[word].Item1 : 0;

	protected sealed override void ProcessCorpus() {
		foreach (var document in Documents()) {
			var words = File.ReadAllText(document).ToLower().Split();
			foreach (var word in TrimPunt(words)) {
				this[document, word]++;
			}

			_mostRepeatedWordOccurrences.Add(document, words.Select(word => this[document, word]).Max());
		}
	}

	public override int MostRepeatedWordOccurrences(string document) => _mostRepeatedWordOccurrences[document];

	public override IEnumerable<string> Words() {
		return _vocabulary.Keys;
	}

	public override IEnumerable<string> Documents() {
		return Directory.GetFiles(_path).Where(t => t.EndsWith(".txt"));
	}

	private IEnumerable<string> TrimPunt(IEnumerable<string> lol) => lol.Select(TrimPunt).Where(t => t != "");

	private string TrimPunt(string word) {
		var first = 0;
		var last = word.Length;
		for (var i = 0; i < word.Length; i++) {
			if (char.IsPunctuation(word[i]) || word[i] == '^') continue;
			first = i;
			break;
		}

		for (var i = word.Length - 1; i >= 0; i--) {
			if (char.IsPunctuation(word[i]) || word[i] == '^') continue;
			last = i + 1;
			break;
		}

		return first < last ? word[first..last] : "";
	}
}
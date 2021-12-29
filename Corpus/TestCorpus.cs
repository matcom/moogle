using System.Text.Json;
using System.Text.Json.Nodes;

namespace Corpus;

public class TestCorpus : Corpus {
	private readonly string _path;
	private readonly Dictionary<string, Dictionary<string, List<int>>> _vocabulary;
	private readonly Dictionary<string, int> _mostRepeatedWordOccurrences;

	public TestCorpus(string path) {
		_path = path;
		_mostRepeatedWordOccurrences = new Dictionary<string, int>();
		_vocabulary = new Dictionary<string, Dictionary<string, List<int>>>();
		ProcessCorpus();
		WordsCount = _vocabulary.Count;
		DocsCount = Directory.GetFiles(_path).Length;
	}

	/// <summary>
	/// Devuelve una lista de índices de una palabra en un documento.
	/// </summary>
	/// <param name="document">Documento.</param>
	/// <param name="word">Palabra.</param>
	/// <param name="creating">Determina si crear una nueva lista o devolver null en caso de que la palabra no esté en el documento.</param>
	private List<int>? this[string document, string word, bool creating] {
		get {
			if (!creating) {
				if (!_vocabulary.ContainsKey(word) || !_vocabulary[word].ContainsKey(document)) {
					return null;
				}
			}

			if (!_vocabulary.ContainsKey(word)) _vocabulary.Add(word, new Dictionary<string, List<int>>());
			if (!_vocabulary[word].ContainsKey(document)) _vocabulary[word].Add(document, new List<int>());
			return _vocabulary[word][document];
		}
	}

	/// <summary>
	/// Devuelve la cantidad de ocurrencias de una palabra en un documento.
	/// </summary>
	/// <param name="document">Documento.</param>
	/// <param name="word">Palabra.</param>
	/// <returns>Cantidad de ocurrencias si la palabra está en el documento. En caso contrario 0.</returns>>
	public override int this[string document, string word] =>
		this[document, word, false]?.Count ?? 0;

	/// <summary>
	/// Devuelve la cantidad de documentos que contienen una palabra.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <returns>Cantidad de documentos que contienen la palabra si al menos uno la contiene. En caso contrario 0.</returns>
	public override int this[string word] => _vocabulary.ContainsKey(word) ? _vocabulary[word].Count : 0;

	/// <summary>
	/// Procesa el corpus.
	/// </summary>
	protected sealed override void ProcessCorpus() {
		foreach (var document in Documents()) {
			var words = File.ReadAllText(document).ToLower().Split().TrimPunctuation().ToArray();
			foreach (var (index, word) in words.Enumerate().Where(t => t.elem.Length != 0))
				this[document, word, true]?.Add(index);

			foreach (var word in _vocabulary.Keys)
				this[document, word, false]?.TrimExcess();
			_mostRepeatedWordOccurrences.Add(document, words.Select(word => this[document, word]).Max());
		}
	}

	/// <summary>
	/// Devuelve la cantidad de veces que se repite la palabra que más se repita en un documento.
	/// </summary>
	/// <param name="document">Documento</param>
	/// <returns>Cantidad de veces que se repite la palabra que más se repite.</returns>
	public override int MostRepeatedWordOccurrences(string document) => _mostRepeatedWordOccurrences[document];

	/// <summary>
	/// Devuelve una colección de las palabras del corpus.
	/// </summary>
	/// <returns>Colección de las palabras del corpus.</returns>
	public override IEnumerable<string> Words() => _vocabulary.Keys;

	/// <summary>
	/// Devuelve una colección de los documentos del corpus.
	/// </summary>
	/// <returns>Colección con las direcciones de los documentos del corpus.</returns>
	public override IEnumerable<string> Documents() => Directory.GetFiles(_path).Where(t => t.EndsWith(".txt"));

	/// <summary>
	/// Devuelve cual es el menor de los gaps que contiene a todas las palabras del array word en un documento.
	/// </summary>
	/// <param name="document">Documento a buscar.</param>
	/// <param name="words">Array de palabras.</param>
	/// <param name="minAmount">Cantidad mínima de palabras que tienen que estar contenidas en el gap.</param>
	/// <param name="gaps">Longitudes relevantes para los gaps.</param>
	/// <returns>Un entero con la longitud del menor de los gaps que los contiene a minAmount words.</returns>
	public override int Proximity(string document, IEnumerable<string> words, int minAmount,
		IEnumerable<int>? gaps = null) {
		var indexDictionary = LoadIndexes(document, words);
		return indexDictionary.Count < minAmount ? int.MaxValue : Tools.Proximity(indexDictionary, gaps, minAmount);
	}

	private Dictionary<string, List<int>> LoadIndexes(string document, IEnumerable<string> words) =>
		words.Where(word => this[document, word, false] is not null)
			.ToDictionary(word => word, word => this[document, word, false]!);
	
}
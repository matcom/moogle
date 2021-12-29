using Corpus;

namespace MRI;

public class VectorMRI : MRI {
	public VectorMRI(Corpus.Corpus corpus) : base(corpus) { }

	public override IEnumerable<(string Key, float)> Query(Query query) {
		var result = Corpus.Documents.Select(t => (t.Key, Similarity(query: query, t.Key)))
			.OrderByDescending(t => t.Item2);
		Console.WriteLine("Done Processing Query");
		return result.ToList();
	}

	/// <summary>
	/// Calcula el peso de una palabra en un documento.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="document">Documento</param>
	/// <returns>Peso</returns>
	float CalculateW(string word, string document) => CalculateTf(word, document) * CalculateIdf(word);

	/// <summary>
	/// Calcula el peso de una palabra en la consulta
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="query">Consulta</param>
	/// <param name="a">Suavizado</param>
	/// <returns>Peso</returns>
	float CalculateWq(string word, Query query, float a = 0.5f) =>
		query.Contains(word) ? CalculateIdf(word) * (a + (1 - a) * CalculateTf(word, query)) : 0;
	
	float CalculateTf(string word, string document) => (float)Freq(word, document) / MaxL(document);
	float CalculateTf(string word, Query query) => (float)Freq(word, query) / MaxL(query);

	float CalculateIdf(string word) =>
		(float)Math.Log10((float)Corpus.Documents.Count / Corpus.Documents.Count(t => t.Value.ContainsKey(word)));

	int MaxL(string document) => Corpus.Documents[document].Values.Max();
	int MaxL(Query query) => query.Select(t => query.Count(i => t == i)).Max();

	int Freq(string word, string document) =>
		Corpus.Documents[document].ContainsKey(word) ? Corpus.Documents[document][word] : 0;

	int Freq(string word, Query query) => query.Contains(word) ? query.Count(t => t == word) : 0;

	IEnumerable<float> Weights(string document, Query query) {
		foreach (var (word, _) in Corpus.Vocabulary.Where(t=>query.Contains(t.Key))) {
			yield return CalculateW(word, document);
		}
	}

	IEnumerable<float> Weights(Query query) {
		foreach (var (word, _) in Corpus.Vocabulary.Where(t=>query.Contains(t.Key))) {
			yield return CalculateWq(word, query);
		}
	}

	float Norm(IEnumerable<float> vector) => (float)Math.Sqrt(vector.Select(t => Math.Pow(t, 2)).Sum());

	float Similarity(Query query, string document) {
		return Weights(query).Zip(Weights(document, query)).Select(t => t.First * t.Second).Sum() /
		       (Norm(Weights(query)) * Norm(Weights(document, query)));
	}
}
using Corpus;

namespace MRI;

public class VectorMRI : MRI {
	public VectorMRI(Corpus.Corpus corpus) : base(corpus) { }

	public override IEnumerable<(string Key, float)> Query(Query query) {
		return Corpus.Documents().Select(document => (document, Similarity(query, document))).ToList().OrderByDescending(t=>t.Item2);
	}

	/// <summary>
	/// Calcula el peso de una palabra en un documento.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="document">Documento</param>
	/// <returns>Peso</returns>
	float CalculateW(string word, string document) => CalculateTf(word, document) * CalculateIdf(word);
	
	/// <summary>
	/// Calcula el peso de una palabra en la consulta.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="query">Consulta</param>
	/// <param name="a">Suavizado</param>
	/// <returns>Peso</returns>
	float CalculateWq(string word, Query query, float a = 0.5f) =>
		CalculateIdf(word) * (a + (1 - a) * CalculateTf(word, query));
	
	/// <summary>
	/// Calcula la frecuencia normalizada de una palabra en un documento.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="document">Documento</param>
	/// <returns></returns>
	float CalculateTf(string word, string document) => (float)Freq(word, document) / MaxL(document);
	
	/// <summary>
	/// Calcula la frecuencia normalizada de una palabra en un la consulta.
	/// </summary>
	/// <param name="word">Palabra</param>
	/// <param name="query">Consulta</param>
	/// <returns>Frecuencia normalizada</returns>
	float CalculateTf(string word, Query query) => (float)Freq(word, query) / MaxL(query);

	float CalculateIdf(string word) =>
		Corpus[word] == 0 ? 0 : (float)Math.Log10((float)Corpus.DocsCount / Corpus[word]);

	int MaxL(string document) => Corpus.MostRepeatedWordOccurrences(document);
	int MaxL(Query query) => query.Select(t => query.Count(i => t == i)).Max();

	int Freq(string word, string document) =>
		Corpus[document, word];

	int Freq(string word, Query query) => query.Contains(word) ? query.Count(t => t == word) : 0;

	IEnumerable<float> Weights(string document, Query query) => query.Select(word => CalculateW(word, document));

	IEnumerable<float> Weights(string document) => from word in Corpus.Words() where Corpus[document, word] != 0 select CalculateW(word, document);

	IEnumerable<float> Weights(Query query) {
		return query.Select(word => CalculateWq(word, query));
	}

	float Norm(IEnumerable<float> vector) => (float)Math.Sqrt(vector.Select(t => Math.Pow(t, 2)).Sum());

	float Similarity(Query query, string document) {
		return Weights(query).Zip(Weights(document, query)).Select(t => t.First * t.Second).Sum() /
		       (Norm(Weights(query)) * Norm(Weights(document)));
	}
}
using Corpus;

namespace MRI;

public abstract class MRI {
	protected readonly Corpus.Corpus Corpus;

	protected MRI(Corpus.Corpus corpus) {
		Corpus = corpus;
	}
	public abstract IEnumerable<(string document, float score)> Query(Query query);
}
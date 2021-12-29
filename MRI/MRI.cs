using Corpus;

namespace MRI;

public abstract class MRI {
	protected readonly Corpus.Corpus Corpus;

	protected MRI(Corpus.Corpus corpus) {
		Corpus = corpus;
	}
	public abstract IEnumerable<(string Key, float)> Query(Query query);
}
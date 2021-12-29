using System.Diagnostics;

namespace Corpus;

public abstract class Corpus {
	public int WordsCount;
	public int DocsCount;

	protected Corpus() {
	}

	public abstract int this[string document, string word] { get; protected set; }
	public abstract int this[string word] { get; }
	protected abstract void ProcessCorpus();

	public abstract int MostRepeatedWordOccurrences(string document);

	public abstract IEnumerable<string> Words();
	public abstract IEnumerable<string> Documents();
}
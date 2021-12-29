using System.Diagnostics;

namespace Corpus;

public abstract class Corpus {
	protected readonly string Path;
	public readonly Dictionary<string, Dictionary<string, int>> Documents;
	public readonly Dictionary<string, int> Vocabulary;
	protected Corpus(string path) {
		Path = path;
		Documents = new Dictionary<string, Dictionary<string, int>>();
		Vocabulary = new Dictionary<string, int>();
		ProcessCorpus();
	}

	protected abstract void ProcessCorpus();
}
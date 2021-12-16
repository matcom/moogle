namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        SearchItem[] items = new SearchItem[3] {
            new("Hello World", "Lorem ipsum dolor sit amet", 0.9f),
            new("Hello World", "Lorem ipsum dolor sit amet", 0.5f),
            new("Hello World", "Lorem ipsum dolor sit amet", 0.1f),
        };

        return new SearchResult(items, query);
    }
}

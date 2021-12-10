namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        // Modifique este método para responder a la búsqueda

        SearchItem[] items = new SearchItem[3] {
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet"),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet"),
            new SearchItem("Hello World", "Lorem ipsum dolor sit amet"),
        };

        return new SearchResult(items, query);
    }
}

namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query)
    {
        SearchItem[] Elemento= new SearchItem[ReadFolder().Length];
        string[] read = ReadFolder();
        for (int i = 0; i < ReadFolder().Length; i++)
        {
            if (read[i]==query)
            {
                Elemento[i]= new SearchItem(read[i], "snippet", 0.2f);
            }
            
        }

        return new SearchResult(Elemento, query);
    }
    
    //Método para leer la carpeta y ver los archivos que contiene.
    public static string[] ReadFolder()
    {

        DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"/Carpeta archivos");
        FileInfo[] files = dir.GetFiles("*.txt");

        string[] Archivo = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            
            Archivo[i]=files[i].Name;
            
        }

        // string [] files = DirectoryInfo.GetFiles(Directory.GetCurrentDirectory()+@"/Carpeta archivos","*.txt");
        // foreach (var file in files)
        // {
        //     Console.WriteLine(file);
        // }
        // SearchItem[] Archivo = new SearchItem[files.Length];

        // for (int i = 0; i < files.Length; i++)
        // {
        //     Archivo[i] = new SearchItem(Path.GetFileName(files[i]),"snippet",(float)i);
        // }

        return Archivo;
    }

    //Método para leer los archivos de la carpeta.
    


}

namespace Sermone.Languages
{
    public class Portuguese : ILang
    {
        public string EnglishLanguageName => "Portuguese (Brazil)";
        public string NativeLanguageName => "Português (Brasil)";

        //Dropdown
        public string File => "Arquivo";
        public string Open => "Abrir";
        public string Save => "Salvar";

        //Nav Bar
        public string Search => "Procurar";

        //Loading Page
        public string PluginList => "lista de plugins";
        public string LoadingPlugin => "Carregando...";

        public string LoadingDesc => "Inicializando {0}...";

        //Buttons
        public string Next => "Avançar >";
        public string Back => "< Retornar";
    }
}

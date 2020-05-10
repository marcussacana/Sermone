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
        public string Loading => "Carregando...";
        public string LoadingDesc => "Inicializando {0}...";
        public string RefreshingDesc => "Atualizando {0}...";
        public string PluginList => "lista de plugins";

        //Buttons
        public string Next => "Avançar >";
        public string Back => "< Retornar";

        //About
        public string AboutVersion => "Versão Protótipo";
        public string AboutDesc => "Essa é uma versão protótipo do software Sermone voltada para fins experimentais e como tal carece de recursos mais avançados para um uso prático, pode também contar com erros no decorrer do uso.";
    }
}

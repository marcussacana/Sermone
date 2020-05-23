namespace Sermone.Languages
{
    public class Portuguese : ILang
    {
        public string EnglishLanguageName => "Portuguese (Brazil)";
        public string NativeLanguageName => "Português (Brasil)";

        //Dropdown
        public string File => "Arquivo";
        public string Open => "Abrir";
        public string OpenAs => "Abrir Como";
        public string Save => "Salvar";
        public string Filter => "Filtro";
        public string AutoSelect => "Seleção Automática";
        public string SelectAll => "Selecionar Tudo";
        public string DeselectAll => "Desselecionar Tudo";

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
        public string AboutDesc => "Essa é uma versão protótipo do software voltada para fins experimentais e como tal carece de recursos mais avançados para um uso prático, pode também contar com erros no decorrer do uso.";

        //Plugin Picker
        public string SelectAPlugin => "Selecione um Plugin";
        public string Name => "Nome";
        public string Extension => "Extensão";
        public string ReadWrite => "Leitura/Escrita";
        public string ReadOnly => "Apenas Leitura";
        public string WriteOnly => "Apenas Escrita";

        //Notifications
        public string BackgroundTaskWarning => "O Sermone está trabalhando em segundo plano, evite fechar-lo sem confirmar a conclusão da tarefa.";
        public string NoResultsFound => "Nenhum Resultado Encontrado";
        public string TryOthersWords => "Tente novamente com outras palavras.";
        public string NotSupported => "Não Suportado";
        public string PluginDontSupport => "O Plugin selecionado não suporta este arquivo.";
        public string Success => "Sucesso";
        public string BackupUpdated => "Backup Atualizado";

        //Settings
        public string Settings => "Configurações";
        public string Language => "Linguagem";
        public string Username => "Nome de Usuário";
        public string Password => "Senha";
        public string BackupOn => "Backup Em";

        public string DennyPattern => "Padrões Inaceitáveis";
        public string IgnorePattern => "Padrões Ignoráveis";
        public string QuotesPattern => "Padrões de Citação";
        public string Breakline => "Quebra de Linha";
        public string AcceptableRange => "Alcance Tolerado";
        public string AsianMode => "Modo Asiático";
        public string Sensitivity => "Sensibilidade";

        //Settings Tooltips
        public string DenyPatternTooltip => "Lista de padrões que nunca aparecem em um díalogo, separado por ponto e vírgula.";
        public string IgnorePatternTooltip => "Lista de padrões especiais que aparecem em um diálogo e devem ser tolerados pelo filtro, separado por ponto e vírgula.";
        public string QuotesPatternTooltip => "Lista de sequências de citações do díalogo, composta por dois caractéres, sendo o primeiro a abertura, o segundo o fechamento, separado por ponto e vírgula.";
        public string BreaklineTooltip => "Caractere especial usado no jogo para quebrar a linha de diálogo, conteúdo escapado.";
        public string AcceptableRangeTooltip => "Lista de alcances dos caractéres usados nos diálogos, use um hífem para declarar um alcance do primeiro até o ultimo caractere, conteúdo não separado.";
        public string AsianModeTooltip => "Otimiza o filtro de díalogos para textos em japonês/chinês";
        public string SensitivityTooltip => "Determina a rigidez do algoritmo de filtro de diálogos, onde um numero maior lhe torna mais tolerante, e um numero menor o oposto.";

        //Dialogs
        public string AreYouSure => "Você tem certeza?";
        public string LoadBackupWarn => "Se você carregar este backup todas as alterações atuais do script serão perdidas";
    }
}

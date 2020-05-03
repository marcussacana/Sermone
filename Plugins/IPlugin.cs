namespace Sermone.Plugins
{
    public interface IPluginCreator
    {
        public IPlugin Create(byte[] Script);
    }

    public interface IPlugin {
        public string[] Import();
        public byte[] Export();
    }
}
